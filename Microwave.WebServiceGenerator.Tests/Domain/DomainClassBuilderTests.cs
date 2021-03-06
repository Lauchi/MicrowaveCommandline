using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Domain;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Tests.Domain
{
    [TestClass]
    public class DomainClassWriterTests : TestBase
    {
        [TestMethod]
        public void BuildDomainClasses()
        {
            foreach (var domainClass in DomainTree.Classes)
            {
                var codeNamespace = new ClassBuilderDirector().BuildInstance(new DomainClassBuilder(domainClass));
                TestUtils.SnapshotTest(codeNamespace);
            }
        }

        [TestMethod]
        public void BuildDomainClassesFirstWrite()
        {
            foreach (var domainClass in DomainTree.Classes)
            {
                var domainClassFirstBuilder = new DomainClassFirstBuilder(domainClass);
                var codeNamespace = new ClassBuilderDirector().BuildInstance(domainClassFirstBuilder);
                TestUtils.SnapshotTest(codeNamespace, false);
            }
        }

        [TestMethod]
        public void BuildEvents()
        {
            foreach (var domainClass in DomainTree.Classes)
            {
                foreach (var domainEvent in domainClass.Events)
                {
                    var codeNamespace = new ClassBuilderDirector().BuildInstance(new DomainEventBuilder(domainClass, domainEvent));
                    TestUtils.SnapshotTest(codeNamespace);
                }
            }
        }

        [TestMethod]
        public void BuildCreationResultBase()
        {
            var creationResult = new ClassBuilderDirector().BuildInstance(new CreationResultBaseClassBuilder(new CreationResultBaseClass()));
            TestUtils.SnapshotTest(creationResult);
        }

        [TestMethod]
        public void BuildDomainEventBaseClass()
        {
            var classFactory = new ClassBuilderDirector();
            var baseClass = classFactory.BuildInstance(new DomainEventBaseClassBuilder(new DomainEventBaseClass()));
            TestUtils.SnapshotTest(baseClass);
        }
    }
}