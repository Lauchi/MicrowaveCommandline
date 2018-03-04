﻿using System.CodeDom;
using GenericWebServiceBuilder.DslModel;

namespace GenericWebServiceBuilder.DslToCSharp
{
    internal class InterfaceParser : IInterfaceParser
    {
        public CodeTypeDeclaration Parse(DomainClass userClass)
        {
            var iface = new CodeTypeDeclaration($"I{userClass.Name}") {IsInterface = true};

            foreach (var function in userClass.Methods)
            {
                var method = new CodeMemberMethod
                {
                    Name = function.Name,
                    ReturnType = new CodeTypeReference(function.ReturnType)
                };

                foreach (var parameter in function.Parameters)
                    method.Parameters.Add(new CodeParameterDeclarationExpression(parameter.Type, parameter.Name));

                iface.Members.Add(method);
            }

            return iface;
        }
    }

    public interface IInterfaceParser
    {
        CodeTypeDeclaration Parse(DomainClass userClass);
    }
}