namespace Jcl.VatNumberValidator
{
    public interface ISpanishVatNumberValidator
    {
        string Name { get; }
        string Description { get; }
        string Normalize(string vatNumber);
        bool Validate(string vatNumber, bool normalize = true);
        bool IsDni(string vatNumber);
        bool IsNie(string vatNumber);
        bool IsCif(string vatNumber);
        bool ValidateDni(string vatNumber, bool normalize = true);
        bool ValidateNie(string vatNumber, bool normalize = true);
        bool ValidateCif(string vatNumber, bool normalize = true);
    }
}