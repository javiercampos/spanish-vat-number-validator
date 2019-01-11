using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Jcl.VatNumberValidator.Tests
{
    public class SpanishVatNumberValidatorMust
    {
        private readonly IVatNumberValidator _generalValidator; 
        private readonly ISpanishVatNumberValidator _validator;

        public SpanishVatNumberValidatorMust()
        {
            var validator = new SpanishVatNumberValidator();
            _validator = validator;
            _generalValidator = validator;
        }

        private static void AssertCollectionTrue(IEnumerable<string> collection, Func<string, bool, bool> method) =>
            AssertCollection(collection, Assert.True, method);

        private static void AssertCollectionFalse(IEnumerable<string> collection, Func<string, bool, bool> method) =>
            AssertCollection(collection, Assert.False, method);

        private static void AssertCollection(IEnumerable<string> collection, Action<bool> assertion,
            Func<string, bool, bool> method) =>
            Assert.All(collection, x => assertion(method(x, true)));

        private IEnumerable<string> RandomString(int minSize, int maxSize, int count)
        {
            if(minSize == 0) throw new ArgumentException("Invalid min size", nameof(minSize));
            if(maxSize < minSize) throw new ArgumentException("Invalid max size", nameof(maxSize));

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            for(var c = 0; c < count; c++)
            {
                yield return new string(Enumerable.Range(0, random.Next(minSize, maxSize))
                    .Select(_ => chars[random.Next(chars.Length)]).ToArray());
            }
        }

        [Fact]
        public void ReturnTrueOnCorrectAnyValidation()
        {
            var testData = 
                SpanishTestData.ValidDnis
                .Concat(SpanishTestData.ValidCifs)
                .Concat(SpanishTestData.ValidNies)
                .Concat(SpanishTestData.RandomValidDni(100))
                .Concat(SpanishTestData.RandomValidNie(100))
                .Concat(SpanishTestData.RandomValidCif(100));
                   
            AssertCollectionTrue(testData, _generalValidator.Validate);
        }

        [Fact]
        public void ReturnFalseOnIncorrectAnyValidation()
        {
            var testData = 
                SpanishTestData.InvalidDnis
                    .Concat(SpanishTestData.InvalidCifs)
                    .Concat(SpanishTestData.InvalidNies)
                    // Jcl: Warning! this could ocasionally generate a correct dni or cif
                    // If it ever happens, an incorrect pass would be something to celebrate!
                    .Concat(RandomString(5, 12, 100));
                   
            AssertCollectionFalse(testData, _generalValidator.Validate);
        }


        [Fact]
        public void ReturnTrueOnCorrectDniValidation()
        {
            AssertCollectionTrue(SpanishTestData.ValidDnis, _validator.ValidateDni);
            AssertCollectionTrue(SpanishTestData.RandomValidDni(100), _validator.ValidateDni);
        }

        [Fact]
        public void ReturnTrueOnCorrectNieValidation()
        {
            AssertCollectionTrue(SpanishTestData.ValidNies, _validator.ValidateNie);
            AssertCollectionTrue(SpanishTestData.RandomValidNie(100), _validator.ValidateNie);
        }

        [Fact]
        public void ReturnTrueOnCorrectCifValidation()
        {
            AssertCollectionTrue(SpanishTestData.ValidCifs, _validator.ValidateCif);
            AssertCollectionTrue(SpanishTestData.RandomValidCif(100), _validator.ValidateCif);
        }

        [Fact]
        public void ReturnFalseOnIncorrectDniInvalidation()
        {
            AssertCollectionFalse(SpanishTestData.InvalidDnis, _validator.ValidateDni);
        }

        [Fact]
        public void ReturnFalseOnIncorrectNieInvalidation()
        {
            AssertCollectionFalse(SpanishTestData.InvalidNies, _validator.ValidateNie);
        }

        [Fact]
        public void ReturnFalseOnIncorrectCifInvalidation()
        {
            AssertCollectionFalse(SpanishTestData.InvalidCifs, _validator.ValidateCif);
        }
    }
}