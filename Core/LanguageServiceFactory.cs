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
                    provider = new TSLanguageService(options);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return provider;
        }
    }

    public enum Language
    {
        TypeScript
    }
}