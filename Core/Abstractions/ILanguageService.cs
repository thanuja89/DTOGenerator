namespace Core.Abstractions
{
    public interface ILanguageService
    {
        string GetPropertyType(string sourceType);
        string GetPropertyDeclaration(string name, string type);
    }
}
