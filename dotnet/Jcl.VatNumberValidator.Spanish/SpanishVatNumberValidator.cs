using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jcl.VatNumberValidator
{
    public class SpanishVatNumberValidator : IVatNumberValidator, ISpanishVatNumberValidator
    {
        private const string DniRegex = @"^(\d{8})([A-Z])$";
        private const string CifRegex = @"^([ABCDEFGHJKLMNPQRSUVW])(\d{7})([0-9A-J])$";
        private const string NieRegex = @"^[XYZ]\d{7}[A-Z]$";

        public string Name => "Spanish Vat Number Validator";
        public string Description => "Spanish Vat (DNI, NIE, CIF) Number Validator by Javier Campos valid by 2019. MIT Licensed";

        public string Normalize(string vatNumber)
        {
            if (vatNumber == null) throw new ArgumentNullException(nameof(vatNumber));
            vatNumber = Regex.Replace(vatNumber.ToUpperInvariant(), @"[\W_]+", "");
            if(vatNumber.Length < 1) return vatNumber;

            // Pad short DNIs with zeros
            if(!char.IsLetter(vatNumber[0]) && vatNumber.Length < 9)
                return vatNumber.PadLeft(9, '0');

            // Do not pad NIE or CIF, they should always be 9 digits
            return vatNumber;
        }

        public bool Validate(string vatNumber, bool normalize = true)
        {
            if (vatNumber == null) throw new ArgumentNullException(nameof(vatNumber));
            if (normalize) vatNumber = Normalize(vatNumber);
            if(vatNumber.Length != 9) return false;

            if (IsDni(vatNumber))
                return ValidateDni(vatNumber);
            if (IsNie(vatNumber))
                return ValidateNie(vatNumber);
            if (IsCif(vatNumber))
                return ValidateCif(vatNumber);
            return false;
        }

        public bool IsDni(string vatNumber) => Regex.IsMatch(vatNumber, DniRegex);
        public bool IsNie(string vatNumber) => Regex.IsMatch(vatNumber, NieRegex);
        public bool IsCif(string vatNumber) => Regex.IsMatch(vatNumber, CifRegex);

        public bool ValidateDni(string vatNumber, bool normalize = true)
        {
            const string dniLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

            if (vatNumber == null) throw new ArgumentNullException(nameof(vatNumber));
            if (normalize) vatNumber = Normalize(vatNumber);
            if (vatNumber.Length != 9) return false;
            if (!IsDni(vatNumber)) return false;

            var matches = Regex.Matches(vatNumber, DniRegex);
            if (!int.TryParse(matches[0].Groups[1].Value, out var integerVat))
                return false;
            return matches[0].Groups[2].Value == dniLetters[integerVat % 23].ToString();
        }

        public bool ValidateNie(string vatNumber, bool normalize = true)
        {
            if (vatNumber == null) throw new ArgumentNullException(nameof(vatNumber));
            if (normalize) vatNumber = Normalize(vatNumber);
            if (vatNumber.Length != 9) return false;
            if (!IsNie(vatNumber)) return false;

            var prefix = vatNumber[0].ToString();
            prefix = "XYZ".IndexOf(prefix, StringComparison.Ordinal).ToString();
            return ValidateDni($"{prefix}{vatNumber.Substring(1)}");
        }

        public bool ValidateCif(string vatNumber, bool normalize = true)
        {
            const string controlDigits = "JABCDEFGHI";
            const string ccMustBeLetterFirstDigits = "PQRSW";

            if (vatNumber == null) throw new ArgumentNullException(nameof(vatNumber));
            if (normalize) vatNumber = Normalize(vatNumber);
            if (vatNumber.Length != 9) return false;
            if (!IsCif(vatNumber)) return false;

            var digits = vatNumber.Substring(1, 7);
            var lastDigit = vatNumber.Substring(8, 1);
            var totalSum = digits
                .Select((c, i) => (int) char.GetNumericValue(c) * (i % 2 == 0 ? 2 : 1))
                .Sum(digit => digit / 10 + digit % 10);
            var cPos = totalSum % 10;
            if (cPos != 0) cPos = 10 - cPos;
            if (int.TryParse(lastDigit, out var lastNumber))
                return !ccMustBeLetterFirstDigits.Contains(vatNumber[0]) && lastNumber == cPos;
            return lastDigit == controlDigits[cPos].ToString();
        }
    }
}