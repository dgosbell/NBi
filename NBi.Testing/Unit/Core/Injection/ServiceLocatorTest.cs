﻿using NBi.Core.Configuration;
using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using NBi.Core.Query.Client;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Extensibility;

namespace NBi.Testing.Unit.Core.Injection
{
    public class ServiceLocatorTest
    {
        #region Fake
        private class FakeSessionFactory : IClientFactory
        {
            public bool CanHandle(string connectionString) => true;

            public IClient Instantiate(string connectionString) => throw new NotImplementedException();
        }

        private class FakeCommandFactory : ICommandFactory
        {
            public bool CanHandle(IClient session) => true;

            public IClient Instantiate(string connectionString) => throw new NotImplementedException();

            public ICommand Instantiate(IClient session, IQuery query, ITemplateEngine engine) => throw new NotImplementedException();
        }

        [SupportedCommandType(typeof(object))]
        private class FakeExecutionEngine : IExecutionEngine
        {
            public DataSet Execute() => throw new NotImplementedException();

            public IEnumerable<T> ExecuteList<T>() => throw new NotImplementedException();

            public object ExecuteScalar() => throw new NotImplementedException();
        }
        #endregion

        [Test]
        public void GetSessionFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetSessionFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<ClientProvider>(obj);
        }

        [Test]
        public void GetSessionFactory_Singleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetSessionFactory();
            var obj2 = locator.GetSessionFactory();
            Assert.That(obj1, Is.EqualTo(obj2));
        }

        [Test]
        public void GetSessionFactory_CantAddTwiceTheSameFactory()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetSessionFactory();
            var obj2 = locator.GetSessionFactory();
            obj1.RegisterFactories(new[] { typeof(FakeSessionFactory) });
            Assert.Throws<ArgumentException>(() => obj2.RegisterFactories(new[] { typeof(FakeSessionFactory) }));
        }

        [Test]
        public void GetCommandFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetCommandFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<CommandProvider>(obj);
        }

        [Test]
        public void GetCommandFactory_Singleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetCommandFactory();
            var obj2 = locator.GetCommandFactory();
            Assert.That(obj1, Is.EqualTo(obj2));
        }

        [Test]
        public void GetResultSetResolverFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetResultSetResolverFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<ResultSetResolverFactory>(obj);
        }

        [Test]
        public void GetResultSetResolverFactory_NotSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetResultSetResolverFactory();
            var obj2 = locator.GetResultSetResolverFactory();
            Assert.That(obj1, Is.Not.EqualTo(obj2));
        }

        [Test]
        public void GetResultSetResolverFactory_UnderlyingServiceLocatorIsSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetResultSetResolverFactory();
            var obj2 = locator.GetResultSetResolverFactory();

            var field = typeof(ResultSetResolverFactory).GetField("serviceLocator", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceLocator1 = field.GetValue(obj1);
            var serviceLocator2 = field.GetValue(obj2);

            Assert.That(serviceLocator1, Is.EqualTo(serviceLocator2));
        }

        [Test]
        public void GetQueryResolverFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetQueryResolverFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<QueryResolverFactory>(obj);
        }

        [Test]
        public void GetQueryResolverFactory_NotSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetQueryResolverFactory();
            var obj2 = locator.GetQueryResolverFactory();
            Assert.That(obj1, Is.Not.EqualTo(obj2));
        }

        [Test]
        public void GetQueryResolverFactory_UnderlyingServiceLocatorIsSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetQueryResolverFactory();
            var obj2 = locator.GetQueryResolverFactory();

            var field = typeof(QueryResolverFactory).GetField("serviceLocator", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceLocator1 = field.GetValue(obj1);
            var serviceLocator2 = field.GetValue(obj2);

            Assert.That(serviceLocator1, Is.EqualTo(serviceLocator2));
        }

        [Test]
        public void GetScalarResolverFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetScalarResolverFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<ScalarResolverFactory>(obj);
        }

        [Test]
        public void GetScalarResolverFactory_NotSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetScalarResolverFactory();
            var obj2 = locator.GetScalarResolverFactory();
            Assert.That(obj1, Is.Not.EqualTo(obj2));
        }
        [Test]
        public void GetScalarResolverFactory_UnderlyingServiceLocatorIsSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetScalarResolverFactory();
            var obj2 = locator.GetScalarResolverFactory();

            var field = typeof(ScalarResolverFactory).GetField("serviceLocator", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceLocator1 = field.GetValue(obj1);
            var serviceLocator2 = field.GetValue(obj2);

            Assert.That(serviceLocator1, Is.EqualTo(serviceLocator2));
        }

        [Test]
        public void GetConfiguration_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetConfiguration();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<IConfiguration>(obj);
        }

        [Test]
        public void GetConfiguration_Singleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetConfiguration();
            var obj2 = locator.GetConfiguration();
            Assert.That(obj1, Is.EqualTo(obj2));
        }


        [Test]
        public void GetExecutionEngineFactory_NoConfig_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetExecutionEngineFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<ExecutionEngineFactory>(obj);
        }

        [Test]
        public void GetExecutionEngineFactory_NoConfig_NotSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetExecutionEngineFactory();
            var obj2 = locator.GetExecutionEngineFactory();
            Assert.That(obj1, Is.Not.EqualTo(obj2));
        }

        [Test]
        public void GetExecutionEngineFactory_Config_ConfigAvailable()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetExecutionEngineFactory();
            Assert.That(obj1.ExtensionCount, Is.EqualTo(0));

            var config = locator.GetConfiguration();
            config.LoadExtensions(new List<Type>() { typeof(FakeSessionFactory), typeof(FakeCommandFactory), typeof(FakeExecutionEngine) });

            var obj2 = locator.GetExecutionEngineFactory();
            Assert.That(obj2.ExtensionCount, Is.EqualTo(1));
        }
    }
}
