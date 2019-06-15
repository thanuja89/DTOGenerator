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

        public Generator(GenOptions options, ILanguageService service)
        {
            _options = options;
            _languageService = service;
        }

        public async Task<string> GenerateAsync(string source)
        {
            var opts = new CSharpParseOptions(LanguageVersion.Latest, DocumentationMode.None, SourceCodeKind.Regular);

            var tree = CSharpSyntaxTree.ParseText(source, opts);

            var root = await tree.GetRootAsync();

            var classes = root.ChildNodes()
                .OfType<ClassDeclarationSyntax>()
                .ToList();

            if (classes.Count == 0)
                throw new ArgumentException("No root class found", nameof(source));

            var sb = new StringBuilder();

            var i = 0;

            foreach (var @class in classes)
            {
                var walker = new PropertyCollector();

                walker.Visit(@class);

                var result = ProcessProperties(walker.Properties);

                var classStr = _languageService.GetContainerDeclaration(@class.Identifier.Text, result);

                sb.Append(classStr);

                if(++i != classes.Count)
                    sb.AppendLine(Environment.NewLine);
            }

            return sb.ToString();
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
