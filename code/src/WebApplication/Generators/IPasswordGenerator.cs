namespace WebApplication.Generators
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(int passwordGenerationCount, PasswordGeneratorSettings passwordGeneratorSettings);
    }
}
