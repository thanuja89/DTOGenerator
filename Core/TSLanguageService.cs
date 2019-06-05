using Core.Abstractions;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class TSLanguageService : ILanguageService
    {
        public TSLanguageService(GenOptions options)
        {
            _options = options;
        }

        private readonly Dictionary<string, string> _mappings = new Dictionary<string, string>()
        {
            { "int", "number" },
            { "string", "string" },
            { "DateTime", "Date" },
            { "bool", "boolean" }
        };
        private readonly GenOptions _options;

        public string GetSimpleType(string sourceType)
        {
            _mappings.TryGetValue(sourceType, out string val);

            return val ?? sourceType;
        }

        public string GetPropertyDeclaration(string name, string type)
        {
            string typeStr = GetPropertyType(type);

            return $"public {name}: {typeStr};";
        }

        public string GetPropertyType(string sourceType)
        {
            var innerTypes = Regex.Split(sourceType, "<|>");

            if (innerTypes.Length <= 1)
            {
                return GetSimpleType(sourceType);
            }

            return MakeGenericType(innerTypes);
        }

        private string MakeGenericType(string[] types)
        {
            bool isNonCollectionTypeFound = false;
            int collectionCount = 0, nonCollectionCount = 0;

            StringBuilder resultBuilder = new StringBuilder();

            foreach (var type in types)
            {
                if (string.IsNullOrWhiteSpace(type))
                    continue;

                var isCollection = Regex.IsMatch(type, @"(IEnumerable)|(I?Collection)|(I?Dictionary)|(I?List)");

                if (!isNonCollectionTypeFound && isCollection)
                    collectionCount++;
                else
                {
                    isNonCollectionTypeFound = true;
                    nonCollectionCount++;

                    var simpleType = GetSimpleType(type);
                    resultBuilder.Append($"{simpleType}<"); // must remove extra one
                }
            }

            resultBuilder.Remove(resultBuilder.Length - 1, 1);

            var closingTags = new string('>', nonCollectionCount - 1);
            resultBuilder.Append(closingTags);

            resultBuilder.Insert(resultBuilder.Length, "", collectionCount);

            return resultBuilder.ToString();
        }
    }
}
