using System.Xml.Schema;
using System.Xml.Linq;
using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0

namespace LinqDemo
{
    class Program
    {
        delegate int delgt(int i);
        static void Main(string[] args)
        {
            var filepath = ConfigurationManager.AppSettings["filepath"];

            //XElement_Load(filepath);
            //XDocument_Load(filepath);
            //XDocument_Foreach(filepath);
            //XElement_Foreach(filepath);
            //XElement_Attribute(filepath);

            //ParseToXml();
            //QueryWithNamespace();

            //LinqToDelegate();
            //LinqToArray();

            //XDocument_Validate(filepath);

            TransformXmlToHtml(filepath);

            Console.ReadLine();
        }

        private static void TransformXmlToHtml(string filepath){
            XElement root = XElement.Load(filepath + "customers2.xml", LoadOptions.PreserveWhitespace);
            XElement html = new XElement("html",
                                new XElement("body",
                                    new XElement("table",
                                        new XAttribute("border", 1),
                                        new XElement("th", "Cust Id"),
                                        new XElement("th", "First Name"),
                                        new XElement("th", "Last Name"),
                                        new XElement("th", "Balance"),
                                        new XElement("th", "Country"),
                                        new XElement("th", "Homephone"),
                                        new XElement("th", "Notes"),
                                        from item in root.Descendants("customer")
                                        select new XElement("tr",
                                            new XElement("td", item.Attribute("id").Value),
                                            new XElement("td", item.Element("firstname").Value),
                                            new XElement("td", item.Element("lastname").Value),
                                            new XElement("td", item.Element("balance").Value),
                                            new XElement("td", item.Element("country").Attribute("code").Value + "-" + item.Element("country").Value),
                                            new XElement("td", item.Element("homephone").Value),
                                            new XElement("td", item.Element("notes").Value))
                                        )));
            html.Save(filepath+"customers2.html", SaveOptions.None);
            Console.WriteLine("Transform completed!");
        }

        private static void XDocument_Validate(string filepath){
            XDocument doc = XDocument.Load(filepath + "schema2.xml", LoadOptions.PreserveWhitespace);
            XmlSchemaSet schemaSet= new XmlSchemaSet();
            schemaSet.Add(null, filepath + "schema.xsd");
            ValidationEventHandler handler = new ValidationEventHandler(MyValidationResult);
            doc.Validate(schemaSet, handler);
            Console.WriteLine("Validation completed!");
        }
        private static void MyValidationResult(object sender, ValidationEventArgs e){
            Console.WriteLine(e.Message);
        }

        private static void QueryWithNamespace(){
            string xml = "<cq:customers xmlns:cq=\"http://www.customer_query.com\">" +
                         "  <cq:customer id=\"1\">" +
                         "    <cq:firstname>John</cq:firstname>" +
                         "    <cq:lastname>Cranston</cq:lastname>" +
                         "    <cq:balance>100.00</cq:balance>" +
                         "    <cq:country code=\"86\">china</cq:country>" +
                         "    <cq:homephone>(445) 269-9857</cq:homephone>" +
                         "    <cq:notes/>" +
                         "  </cq:customer>" +
                         "</cq:customers>";
            XElement element = XElement.Parse(xml);
            XNamespace ns = "http://www.customer_query.com";
            var custs = from cust in element.Elements(ns+"customer")
                        where (from country in cust.Elements(ns + "country")
                                where Convert.ToInt32(country.Attribute("code").Value)==86
                                select country).Any()
                        //where Convert.ToInt32(cust.Attribute("id").Value)==1
                        select cust;
            Array.ForEach(custs.ToArray(), new Action<XElement>((customer)=>{Console.WriteLine(customer.Element(ns+"firstname").Value);}));

            foreach (var cust in element.Elements(ns+"customer"))
            {
                cust.AddAnnotation("This is an xml from code."); 
                Console.WriteLine(cust.Annotation(typeof(Object)));               
            }
            element.Save(Console.Out);


            custs = from balances in element.Elements(ns+"customer").Elements(ns+"balance")
                    let country = balances.ElementsAfterSelf().FirstOrDefault()
                    where (int)(country.Attribute("code"))==86
                    orderby (int)(country.Attribute("code"))
                    select balances;
            Array.ForEach(custs.ToArray(), new Action<XElement>((customer)=>{Console.WriteLine(customer.Value);}));
        }

