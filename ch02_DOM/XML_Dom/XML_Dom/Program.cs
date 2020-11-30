using System.IO;
using System;
using System.Xml;
using System.Configuration;

namespace XML_Dom
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlfilepath = ConfigurationManager.AppSettings["filepath"];

            XmlDocument doc;
            doc=Load(xmlfilepath);
            //doc=LoadXML(xmlfilepath);

            //LoopXMLNodes(doc);

            //QueryXml(doc);

            //Save(doc);

            doc=AppendNewElement(doc);
        }

        private static XmlDocument Load(string xmlfilepath){
            if(string.IsNullOrWhiteSpace(xmlfilepath)){
                return null;
            }

            XmlDocument doc = new XmlDocument();
            using(var stream = new FileStream(xmlfilepath, FileMode.Open)){
                doc.Load(stream);
            }
            Console.WriteLine("==========XML opened successfully!==========");
            return doc;
        }
        private static XmlDocument LoadXML(string xmlfilepath){
            if(string.IsNullOrWhiteSpace(xmlfilepath)){
                return null;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlfilepath);
            Console.WriteLine("XML opened successfully!");
            return doc;
        }

        private static void LoopXMLNodes(XmlDocument doc){
            if(doc==null){
                return;
            }

            var root = doc.DocumentElement;
            Console.WriteLine($"--{root.Name}");

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                Console.WriteLine($"----CustomerId:{node.Attributes["id"].Value}");
                if(node.HasChildNodes){
                    foreach (XmlNode cnode in node.ChildNodes)
                    {
                        Console.WriteLine($"------{cnode.Name}:{cnode.InnerText}");
                    }
                }
            }
            
            Console.WriteLine("==========XML looped successfully!==========");
        }

        private static void QueryXml(XmlDocument doc){
            if(doc==null){
                return;
            }
            Console.WriteLine("==========GetElementsByTagName==========");
            var list1 = doc.GetElementsByTagName("firstname");
            foreach (XmlNode node in list1)
            {
                Console.WriteLine(node.InnerText);
            }

            Console.WriteLine("==========GetElementById==========");
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var custId = node.Attributes["id"].Value;
                var custName = string.Format("{0} {1}", node.ChildNodes[0].InnerText, doc.GetElementById(custId).ChildNodes[1].InnerText);
                Console.WriteLine($"CustomerId:{custId}, CustomerName:{custName}");
            }

            Console.WriteLine("==========SelectNodes==========");
            var list2 = doc.SelectNodes("//customer[./firstname/text()='Annie']");
            foreach (XmlNode node in list2)
            {
                var custId = node.Attributes["id"].Value;
                var custName = string.Format("{0} {1}", doc.GetElementById(custId).ChildNodes[0].InnerText, doc.GetElementById(custId).ChildNodes[1].InnerText);
                Console.WriteLine($"CustomerId:{custId}, CustomerName:{custName}");
            }

            Console.WriteLine("==========SelectSingleNode==========");
            var snode = doc.SelectSingleNode("//customer[./firstname/text()='Annie']");
            if(snode!=null){
                var custId = snode.Attributes["id"].Value;
                var custName = string.Format("{0} {1}", snode.ChildNodes[0].InnerText, snode.ChildNodes[1].InnerText);
                Console.WriteLine($"CustomerId:{custId}, CustomerName:{custName}");
            }
        }

        private static void Save(XmlDocument doc){
            var xmlfilepath = ConfigurationManager.AppSettings["filepath"];
            var new_xmlfilepath = xmlfilepath.Replace("Customers.xml","Customers_New.xml");
            doc.Save(new_xmlfilepath);
            Console.WriteLine("==========Save to filepath==========");

            doc.Save(Console.Out);
            Console.WriteLine("==========Save to Console==========");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent=true;
            settings.IndentChars="    ";
            var new_xmlfilepath2 = xmlfilepath.Replace("Customers.xml","Customers_New2.xml");
            XmlWriter writer = XmlWriter.Create(new_xmlfilepath2, settings);
            doc.Save(writer);
            Console.WriteLine("==========Save to filepath with writer==========");
        }

        private static XmlDocument AppendNewElement(XmlDocument doc){
            if(doc==null){
                return null;
            }

            XmlNode root = doc.DocumentElement;
            //var newCustomer = doc.CreateDocumentFragment();
            //newCustomer.InnerXml="";
            XmlElement newCustomer = doc.CreateElement("customer");
            newCustomer.SetAttribute("id", "5");
            XmlElement fnode = doc.CreateElement("firstname");
            fnode.InnerText="Young";
            newCustomer.AppendChild(fnode);
            XmlElement lnode = doc.CreateElement("lastname");
            lnode.InnerText="Bob";
            newCustomer.AppendChild(lnode);
            XmlElement hnode = doc.CreateElement("homephone");
            hnode.InnerText="(445) 269-5678";
            newCustomer.AppendChild(hnode);
            XmlElement nnode = doc.CreateElement("notes");
            nnode.InnerText="";
            //newCustomer.AppendChild(nnode);

            XmlCDataSection notes = doc.CreateCDataSection("Bob joined when testing.");
            nnode.ReplaceChild(notes, nnode.ChildNodes[0]);

            root.AppendChild(newCustomer);

            XmlNode lastCust = doc.DocumentElement.LastChild;
            lastCust.InsertAfter(nnode, lastCust.LastChild);
            Console.WriteLine("==========Display the new xml==========");
            doc.Save(Console.Out);

            return doc;
        }
    }
}
