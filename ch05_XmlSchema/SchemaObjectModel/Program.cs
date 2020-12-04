using System.Xml.XPath;
using System.Reflection.Metadata;
using System.Xml.Schema;
using System;
using System.Configuration;
using System.Xml;

//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0
namespace SchemaObjectModel
{
    class Program
    {
        private static bool IsValidationError = false;
        static void Main(string[] args)
        {
            var filepath = ConfigurationManager.AppSettings["filepath"];

            //GenerateSOMFile(filepath);

            //ValidateXmlViaXmlReader(filepath);

            //ValidateXmlViaXmlDocument(filepath);

            ValidateXmlViaXPathDocument(filepath);

            //ValidationStandaloneXmlViaXmlDoc(filepath);
        }

        private static void ValidationStandaloneXmlViaXmlDoc(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load(filepath + "schema2.xml");
            doc.Load(filepath + "schema.xml");
            doc.Schemas.Add(null, filepath + "Customers.xsd");
            ValidationEventHandler handler = new ValidationEventHandler(ValidationEventHandler);
            doc.Validate(handler);
            Console.WriteLine("Completed!");
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: " + e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning: " + e.Message);
                    break;
                default:
                    Console.WriteLine("Complete: " + e.Message);
                    break;
            }
        }

        private static void ValidateXmlViaXPathDocument(string filepath)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                ValidationType = ValidationType.Schema
            };
            settings.Schemas.Add("", filepath + "Customers.xsd");
            settings.ValidationEventHandler += new ValidationEventHandler(OnValidationError);
            //XmlReader reader = XmlReader.Create(filepath+"schema2.xml", settings);
            XmlReader reader = XmlReader.Create(filepath + "schema.xml", settings);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            // 2. Read fragment document via navigator
            {
                XPathExpression expression = nav.Compile("customers/customer[@id>3]");
                XPathNavigator _nav = nav.SelectSingleNode(expression);
                nav=_nav;
            }

            // 1. Read whole document
            XmlReader _reader = nav.ReadSubtree();
            _reader.MoveToContent();
            if(_reader.HasAttributes){
                Console.Write("<{0}", _reader.Name);
                while (_reader.MoveToNextAttribute())
                {
                    Console.Write(" {0}='{1}'", _reader.Name, _reader.Value);
                }
                Console.Write(">", _reader.Name);
            }else{
                Console.Write("<{0}>", _reader.Name);
            }

            while (_reader.Read())
            {
                switch (_reader.NodeType)
                {
                    case XmlNodeType.Element:
                        Console.Write("<{0}", _reader.Name);
                        while (_reader.MoveToNextAttribute())
                        {
                            Console.Write(" {0}='{1}'", _reader.Name, _reader.Value);
                        }
                        Console.Write(">", _reader.Name);
                        break;
                    case XmlNodeType.Text:
                        Console.Write(_reader.Value);
                        break;
                    case XmlNodeType.CDATA:
                        Console.Write("<![CDATA[{0}]]>", _reader.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        Console.Write("<?{0} {1}?>", _reader.Name, _reader.Value);
                        break;
                    case XmlNodeType.Comment:
                        Console.Write("<!--{0}-->", _reader.Value);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        Console.Write("<?xml version='1.0'?>");
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        Console.Write("<!DOCTYPE {0} [{1}]", _reader.Name, _reader.Value);
                        break;
                    case XmlNodeType.EntityReference:
                        Console.Write(_reader.Name);
                        break;
                    case XmlNodeType.EndElement:
                        Console.Write("</{0}>", _reader.Name);
                        break;
                    default:
                        Console.WriteLine($"Other node type {_reader.NodeType} with value {_reader.Value}");
                        break;
                }
            }
        }

        private static void ValidateXmlViaXmlDocument(string filepath)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                ValidationType = ValidationType.DTD
            };
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add("", filepath + "Customers.xsd");

            XmlReader reader = XmlReader.Create(filepath + "schema.xml", settings);
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            reader.Close();

            XmlElement cust = document.CreateElement("customer");
            XmlElement fname = document.CreateElement("firstname");
            XmlElement lname = document.CreateElement("lastname");
            XmlElement balance = document.CreateElement("balance");
            XmlElement country = document.CreateElement("country");
            XmlElement homephone = document.CreateElement("homephone");
            XmlElement notes = document.CreateElement("notes");
            XmlAttribute id = document.CreateAttribute("id");
            id.Value = "5";

            XmlText fnameText = document.CreateTextNode("Young");
            XmlText lnameText = document.CreateTextNode("Bob");
            XmlText balanceText = document.CreateTextNode("200.00");
            XmlText countryText = document.CreateTextNode("china");
            //XmlText homephoneText = document.CreateTextNode("12345678901010101010101010101234567890101010101010101010");
            XmlText homephoneText = document.CreateTextNode("12345678902");
            XmlCDataSection notesText = document.CreateCDataSection("I'm new here...testing now!!!");

            cust.Attributes.Append(id);
            cust.AppendChild(fname);
            cust.AppendChild(lname);
            cust.AppendChild(balance);
            cust.AppendChild(country);
            cust.AppendChild(homephone);
            cust.AppendChild(notes);

            fname.AppendChild(fnameText);
            lname.AppendChild(lnameText);
            balance.AppendChild(balanceText);
            country.AppendChild(countryText);
            homephone.AppendChild(homephoneText);
            notes.AppendChild(notesText);

            document.DocumentElement.AppendChild(cust);
            document.Validate(OnValidationError, cust);
            if (!IsValidationError)
            {
                document.Save(Console.Out);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("File saved successfully.");
            }
            else
            {
                Console.WriteLine("File saved failed.");
            }
        }

        private static void OnValidationError(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(e.Message);
            IsValidationError = true;
        }

        private static void ValidateXmlViaXmlReader(string filepath)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                ValidationType = ValidationType.DTD
            };
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add("", filepath + "Customers.xsd");

            XmlReader reader = XmlReader.Create(filepath + "schema.xml", settings);
            while (reader.Read())
            {
                Console.WriteLine("validating...");
            }
            reader.Close();
            Console.WriteLine("validation is completed!");
        }

        private static void GenerateSOMFile(string filepath)
        {
            XmlSchema schema = new XmlSchema();

            // nameType
            XmlSchemaSimpleType nameType = new XmlSchemaSimpleType();
            XmlSchemaSimpleTypeRestriction nameRstr = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            XmlSchemaMinLengthFacet nameFacet1 = new XmlSchemaMinLengthFacet { Value = "3" };
            XmlSchemaMaxLengthFacet nameFacet2 = new XmlSchemaMaxLengthFacet { Value = "255" };
            nameRstr.Facets.Add(nameFacet1);
            nameRstr.Facets.Add(nameFacet2);
            nameType.Content = nameRstr;

            // homephoneType
            XmlSchemaSimpleType phoneType = new XmlSchemaSimpleType();
            XmlSchemaSimpleTypeRestriction phoneRstr = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            XmlSchemaMaxLengthFacet phoneFacet1 = new XmlSchemaMaxLengthFacet { Value = "25" };
            phoneRstr.Facets.Add(phoneFacet1);
            phoneType.Content = phoneRstr;

            // notesType
            XmlSchemaSimpleType notesType = new XmlSchemaSimpleType();
            XmlSchemaSimpleTypeRestriction notesRstr = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            XmlSchemaMaxLengthFacet notesFacet1 = new XmlSchemaMaxLengthFacet { Value = "255" };
            notesRstr.Facets.Add(notesFacet1);
            notesType.Content = notesRstr;

            // balanceType
            XmlSchemaSimpleType balanceType = new XmlSchemaSimpleType();
            XmlSchemaSimpleTypeRestriction balanceRstr = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            XmlSchemaMinLengthFacet balanceFacet1 = new XmlSchemaMinLengthFacet { Value = "0" };
            balanceRstr.Facets.Add(balanceFacet1);
            balanceType.Content = balanceRstr;

            // countryType
            XmlSchemaSimpleType countryType = new XmlSchemaSimpleType();
            XmlSchemaSimpleTypeRestriction countryRstr = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
            };
            countryType.Content = countryRstr;

            // customerType
            XmlSchemaComplexType customerType = new XmlSchemaComplexType();
            XmlSchemaSequence sequence = new XmlSchemaSequence();

            XmlSchemaElement firstname = new XmlSchemaElement
            {
                Name = "firstname",
                SchemaType = nameType
            };
            sequence.Items.Add(firstname);

            XmlSchemaElement lastname = new XmlSchemaElement
            {
                Name = "lastname",
                SchemaType = nameType
            };
            sequence.Items.Add(lastname);

            XmlSchemaElement balance = new XmlSchemaElement
            {
                Name = "balance",
                SchemaType = balanceType
            };
            sequence.Items.Add(balance);

            XmlSchemaElement country = new XmlSchemaElement
            {
                Name = "country",
                SchemaType = countryType
            };
            sequence.Items.Add(country);

            XmlSchemaElement phone = new XmlSchemaElement
            {
                Name = "homephone",
                SchemaType = phoneType
            };
            sequence.Items.Add(phone);

            XmlSchemaElement notes = new XmlSchemaElement
            {
                Name = "notes",
                SchemaType = notesType
            };
            sequence.Items.Add(notes);

            customerType.Particle = sequence;

            // attribute: id
            XmlSchemaAttribute custId = new XmlSchemaAttribute();
            custId.Name = "id";
            custId.SchemaTypeName = new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema");
            custId.Use = XmlSchemaUse.Required;
            customerType.Attributes.Add(custId);

            // top complex type
            XmlSchemaComplexType x_customerType = new XmlSchemaComplexType();
            XmlSchemaSequence x_sequence = new XmlSchemaSequence();
            XmlSchemaElement customer = new XmlSchemaElement();
            customer.Name = "customer";
            customer.SchemaType = customerType;
            customer.MinOccurs = 0;
            customer.MaxOccursString = "unbounded";
            x_sequence.Items.Add(customer);
            x_customerType.Particle = x_sequence;

            // top element
            XmlSchemaElement customers = new XmlSchemaElement();
            customers.Name = "customers";
            customers.SchemaType = x_customerType;
            schema.Items.Add(customers);

            // compiled the schema
            try
            {
                XmlSchemaSet set = new XmlSchemaSet();
                set.Add(schema);
                set.Compile();
                Console.WriteLine("Schema compiled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Schema compiled failed: " + ex.Message);
            }

            // save the schema
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                OmitXmlDeclaration = true
            };
            XmlWriter writer = XmlWriter.Create(filepath + "Customers.xsd", settings);
            schema.Write(writer);
            writer.Close();
            Console.WriteLine("Schema file generated!");
        }

    }
}
