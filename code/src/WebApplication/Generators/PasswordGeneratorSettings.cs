using System.Text;

namespace WebApplication.Generators
{
    public class PasswordGeneratorSettings
    {
        private const string DigitCharacters = "0123456789";
        private const string LowercaseLetterCharacters = "abcdefghijklmnopqrstuvwxyz";
        private const string SymbolCharacters = @"!#$%&*@";
        private const string UppercaseLetterCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public PasswordGeneratorSettings(
            bool includeUppercaseLetters = true,
            bool includeLowercaseLetters = true,
            bool includeDigits = true,
            bool includeSymbols = false,
            int passwordLength = 12)
        {
            IncludeUppercaseLetters = includeUppercaseLetters;
            IncludeLowercaseLetters = includeLowercaseLetters;
            IncludeDigits = includeDigits;
            IncludeSymbols = includeSymbols;
            PasswordLength = passwordLength;

            var characterSetBuilder = new StringBuilder();

            if (includeLowercaseLetters)
            {
                characterSetBuilder.Append(LowercaseLetterCharacters);
            }

            if (includeDigits)
            {
                characterSetBuilder.Append(DigitCharacters);
            }

            if (includeSymbols)
            {
                characterSetBuilder.Append(SymbolCharacters);
            }

            if (includeUppercaseLetters)
            {
                characterSetBuilder.Append(UppercaseLetterCharacters);
            }

            CharacterSet = characterSetBuilder.ToString();
            MaxPasswordGenerations = CharacterSet.Length;
        }

        public string CharacterSet { get; set; }

        public bool IncludeDigits { get; set; }

        public bool IncludeLowercaseLetters { get; set; }

        public bool IncludeUppercaseLetters { get; set; }

        public bool IncludeSymbols { get; set; }

        public int MaxPasswordGenerations { get; set; }

        public int PasswordLength { get; set; }
    }
}
