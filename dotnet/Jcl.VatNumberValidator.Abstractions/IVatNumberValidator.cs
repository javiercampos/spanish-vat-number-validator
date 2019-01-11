namespace Jcl.VatNumberValidator
{
    public interface IVatNumberValidator
    {
        string Name { get; }
        string Description { get; }

        string Normalize(string vatNumber);
        bool Validate(string vatNumber, bool normalize = true);
    }
}
