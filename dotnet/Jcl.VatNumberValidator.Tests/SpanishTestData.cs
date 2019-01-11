using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jcl.VatNumberValidator.Tests
{
    /// <summary>
    /// Test data generated using https://generadordni.es
    /// </summary>
    internal static class SpanishTestData
    {
        public static IEnumerable<string> InvalidDnis => Dnis.Select(MakeInvalid).ToArray();
        public static IEnumerable<string> InvalidCifs => Cifs.Select(MakeInvalid).ToArray();
        public static IEnumerable<string> InvalidNies => Nies.Select(MakeInvalid).ToArray();
        public static IEnumerable<string> ValidDnis => Dnis;
        public static IEnumerable<string> ValidCifs => Cifs;
        public static IEnumerable<string> ValidNies => Nies;

        public static IEnumerable<string> RandomValidDni(int count)
        {
            var random = new Random();
            for(var i = 0; i < count; i++)
            {
                var numDni = random.Next(1000, 100000000);
                yield return $"{numDni}{"TRWAGMYFPDXBNJZSQVHLCKET"[numDni % 23]}";
            }            
        }

        public static IEnumerable<string> RandomValidNie(int count)
        {
            var random = new Random();
            for(var i = 0; i < count; i++)
            {
                var numNie = random.Next(1000, 10000000);
                var nieLetter = random.Next(3);
                var letterIndex = int.Parse($"{nieLetter}{numNie:D7}") % 23;
                yield return $"{"XYZ"[nieLetter]}{numNie:D7}{"TRWAGMYFPDXBNJZSQVHLCKET"[letterIndex]}";
            }            
        }

        public static IEnumerable<string> RandomValidCif(int count)
        {
            const string validLetters = "ABCDEFGHJNPQRSUVW";
            const string ccDigits = "JABCDEFGHI";
            const string ccDigitMustBeLetterCodes = "PQRSW";
            const string ccDigitMstBeNumberCodes = "ABEH";

            var random = new Random();
            for(var i = 0; i < count; i++)
            {
                var letter = validLetters[random.Next(validLetters.Length)];
                var residentCode = random.Next(0, 100);
                var inscriptionCode = random.Next(0, 100000);
                var ccIsLetter = ccDigitMustBeLetterCodes.Contains(letter) || residentCode == 0;
                // Jcl: there doesn't seem to be a consent on whether the control digit
                // should be a letter or a number if the first char is not PQRSW or ABEH
                // so we just randomize it :-)
                if(!ccIsLetter && !ccDigitMstBeNumberCodes.Contains(letter)) ccIsLetter = random.Next(2) == 0;
                
                var cifString = $"{residentCode:D2}{inscriptionCode:D5}";
                var totalSum = cifString
                    .Select((c, idx) => (int) char.GetNumericValue(c) * (idx % 2 == 0 ? 2 : 1))
                    .Sum(digit => digit / 10 + digit % 10);
                var cPos = totalSum % 10;
                if (cPos != 0) cPos = 10 - cPos;
                var controlDigit = ccIsLetter?ccDigits[cPos]:(char)('0'+cPos);
                var cif = $"{letter}{cifString}{controlDigit}";
                yield return cif;
            }            
        }

        private static string MakeInvalid(string vatNumber)
        {
            var sb = new StringBuilder(vatNumber)
            {
                [5] = (char) ('0' + ((int) char.GetNumericValue(vatNumber, 5) + 1) % 10)
            };
            return sb.ToString();
        }

        private static readonly string[] Dnis =
        {
            "90275465M", "32949828M", "87553330N", "6752999S", "13030171G", "77347964E", "25784916F", "77774075N",
            "80873665F", "91070811B", "58318104V", "57200519W", "81124382R", "14119603H", "86644790H", "61376281E",
            "24645483V", "67845026V", "12476910P", "95229254P", "80617211A", "63185017S", "78708631D", "51763178E",
            "9087683S", "63137384S", "46961528J", "80867900S", "82305428C", "43876227R", "41726937S", "7140532K",
            "34166784P", "91867114P", "1345569T", "21952230H", "55605888F", "97037594L", "14272536R", "86472290H",
            "24163508F", "59109402T", "57829811Z", "80069920C", "31453367Q", "90605813G", "15969767Q", "94978559J",
            "21247704M", "78645631Y", "94326527P", "13098364W", "50584302B", "73333112G", "85927427W", "86366828B",
            "2455146B", "18560323J", "40139046K", "73698390L", "67887692H", "3705909B", "58985523E", "26450016Q",
            "45245602W", "1109607H", "64835995F", "49688942H", "51224135F", "31834324R", "99132853R", "27730943G",
            "22113561G", "98901177G", "84075334F", "36027607Q", "86143885F", "98275058Z", "40473276S", "85734019R",
            "43096031B", "47095808L", "6197850V", "18792490H", "94990801L", "13409911Z", "33688165L", "27525519Q",
            "53848396Z", "18654307L", "55511887F", "50499929W", "43306380W", "29555398F", "8490894F", "88818466X",
            "56809308K", "7270287X", "45309236H"
        };

        private static readonly string[] Nies =
        {
            "Z8910692Z", "X3675428M", "X1007484S", "Y6717558P", "X9915758K", "X5086084W", "Z4407928Y", "Z7743118N",
            "Z4934237Y", "X0332517Y", "Z1817305B", "X1947637C", "Y9657569K", "Y2521747H", "Y6786030D", "X3200828X",
            "Z3188491Y", "Y3050079V", "X8484817W", "Z5836168S", "X2823873W", "Y2837916Y", "X0776056J", "X2311073X",
            "Z4584209S", "Z2177350Z", "Z5416171K", "X1574889X", "X1952287R", "Z5754854Y", "X8564565D", "X8938169R",
            "X0189533J", "Z9219762X", "X9313450Z", "Z8185078G", "Y9880175X", "Y0991179P", "X0948379C", "Y0449408W",
            "Y4958182V", "Z4198399F", "X1359739W", "Y0491112F", "Z7741237V", "X8870894R", "Z7595913F", "Y9270953N",
            "Z8516129Q", "Z3030138P", "Z3346157F", "Z0972544V", "Y5097269T", "Z2092832K", "Z1237023L", "Y6994205B",
            "Y6869334F", "Z0416513B", "X9268101K", "X5781305W", "Y0494781L", "Z2475684S", "Y7189671T", "X9932463M",
            "Z3203375D", "Z0167566Q", "Z4973572B", "Y7783463R", "Y9867162S", "Y5521154H", "Z3416718G", "Z1920566W",
            "X7782352A", "Y0309257J", "X9795581L", "Z9871184A", "X8564083X", "Z5496664Z", "Z8637173B", "Y4934320Y",
            "Z6472053L", "Z2190064D", "X1313699P", "Z1153479B", "Y8184088N", "Y6898389J", "Y3067869M", "Z8053593X",
            "Y6438754X", "Z9336907Q", "Z7003511Q", "Y1443789R", "X3430433Y", "X1710280T", "Y0595458W", "X0290311M",
            "Z9878160X", "X6380265L", "Y1855420R"
        };

        private static readonly string[] Cifs =
        {
            "B63520670", "B90702176", "A30339220", "B80851017", "B60864279", "B76895820", "B10447621", "A65187080",
            "B74807264", "A41932211", "N6675245B", "B10830263", "B31396948", "B50397652", "B54463260", "P4536055I",
            "B17051988", "B38770673", "B32917999", "B24381113", "B17530957", "F5830883D", "B20477022", "B05383245",
            "E46408027", "C8407803I", "S9412699B", "B39919535", "A89105456", "A63978225", "A35816412", "A66962630",
            "A82886391", "A40972432", "K2217434F", "A51147957", "B39564786", "A28848000", "B06413439", "A45175155",
            "B01538073", "B28369890", "A90644295", "P0291952J", "A99102626", "B83304360", "A89481683", "A94713567",
            "D5466368G", "A95102349", "B23314602", "B47275037", "A74355991", "A84061514", "E21197009", "A61827309",
            "A65616583", "Q9735411B", "A49700610", "B64770746", "A99928640", "B85030500", "B93638708", "B54811583",
            "B99141608", "D6117020E", "B66889288", "B24280810", "B50825637", "A59125294", "B64332265", "Q0258096G",
            "A54025846", "A87404349", "B93273423", "A62048335", "A13785639", "A26501254", "E58366501", "A92251453",
            "G4564662G", "A94776960", "P9863395A", "A84039320", "S2348245H", "A81539827", "B92069715", "A85316909",
            "A62135504", "F4854217I", "B59834903", "K5609874B", "D7032108H", "D3759651G", "B75293027", "D6169734H",
            "B11057783", "B72940620", "S1519053A"
        };
    }
}