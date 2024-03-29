﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Core.Walkers
{
    public class PropertyCollector : CSharpSyntaxWalker
    {
        private readonly List<(string name, string type)> _propertyDefs = new List<(string name, string type)>();

        public IReadOnlyCollection<(string name, string type)> Properties => _propertyDefs.AsReadOnly();

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (node.Modifiers.Any(m => m.Kind() == SyntaxKind.PublicKeyword))
                _propertyDefs.Add((node.Identifier.Text, node.Type.ToString()));
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (node.Modifiers.Any(m => m.Kind() == SyntaxKind.PublicKeyword))
            {
                VariableDeclarationSyntax variableDeclaration = node.Declaration;

                foreach (var variable in variableDeclaration.Variables)
                {
                    _propertyDefs.Add((variable.Identifier.Text, variableDeclaration.Type.ToString()));
                }
            }
                
        }
    }
}
