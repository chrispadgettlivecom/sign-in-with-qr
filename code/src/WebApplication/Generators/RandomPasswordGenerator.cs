namespace WebApplication.Generators
{
    public class RandomPasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword(int passwordGenerationCount, PasswordGeneratorSettings passwordGeneratorSettings)
        {
            var password = new char[passwordGeneratorSettings.PasswordLength];
            var characterSet = passwordGeneratorSettings.CharacterSet;

            var random = new Random();

            for (var characterPosition = 0; characterPosition < passwordGeneratorSettings.PasswordLength; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(passwordGenerationCount, characterSet.Length)];
            }

            return string.Join(null, password);
        }
    }
}
