using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModel.Domain;
using DslModelToCSharp.Domain;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Domain
{
    [TestFixture]
    public class DomainClassWriterTests : TestBase
    {
        [Test]
        public void TestDomainClasses()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/User.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Users/User.g.cs"), @"\s+", String.Empty));
        }

        [Test]
        public void TestCreateEvents()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/UserCreateEvent.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/UserCreateEvent.g.cs"), @"\s+", String.Empty));
        }

        [Test]
        public void TestUpdateEvents()
        {
            var domainBuilder = new DomainWriter(DomainNameSpace, DomainBasePath, SolutionBasePath);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var domainTree = new DslParser(new Tokenizer(), new Parser()).Parse(content);
                domainBuilder.Build(domainTree, DomainBasePath);
            }

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateAgeEvent.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Users/UserUpdateAgeEvent.g.cs"), @"\s+", String.Empty));
            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Users/UserUpdateNameEvent.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Users/UserUpdateNameEvent.g.cs"), @"\s+", String.Empty));
        }

        [Test]
        public void CreationResultBase_Builder()
        {
            new DomainClassWriter(DomainNameSpace, DomainBasePath, SolutionBasePath).Write(new CreationResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Base/CreationResult.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Base/CreationResult.g.cs"), @"\s+", String.Empty));
        }

        [Test]
        public void DomainEventBaseClass_Builder()
        {
            var classFactory = new ClassFactory();
            var baseClass = classFactory.BuildInstance(new DomainEventBaseClassBuilder());
            new FileWriter(DomainBasePath).WriteToFile("DomainEventBase", "Base", baseClass);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Base/DomainEventBase.g.cs"), @"\s+", String.Empty),
            Regex.Replace(File.ReadAllText("Domain/Base/DomainEventBase.g.cs"), @"\s+", String.Empty));
        }
    }
}