        private static void ParseToXml(){
            string xml = "<customers>" +
                         "  <customer id=\"1\">" +
                         "    <firstname>John</firstname>" +
                         "    <lastname>Cranston</lastname>" +
                         "    <balance/>" +
                         "    <country>china</country>" +
                         "    <homephone>(445) 269-9857</homephone>" +
                         "    <notes/>" +
                         "  </customer>" +
                         "</customers>";
            XElement element = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

            XElement _cust = new XElement("customer",
                                new XElement("firstname", "Annie"),
                                new XElement("lastname", "Rose"),
                                new XElement("balance", null),
                                new XElement("country", "china"),
                                new XElement("homephone", string.Empty),
                                new XElement("notes", new XCData("This is a XCData from code!")));
            _cust.SetAttributeValue("id", "2");
            _cust.SetElementValue("lastname", "Ross");
            //_cust.SetElementValue("balance", "100.00");
            //_cust.SetElementValue("homephone", "12345678900");
            element.Add(_cust);
            element.Save(Console.Out);

            Console.WriteLine();
            //_cust.Remove();

            var custs = from cust in element.Elements("customer")
                        let balance = cust.Element("balance")
                        where balance.IsEmpty||balance.Value.Length==0
                        select cust;
            Array.ForEach(custs.ToArray(), new Action<XElement>((customer)=>{Console.WriteLine(customer.Element("firstname").Value);}));
        }

        private static void XElement_Attribute(string filepath){
            XElement doc = XElement.Load(filepath + "customers.xml", LoadOptions.PreserveWhitespace);
            var custs = from cust in doc.Elements("customer")
                        let countryCode = cust.Element("country").Attribute("code")
                        where countryCode==null
                        select cust;
            
            int i=0;
            foreach (var cust in custs)
            {
                i++;
                if(i==1){
                    cust.Element("country").Add(new XAttribute("code", "+86"));
                    continue;
                } else if(i==2){
                    //cust.Element("country").Add(new XAttribute("code", "+68"));
                    cust.Element("country").SetAttributeValue("code", "+68");
                    continue;
                }
                cust.Element("country").Add(new XAttribute("code", "86"));
            }
            doc.Save(filepath + "customers2.xml", SaveOptions.None);

            doc = XElement.Load(filepath + "customers2.xml");
            custs = from cust in doc.Elements("customer")
                        let countryCode = cust.Element("country").Attribute("code")
                        where countryCode.Value=="+86"
                        select cust;
            Array.ForEach(custs.ToArray(), new Action<XElement>((customer)=>{Console.WriteLine(customer.Element("firstname").Value);}));

            foreach (var cust in custs)
            {
                cust.Element("country").Attribute("code").Remove();
            }
            doc.Save(filepath + "customers3.xml");
        }

        private static void XElement_Foreach(string filepath){
            XElement doc = XElement.Load(filepath + "customers.xml");
            foreach (XElement customer in doc.Elements())
            {
                Console.WriteLine("#####################");
                Console.WriteLine("CustomerId: ".PadLeft(15) + customer.Attribute("id").Value);
                if(!customer.HasElements){
                    continue;
                }
                foreach (XElement child in customer.Descendants())
                {
                    Console.WriteLine((child.Name.ToString()+"： ").PadLeft(15) + child.Value);
                }
            }
            Console.WriteLine("#####################");
            var custs = from cust in doc.Elements("customer")
                        select cust;
            Array.ForEach(custs.ToArray(), new Action<XElement>((customer)=>{Console.WriteLine(customer.Element("firstname").Value);}));
        }
        private static void XDocument_Foreach(string filepath){
            XDocument doc = XDocument.Load(filepath + "customers.xml");
            var custs = from cust in doc.Elements("customers").Elements("customer")
                        select cust;
            Array.ForEach(custs.ToArray(), new Action<XElement>((customer)=>{Console.WriteLine(customer.Element("firstname").Value);}));
        }
        private static void XDocument_Load(string filepath){
            XDocument doc = XDocument.Load(filepath + "customers.xml");

            Console.WriteLine("####### With Root ########");
            IEnumerable<XElement> customers1 = from cust in doc.Elements()
                                                select cust;
            foreach (XElement cust in customers1)
            {
                Console.WriteLine(cust);
            }
            Console.WriteLine("####### Without Root ########");
            IEnumerable<XElement> customers2 = from cust in doc.Root.Elements()
                                                select cust;
            foreach (XElement cust in customers2)
            {
                Console.WriteLine(cust);
            }
        }

        private static void XElement_Load(string filepath){
            XElement doc = XElement.Load(filepath + "customers.xml");
            IEnumerable<XElement> customers = from cust in doc.Elements()
                                                select cust;
            foreach (XElement cust in customers)
            {
                Console.WriteLine(cust);
            }
        }

        private static void LinqToDelegate(){
            delgt myDelgt = x => x*x;
            Console.WriteLine(myDelgt(5));
        }

        private static void LinqToArray(){
            string[] array = {"Hello", "你好", "good day today!", "It's sunshine~", "阳光总在风雨后"};
            var s = from n
                    in array
                    where n.Length > 3
                    select n;
            
            foreach (var n in s)
            {
                Console.WriteLine(n.ToString());
            }
        }
    }
}
