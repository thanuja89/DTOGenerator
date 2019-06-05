using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Walkers
{
    public class PropertyCollectorWalker : CSharpSyntaxWalker
    {
        private readonly List<string> _propertyDefs = new List<string>();

        public PropertyCollectorWalker()
        {

        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (node.Modifiers.Any(m => m.Text == "public"))
                _propertyDefs.Add($"public {node.Identifier}: {node.Type.ToString()};");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var prop in _propertyDefs)
            {
                sb.AppendLine($"\t{prop}");
            }

            return sb.ToString();
        }
    }
}
