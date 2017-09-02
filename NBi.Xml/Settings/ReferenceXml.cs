﻿using System.Xml.Serialization;
using NBi.Xml.Items.Format;
using NBi.Xml.Items;

namespace NBi.Xml.Settings
{
    public class ReferenceXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("connectionString")]
        public string ConnectionString { get; set; }

        [XmlElement("regex")]
        public string Regex { get; set; }

        [XmlElement("numeric-format")]
        public NumericFormatXml NumericFormat { get; set; }

        [XmlElement("currency-format")]
        public CurrencyFormatXml CurrencyFormat { get; set; }

        [XmlElement("report")]
        public ReportBaseXml Report { get; set; }

        [XmlElement("etl")]
        public EtlBaseXml Etl { get; set; }

    }
}
