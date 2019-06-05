using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Core.Walkers
{
    public class PropertyCollectorWalker : CSharpSyntaxWalker
    {
        private readonly List<(string name, string type)> _propertyDefs = new List<(string name, string type)>();

        public IReadOnlyCollection<(string name, string type)> Properties => _propertyDefs.AsReadOnly();

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (node.Modifiers.Any(m => m.Text == "public"))
                _propertyDefs.Add((node.Identifier.ToString(), node.Type.ToString()));
        }
    }
}
