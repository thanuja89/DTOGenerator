﻿namespace Core.Abstractions
{
    public interface ILanguageService
    {
        string GetContainerDeclaration(string name, string contents);
        string GetPropertyDeclaration(string name, string type);
        string GetPropertyType(string sourceType);
        string GetSimpleType(string sourceType);
    }
}
