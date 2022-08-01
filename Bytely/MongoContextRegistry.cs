using Bytely.Models;
using Bytely.Models.MongoContext;
using Bytely.Models.Sequence;
using Bytely.Models.Settings;
using MongoDB.Driver;

namespace Bytely.Web
{
    public static class MongoContextRegistry
    {
        public static void RegisterMongoContext(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            var mongoSettings = new MongoSettings();
            configuration.GetSection(nameof(MongoSettings)).Bind(mongoSettings);

            var sequenceSettings = new SequenceSettings();
            configuration.GetSection(nameof(SequenceSettings)).Bind(sequenceSettings);

            var context = new MongoContext();
            context.Client = new MongoClient(mongoSettings.ConnectionString);
            context.Database = context.Client.GetDatabase(mongoSettings.DatabaseName);
            context.BytelyUrl = context.Database.GetCollection<BytelyUrl>(mongoSettings.BytelyUrlCollectionName);
            context.Sequence = context.Database.GetCollection<SequenceEntry>(sequenceSettings.SequenceCollectionName);
            builder.Services.AddSingleton<IMongoContext>(context);

            InitializeSequence(sequenceSettings, context);
            InitializeIndexes(context);
        }

        private static void InitializeSequence(SequenceSettings settings, IMongoContext context)
        {
            var collections = context.Database!.ListCollectionNames().ToList();
            var sequenceCollectionName = context.Sequence!.CollectionNamespace.CollectionName;

            if (!collections.Contains(sequenceCollectionName))
                context.Database.CreateCollection(sequenceCollectionName);

            foreach (var sequenceInfo in settings.Sequences)
            {
                var sequenceExists = context.Sequence
                    .Find(r => r.Name == sequenceInfo.TargetCollectionName)
                    .Any();

                if (!sequenceExists)
                {
                    var sequenceEntry = new SequenceEntry
                    {
                        Name = sequenceInfo.TargetCollectionName,
                        Value = sequenceInfo.InitialId
                    };
                    context.Sequence.InsertOne(sequenceEntry);
                }
            }
        }

        private static void InitializeIndexes(IMongoContext context)
        {
            var options = new CreateIndexOptions() { Unique = true };
            var builder = Builders<BytelyUrl>.IndexKeys;
            var indexModel = new CreateIndexModel<BytelyUrl>(builder.Descending(r => r.UrlId), options);
            _ = context.BytelyUrl!.Indexes.CreateOne(indexModel);
        }
    }
}
