﻿#region Using directives
using System.Collections.Generic;
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Query;
using NBi.Core.Injection;
using NBi.Extensibility.Query;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class ExecutionMatchPatternBuilderTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void GetConstraint_Build_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<ExecutionXml>(); 
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            itemXmlStubFactory.Setup(i => i.GetParameters()).Returns(new List<QueryParameterXml>());
            itemXmlStubFactory.Setup(i => i.GetVariables()).Returns(new List<QueryTemplateVariableXml>());
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new MatchPatternXml();

            var builder = new ExecutionMatchPatternBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<MatchPatternConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_Build_CorrectIDbCommand()
        {
            var sutXmlStubFactory = new Mock<ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryableXml>();
            itemXmlStubFactory.Setup(i => i.GetQuery()).Returns("query");
            itemXmlStubFactory.Setup(i => i.GetParameters()).Returns(new List<QueryParameterXml>());
            itemXmlStubFactory.Setup(i => i.GetVariables()).Returns(new List<QueryTemplateVariableXml>());
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new MatchPatternXml();

            var builder = new ExecutionMatchPatternBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<IQuery>());
        }

    }
}
