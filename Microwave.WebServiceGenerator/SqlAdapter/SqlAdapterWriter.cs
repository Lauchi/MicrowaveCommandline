﻿using System.Linq;
using Microwave.LanguageModel;

namespace Microwave.WebServiceGenerator.SqlAdapter
{
    public class SqlAdapterWriter
    {
        private readonly string _sqlAdapterNameSpace;
        private readonly string _basePath;
        private readonly IFileWriter _fileWriter;
        private RepositoryBuilder _repositoryBuilder;
        private EventStoreContextBuilder _eventStoreContextBuilder;
        private ClassFactory _classFactory;
        private HangfireContextBuilder _hangfireContextBuilder;

        public SqlAdapterWriter(string sqlAdapterNameSpace, string basePath)
        {
            _sqlAdapterNameSpace = sqlAdapterNameSpace;
            _basePath = basePath;
            _fileWriter = new FileWriter(basePath);
            _repositoryBuilder = new RepositoryBuilder(sqlAdapterNameSpace);
            _eventStoreContextBuilder = new EventStoreContextBuilder(sqlAdapterNameSpace);
            _classFactory = new ClassFactory();
            _hangfireContextBuilder = new HangfireContextBuilder(sqlAdapterNameSpace);
        }

        public void Write(DomainTree domainTree)
        {
            foreach (var domainClass in domainTree.Classes)
            {
                var repo = _repositoryBuilder.Build(domainClass);
                _fileWriter.WriteToFile(repo.Types[0].Name, $"{domainClass.Name}s", repo);
            }

            var dbContext = _eventStoreContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile(dbContext.Types[0].Name, "Base", dbContext);

            var hangfireContext = _hangfireContextBuilder.Build(domainTree.Classes.ToList());
            _fileWriter.WriteToFile(hangfireContext.Types[0].Name, "Base", hangfireContext);

            var eventStoreRepo = _classFactory.BuildInstance(new EventStoreRepositoryBuilder(_sqlAdapterNameSpace));
            _fileWriter.WriteToFile(eventStoreRepo.Types[0].Name, "Base", eventStoreRepo);
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(_basePath);
        }
    }
}