﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModel.Application;
using DslModelToCSharp.Application;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Application
{
    [TestFixture]
    public class HookResultBuilderTests : TestBase
    {
        [Test]
        public void Write()
        {
            var hookResultBuilder = new HookResultBuilder(ApplicationNameSpace);

            var codeNamespace = hookResultBuilder.Write(new HookResultBaseClass());

            new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, "Base", codeNamespace);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Base/HookResult.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Base/HookResult.g.cs"), @"\s+", String.Empty));
        }
    }
}