﻿using System.IO;
#region Using directives

using NBi.Xml;
using System.Reflection;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Xml.Settings;
using System;
#endregion

namespace NBi.Testing.Unit.Xml.Settings
{
    [TestFixture]
    public class SettingsXmlTest
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

        protected TestSuiteXml DeserializeSample(string filename)
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(string.Format("NBi.Testing.Unit.Xml.Resources.{0}.xml", filename)))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            manager.ApplyDefaultSettings();
            return manager.TestSuite;
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultLoaded()
        {           
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefault");

            Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(2)); //(One empty and one initialized)
            var sutDefault = ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest);
            Assert.That(sutDefault.ConnectionStringSpecified, Is.True);
            Assert.That(sutDefault.ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
            Assert.That(sutDefault.Parameters, Is.Not.Null);
        }

        [Test]
        public void Deserialize_SettingsWithoutTagParallelizeQueries_ParallelizeQueriesIsTrue()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefault");

            Assert.That(ts.Settings.ParallelizeQueries, Is.True);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefault");

            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.ConnectionString, Is.Null.Or.Empty);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultReplicatedForTestRoleAdded()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefault");

            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Not.StringContaining("\r"));
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Not.StringContaining("\n"));
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.StringContaining("Roles=\"admin\""));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefaultForAssert_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefaultAssert");

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).GetCommand().Connection.ConnectionString, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_NoDefaultLoadedButAreAutomaticallyCreated()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithoutDefault");

            Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(2));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithoutDefault");

            Assert.That(((QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).Item).GetConnectionString(), Is.Null.Or.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_DefaultEqualToEmptyDefaultAndNotNull()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithoutDefault");

            Assert.That(ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest), Is.Not.Null);
            Assert.That(ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionStringSpecified, Is.False);
            Assert.That(ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).Parameters, Has.Count.EqualTo(0));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithReference_ReferenceLoaded()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");

            Assert.That(ts.Settings.References.Count, Is.EqualTo(2));
            Assert.That(ts.Settings.GetReference("first-ref"), Is.Not.Null);
            Assert.That(ts.Settings.GetReference("first-ref").ConnectionString.Inline, Is.EqualTo("My First Connection String"));
            Assert.That(ts.Settings.GetReference("second-ref"), Is.Not.Null);
            Assert.That(ts.Settings.GetReference("second-ref").ConnectionString.Inline, Is.EqualTo("My Second Connection String"));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithReferenceNotExisting_ThrowArgumentException()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");

            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { ts.Settings.GetReference("not-existing"); });
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithReference_ReferenceAppliedToTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");

            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Not.Null.And.Not.Empty);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.EqualTo("My Second Connection String"));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithReferenceButMissingconnStrRef_NullAppliedToTest()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");
            Assert.That(((QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).Item).GetConnectionString(), Is.Null.Or.Empty);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Null.Or.Empty);
        }

        [Test]
        public void DeserializeStructurePerspective_SettingsWithReference_ReferenceAppliedToTest()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");

            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Not.Null.And.Not.Empty);
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.EqualTo("My First Connection String"));
        }

        [Test]
        public void DeserializeStructurePerspective_SettingsWithoutParallelizeQueries_False()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithoutParallelQueries");

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ParallelizeQueries, Is.True);
        }

        [Test]
        public void DeserializeStructurePerspective_SettingsWithParallelizeQueriesSetFalse_False()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithParallelQueriesSetFalse");

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ParallelizeQueries, Is.False);
        }

        [Test]
        public void DeserializeStructurePerspective_SettingsWithParallelizeQueriesSetTrue_True()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithParallelQueriesSetTrue");

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ParallelizeQueries, Is.True);
        }

        [Test]
        public void DeserializeStructurePerspective_SettingsWithParameters_True()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithParameters");

            var parameters =((QueryXml)ts.Tests[testNr].Systems[0].BaseItem).GetParameters();
            Assert.That(parameters.Count, Is.EqualTo(3));
        }

        [Test]
        public void DeserializeStructurePerspective_SettingsWithVariables_True()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithVariables");

            var parameters = ((QueryXml)ts.Tests[testNr].Systems[0].BaseItem).GetVariables();
            Assert.That(parameters.Count, Is.EqualTo(3));
        }

        [Test]
        public void DeserializeStructurePerspective_SettingsWithDefaultEverywhere_True()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefaultEverywhere");

            //The connections string is overriden where needed
            Assert.That(((QueryXml)ts.Tests[testNr].Systems[0].BaseItem).GetConnectionString(), Is.EqualTo("My Connection String"));
            Assert.That(((QueryXml)ts.Tests[testNr].Constraints[0].BaseItem).GetConnectionString(), Is.EqualTo("My Connection String from Everywhere"));

            //The param is copied everywhere
            Assert.That(((QueryXml)ts.Tests[testNr].Systems[0].BaseItem).GetParameters().Find(p => p.Name == "paramEverywhere").StringValue, Is.EqualTo("120"));
            Assert.That(((QueryXml)ts.Tests[testNr].Constraints[0].BaseItem).GetParameters().Find(p => p.Name == "paramEverywhere").StringValue, Is.EqualTo("120"));

            //The param is not overriden
            Assert.That(((QueryXml)ts.Tests[testNr].Systems[0].BaseItem).GetParameters().Find(p => p.Name == "paramToOverride").StringValue, Is.EqualTo("Alpha"));
            //The param is overriden
            Assert.That(((QueryXml)ts.Tests[testNr].Constraints[0].BaseItem).GetParameters().Find(p => p.Name == "paramToOverride").StringValue, Is.EqualTo("80"));

            //The param is not overriden
            Assert.That(((QueryXml)ts.Tests[testNr].Systems[0].BaseItem).GetParameters().Find(p => p.Name == "paramToOverrideTwice").StringValue, Is.EqualTo("3"));
            //The param is overriden
            Assert.That(((QueryXml)ts.Tests[testNr].Constraints[0].BaseItem).GetParameters().Find(p => p.Name == "paramToOverrideTwice").StringValue, Is.EqualTo("1"));

        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToTabCardinalLineFeed_True()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.InternalFieldSeparator, Is.EqualTo("Tab"));
            Assert.That(profile.FieldSeparator, Is.EqualTo('\t'));
            Assert.That(profile.InternalRecordSeparator, Is.EqualTo("#Lf"));
            Assert.That(profile.RecordSeparator, Is.EqualTo("#\n"));
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToDefaultFirstRowHeader_False()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.FirstRowHeader, Is.False);
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToDefaultMissingemptyValues_Default()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.EmptyCell, Is.EqualTo("(empty)"));
            Assert.That(profile.MissingCell, Is.EqualTo("(null)"));
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToFirstRowHeader_True()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite2");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.FirstRowHeader, Is.True);
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToEmptyCell_True()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite2");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.EmptyCell, Is.EqualTo("empty value"));
            Assert.That(profile.MissingCell, Is.EqualTo("missing value"));
        }

        [Test]
        public void Serialize_CardinalForFieldSeparator_FieldSeparatorSpecified()
        {
            var profile = new CsvProfileXml
            {
                FieldSeparator = '#',
                RecordSeparator = "\r"
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.StringContaining("field-separator"));
            Assert.That(xml, Is.StringContaining("#"));

            Assert.That(xml, Is.StringContaining("record-separator"));
            Assert.That(xml, Is.StringContaining("Cr"));

            Assert.That(xml, Is.Not.StringContaining("<FieldSeparator>"));
            Assert.That(xml, Is.Not.StringContaining("<TextQualifier>"));
            Assert.That(xml, Is.Not.StringContaining("<RecordSeparator>"));
            Assert.That(xml, Is.Not.StringContaining("<FirstRowHeader>"));

            Assert.That(xml, Is.Not.StringContaining("first-row-header"));
        }

        [Test]
        public void Serialize_CrLfForRecordSeparator_RecordSeparatorNotSpecified()
        {
            var profile = new CsvProfileXml
            {
                FieldSeparator = '#',
                RecordSeparator = "\r\n"
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.Not.StringContaining("record-separator"));
        }

        [Test]
        public void Serialize_TrueForFirstRowHeader_FirstRowHeaderSpecified()
        {
            var profile = new CsvProfileXml
            {
                FirstRowHeader = true
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.StringContaining("first-row-header"));
        }

        [Test]
        public void Serialize_FalseForFirstRowHeader_FirstRowHeaderSpecified()
        {
            var profile = new CsvProfileXml { FirstRowHeader = false };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.Not.StringContaining("first-row-header"));
        }

        [Test]
        public void Serialize_EmptyForEmpytCell_EmptyCellNotSpecified()
        {
            var profile = new CsvProfileXml { EmptyCell = "(empty)" };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.Not.StringContaining("empty-cell"));
        }

        [Test]
        public void Serialize_StringForEmpytCell_EmptyCellSpecified()
        {
            var profile = new CsvProfileXml { EmptyCell = "my value" };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.StringContaining("empty-cell"));
            Assert.That(xml, Is.StringContaining("my value"));
        }

        [Test]
        public void Serialize_SemiColumnForFieldSeparator_FieldSeparatorNotSpecified()
        {
            var profile = new CsvProfileXml
            {
                FieldSeparator = ';',
                RecordSeparator = "#"
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.Not.StringContaining("field-separator"));
        }

        [Test]
        public void Serialize_SemiColumnAndCrLf_CsvProfileNotSpecified()
        {
            var settings = new SettingsXml();
            settings.CsvProfile.FieldSeparator = ';';
            settings.CsvProfile.RecordSeparator = "\r\n";

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<SettingsXml>(settings);

            Assert.That(xml, Is.Not.StringContaining("field-separator"));
            Assert.That(xml, Is.Not.StringContaining("record-separator"));
            Assert.That(xml, Is.Not.StringContaining("csv-profile"));
        }
    }
}
