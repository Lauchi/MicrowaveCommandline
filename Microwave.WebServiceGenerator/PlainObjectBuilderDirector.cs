﻿using System.CodeDom;

namespace Microwave.WebServiceGenerator
{
    public class PlainObjectBuilderDirector
    {
        public CodeNamespace BuildInstance(IPlainDataObjectBuilder builder)
        {
            var nameSpace = builder.BuildNameSpace();
            var targetClass = builder.BuildClassType();
            nameSpace.Types.Add(targetClass);
            builder.AddClassProperties(targetClass);
            builder.AddConstructor(targetClass);
            builder.AddBaseTypes(targetClass);
            return nameSpace;
        }
    }
}