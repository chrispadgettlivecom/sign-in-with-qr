using System.Security.Cryptography;

namespace WebApplication.Providers
{
    public interface IHashProvider
    {
        Task<string> GenerateHash<THashAlgorithm>(string input)
            where THashAlgorithm : HashAlgorithm, new();
    }
}
