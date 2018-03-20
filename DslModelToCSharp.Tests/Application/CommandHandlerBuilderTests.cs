﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModelToCSharp.Application;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Application
{
    [TestFixture]
    public class CommandHandlerBuilderTests : TestBase
    {
        [Test]
        public void Build()
        {
            var commandHandlerBuilder = new CommandHandlerBuilder(ApplicationNameSpace);

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                foreach (var domainClass in domainTree.Classes)
                {
                    var codeNamespace = commandHandlerBuilder.Build(domainClass);
                    new FileWriter(ApplicationBasePath).WriteToFile(codeNamespace.Types[0].Name, domainClass.Name + "s", codeNamespace);
                }
            }

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(ApplicationBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Posts/PostCommandHandler.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Application/Posts/PostCommandHandler.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../ApplicationExpected/Generated/Users/UserCommandHandler.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Application/Users/UserCommandHandler.g.cs"), @"\s+", String.Empty));
        }
    }
}