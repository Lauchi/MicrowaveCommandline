﻿using System.Collections.Generic;
using Microwave.LanguageModel;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceModel.Application
{
    public class QueueRepositoryInterface : DomainClass
    {
        public QueueRepositoryInterface()
        {
            Name = "IQueueRepository";
            Methods = new List<DomainMethod>
            {
                new DomainMethod
                {
                    Name = "AddEventForJob",
                    ReturnType = "Task",
                    Parameters = {new Parameter {Name = "eventAndJob", Type = new EventAndJobClass().Name}}
                },
                new DomainMethod
                {
                    Name = "RemoveEventsFromQueue",
                    Parameters =
                    {
                        new Parameter
                        {
                            Name = "handledEvents",
                            Type = $"List<{new EventJobClass().Name}>"
                        }
                    },
                    ReturnType = "Task"
                },
                new DomainMethod
                {
                    Name = "GetEvents",
                    ReturnType = $"Task<List<{new EventJobClass().Name}>>",
                    Parameters =
                    {
                        new Parameter
                        {
                            Name = "jobName",
                            Type = $" string"
                        }
                    }
                }
            };
        }
    }
}