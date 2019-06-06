using Core.Abstractions;
using Core.Walkers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Generator
    {
        private readonly GenOptions _options;
        private readonly ILanguageService _languageService;

        public Generator(GenOptions options)
        {
            _options = options;
            _languageService = LanguageServiceFactory.Create(options);
        }

        public async Task<string> GenerateAsync(string source)
        {
            var opts = new CSharpParseOptions(LanguageVersion.Latest, DocumentationMode.None, SourceCodeKind.Regular);

            var tree = CSharpSyntaxTree.ParseText(source, opts);

            var root = await tree.GetRootAsync();

            var @class = root.ChildNodes()
                .OfType<ClassDeclarationSyntax>()
                .Single();

            if (@class == null)
                throw new ArgumentException("No root class found", nameof(source));

            var walker = new PropertyCollectorWalker();

            walker.Visit(@class);

            var result = ProcessProperties(walker.Properties);

            var classStr = _languageService.GetClassDeclaration(@class.Identifier.Text, result);

            return classStr;
        }

        private string ProcessProperties(IEnumerable<(string name, string type)> propertyDefs)
        {
            var sb = new StringBuilder();

            foreach (var (name, type) in propertyDefs)
            {
                var nameStr = _options.IsCamelCaseEnabled ? name.ToCamelCase() : name;

                string propStr = _languageService.GetPropertyDeclaration(nameStr, type);

                sb.Append(propStr);
            }

            return sb.ToString();
        }
    }
}
