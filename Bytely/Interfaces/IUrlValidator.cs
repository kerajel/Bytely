namespace Bytely.Core.Interfaces
{
    public interface IUrlValidator
    {
        bool IsUrlValid(string url, out Uri? uri);
    }
}
