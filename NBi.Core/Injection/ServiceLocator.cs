﻿using NBi.Core.Injection;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using NBi.Core.Query.Session;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Injection
{
    public class ServiceLocator
    {
        private readonly IKernel kernel;

        public ServiceLocator()
        {
            kernel = new StandardKernel(new ConfigurationModule(), new QueryModule(), new ResolverModule());
        }

        public SessionFactory GetSessionFactory()
        {
            return kernel.Get<SessionFactory>();
        }

        public CommandFactory GetCommandFactory()
        {
            return kernel.Get<CommandFactory>();
        }

        public ExecutionEngineFactory GetExecutionEngineFactory()
        {
            return kernel.Get<ExecutionEngineFactory>();
        }

        public ResultSetResolverFactory GetResultSetResolverFactory()
        {
            return kernel.Get<ResultSetResolverFactory>();
        }
        
        public QueryResolverFactory GetQueryResolverFactory()
        {
            return kernel.Get<QueryResolverFactory>();
        }

        public ScalarResolverFactory GetScalarResolverFactory()
        {
            return kernel.Get<ScalarResolverFactory>();
        }

        public Configuration.Configuration GetConfiguration()
        {
            return kernel.Get<Configuration.Configuration>();
        }
    }
}
