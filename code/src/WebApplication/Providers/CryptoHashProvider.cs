using System.Security.Cryptography;
using System.Text;

namespace WebApplication.Providers
{
    public class CryptoHashProvider : IHashProvider
    {
        public Task<string> GenerateHash<THashAlgorithm>(string input)
            where THashAlgorithm : HashAlgorithm, new()
        {
            var hashAlgorithm = Activator.CreateInstance<THashAlgorithm>();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var outputBytes = hashAlgorithm.ComputeHash(inputBytes);
            var outputBuilder = new StringBuilder(2 * outputBytes.Length);

            foreach (var outputByte in outputBytes)
            {
                outputBuilder.Append(outputByte.ToString("x2"));
            }

            var output = outputBuilder.ToString();
            return Task.FromResult(output);
        }
    }
}
