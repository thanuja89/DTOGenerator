using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core
{
    public class TSLanguageService : ILanguageService
    {
        private readonly Dictionary<string, string> _mappings = new Dictionary<string, string>()
        {
            { "int", "number" },
            { "string", "string" },
            { "DateTime", "Date" },
            { "bool", "boolean" }
        };

        public string GetClassDeclaration(string name, string contents)
        {
            return 
$@"export class {name} {{{contents}
}}";
        }

        public string GetSimpleType(string sourceType)
        {
            if (IsCollection(sourceType))
                return "any[]";

            _mappings.TryGetValue(sourceType, out string val);

            return val ?? sourceType;
        }

        public string GetPropertyDeclaration(string name, string type)
        {
            string typeStr = GetPropertyType(type);

            return $"{Environment.NewLine}\tpublic {name}: {typeStr};";
        }

        public string GetPropertyType(string sourceType)
        {
            return GetTSType(sourceType);
        }

        private string GetTSType(string sourceType)
        {
            var match = Regex.Match(sourceType, "(?<type>[a-zA-Z_0-9()<>]+?)<(?<innerType>[a-zA-Z_0-9()<>]+)>");

            if (!match.Success)
                return GetSimpleType(sourceType);

            string type = match.Groups["type"].Value;
            string innerType = match.Groups["innerType"].Value;

            string innerTSType = GetTSType(innerType);

            if (IsCollection(type))
            {
                return $"{innerTSType}[]";
            }

            return $"{type}<{innerTSType}>";
        }

        private bool IsCollection(string type)
        {
            return Regex.IsMatch(type, @"^(IEnumerable)|(I?Collection)|(I?List)|(Stack)|(Queue)|(ArrayList)$");
        }
    }
}
