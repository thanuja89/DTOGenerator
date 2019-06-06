using Core.Abstractions;
using System;

namespace Core
{
    public class LanguageServiceFactory
    {
        public static ILanguageService Create(GenOptions options)
        {
            ILanguageService provider;

            switch (options.Language)
            {
                case Language.TypeScript:
                    provider = new TSLanguageService();
                    break;

                default:
                    throw new ArgumentException("Invalid language", nameof(options.Language));
            }

            return provider;
        }
    }

    public enum Language
    {
        TypeScript
    }
}