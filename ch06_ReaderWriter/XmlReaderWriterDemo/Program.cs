using System.Text;
using System.Xml;
using System;
using System.Configuration;
using System.IO;

//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0
namespace XmlReaderWriterDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var filepath = ConfigurationManager.AppSettings["filepath"];

            //Read(filepath, ""schema2.xml"");

            //NameTableDemo(filepath);

            Write(filepath);
        }

        private static void Write(string filepath){
            var settings = new XmlWriterSettings{
                Indent=true,
                IndentChars="    ",
                Encoding = Encoding.UTF8
            };
            using(var writer = XmlWriter.Create(filepath + "schema3.xml", settings)){
                writer.WriteStartDocument();

                writer.WriteStartElement("customers");

                writer.WriteStartElement("customer");
                writer.WriteAttributeString("id", null, "6");

                writer.WriteElementString("firstname", "Young");
                writer.WriteElementString("lastname", "Bob2");
                writer.WriteElementString("balance", "200.06");
                writer.WriteElementString("country", "china");
                writer.WriteElementString("homephone", "12345678903");
                writer.WriteElementString("notes", "<![CDATA[This file can pass 6 validation.]]>");

                writer.WriteEndElement();
                
                writer.WriteStartElement("customer");
                writer.WriteAttributeString("id", null, "7");

                writer.WriteStartElement("firstname");
                writer.WriteString("Young");
                writer.WriteEndElement();
                writer.WriteStartElement("lastname");
                writer.WriteString("Bob3");
                writer.WriteEndElement();
                writer.WriteStartElement("balance");
                writer.WriteString("200.07");
                writer.WriteEndElement();
                writer.WriteStartElement("country");
                writer.WriteString("china");
                writer.WriteEndElement();
                writer.WriteStartElement("notes");
                writer.WriteCData("This file can pass 7 validation.");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.Flush();
            }

            Read(filepath, "schema3.xml");
        }

        private static void NameTableDemo(string filepath){
            NameTable table = new NameTable();
            XmlTextReader reader1 = new XmlTextReader(filepath + "schema.xml", table);
            XmlTextReader reader2 = new XmlTextReader(filepath + "schema2.xml", table);
            
        }

        private static void Read(string filepath, string xmlfilename){
            //XmlTextReader reader = new XmlTextReader(filepath + "schema2.xml");
            //reader.WhitespaceHandling = WhitespaceHandling.None;

            var  settings = new XmlReaderSettings{
                IgnoreComments=true,
                IgnoreWhitespace=true
            };
            var reader = XmlReader.Create(filepath + xmlfilename, settings);
            while (reader.Read())
            {
                if(reader.NodeType==XmlNodeType.Element){
                    if(reader.Name == "customers"){
                        Console.WriteLine("customers");
                        continue;
                    }
                    if(reader.Name == "customer"){
                        Console.Write("--customer:");
                        while (reader.MoveToNextAttribute())
                        {
                            Console.Write(" {0}='{1}'", reader.Name, reader.Value);
                        }
                        Console.WriteLine();
                        continue;
                    }
                    //Console.WriteLine($"----{reader.Name}: {reader.ReadElementString()}");

                    //var name = reader.Name;
                    //reader.Read();
                    //var value = reader.Value;
                    //Console.WriteLine($"----{name}: {value}");

                    Console.WriteLine($"----{reader.Name}: {reader.ReadString()}");
                }
            }
            reader.Close();
        }

        private static void LoadFromUrl(string filefullpath){
            XmlTextReader reader;
            try {
                reader = new XmlTextReader(filefullpath);
                reader.Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private static void LoadFromStream(string filefullpath){
            XmlTextReader reader;
            try {
                using (var fs = File.OpenRead(filefullpath))
                {
                    reader = new XmlTextReader(fs);
                    reader.Close();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private static void LoadFromXmlText(string xmlText){
            XmlTextReader reader;
            using (var ms = new MemoryStream())
            {
                byte[] data = ASCIIEncoding.ASCII.GetBytes(xmlText);
                ms.Write(data, 0, data.Length);
                reader = new XmlTextReader(ms);
                reader.Close();
            }
        }
    }
}
