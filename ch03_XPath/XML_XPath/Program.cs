using System.Xml;
using System.Xml.XPath;
using System;
using System.Configuration;
//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0

namespace XML_XPath
{
    class Program
    {
        static void Main(string[] args)
        {
            var isXmlDoc = true;
            var xmlfilepath = ConfigurationManager.AppSettings["filepath"];

            //XPathNavigator _nav = CreateNavigator(isXmlDoc, xmlfilepath);

            //LoopXml(xmlfilepath);

            //SelectNodes(xmlfilepath);
            //SelectSingleNode(xmlfilepath);

            //UseXmlReader(xmlfilepath);
            //UseXmlWriter(xmlfilepath);

            UpdateNavigator();
        }

        private static void UpdateNavigator(){
            XmlDocument doc = new XmlDocument();
            doc.Load("CustomersNew.xml");
            var nav = doc.CreateNavigator();
            if(nav.CanEdit){
                nav.MoveToRoot();
                nav.MoveToFirstChild();
                var id=nav.GetAttribute("id", "");
                if(id!="3"){
                    Console.WriteLine("Not found attribute");
                    return;
                }
                if(nav.HasChildren){
                    nav.MoveToFirstChild();
                    do{
                        switch (nav.Name)
                        {
                            case "firstname":
                            case "lastname":
                                nav.SetValue(nav.Value+" new");
                                break;
                            case "homephone":
                                nav.SetValue("(445) 269-3456");
                                break;
                            default:
                                break;
                        }
                    }while(nav.MoveToNext());
                    nav.MoveToParent();
                }
                doc.Save(Console.Out);
                doc.Save("CustomersNew2.xml");
            }
        }

        private static void UseXmlWriter(string xmlfilepath){
            XPathDocument doc = new XPathDocument(xmlfilepath);
            XPathNavigator nav = doc.CreateNavigator();
            nav.MoveToRoot();
            nav.MoveToFirstChild();
            if(nav.HasChildren){
                nav.MoveToFirstChild();
                do{
                    var id = nav.GetAttribute("id","");
                    if(id=="3"){
                        XmlWriterSettings settings = new XmlWriterSettings{
                            Indent=true,
                            IndentChars="    ",
                            OmitXmlDeclaration=true
                        };
                        XmlWriter writer = XmlWriter.Create("CustomersNew.xml",settings);
                        try{
                            nav.WriteSubtree(writer);
                        }catch(Exception ex){
                            Console.WriteLine(ex.Message);
                        }finally{
                            writer.Close();
                        }
                    }
                } while(nav.MoveToNext());
            }
        }

        private static void UseXmlReader(string xmlfilepath){
            XPathDocument doc = new XPathDocument(xmlfilepath);
            XPathNavigator nav = doc.CreateNavigator();
            nav.MoveToRoot();
            nav.MoveToFirstChild();
            if(nav.HasChildren){
                nav.MoveToFirstChild();
                do{
                    var id = nav.GetAttribute("id","");
                    if(id=="3"){
                        Console.Write("<{0} id=\"{1}\">", nav.Name, id);
                        XmlReader reader = nav.ReadSubtree();
                        reader.MoveToContent();
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    Console.Write("<{0}>", reader.Name);
                                    break;
                                case XmlNodeType.Text:
                                    Console.Write(reader.Value);
                                    break;
                                case XmlNodeType.CDATA:
                                    Console.Write("<![CDATA[{0}]]>", reader.Value);
                                    break;
                                case XmlNodeType.ProcessingInstruction:
                                    Console.Write("<?{0} {1}?>", reader.Name, reader.Value);
                                    break;
                                case XmlNodeType.Comment:
                                    Console.Write("<!--{0}-->", reader.Value);
                                    break;
                                case XmlNodeType.XmlDeclaration:
                                    Console.Write("<?xml version='1.0'?>");
                                    break;
                                case XmlNodeType.Document:
                                    break;
                                case XmlNodeType.DocumentType:
                                    Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                                    break;
                                case XmlNodeType.EntityReference:
                                    Console.Write(reader.Name);
                                    break;
                                case XmlNodeType.EndElement:
                                    Console.Write("</{0}>", reader.Name);
                                    break;
                                default:
                                    Console.WriteLine($"Other node type {reader.NodeType} with value {reader.Value}");
                                    break;
                            }
                        }
                    }
                } while(nav.MoveToNext());
            }
        }

        private static void SelectSingleNode(string xmlfilepath){
            XPathDocument doc = new XPathDocument(xmlfilepath);
            XPathNavigator _nav = doc.CreateNavigator();

            XPathExpression expression = _nav.Compile("customers/customer[@id=1]");
            XPathNavigator nav = _nav.SelectSingleNode(expression);

            nav.MoveToFirstChild();
            do{
                switch (nav.Name)
                {
                    case "firstname":
                        Console.WriteLine($"FirstName: {nav.Value}");
                        break;
                    case "lastname":
                        Console.WriteLine($"LastName: {nav.Value}");
                        break;
                    case "homephone":
                        Console.WriteLine($"HomePhone: {nav.Value}");
                        break;
                    case "notes":
                        Console.WriteLine($"Notes: {nav.Value}");
                        break;
                    default:
                        Console.WriteLine("N/A");
                        break;
                }
            } while(nav.MoveToNext());
        }

        private static void SelectNodes(string xmlfilepath){
            XPathDocument doc = new XPathDocument(xmlfilepath);
            XPathNavigator nav = doc.CreateNavigator();

            try{
                XPathNodeIterator iterator = nav.Select("customers/customer");
                Console.WriteLine($"Total {iterator.Count} element(s) selected.");
                if(iterator.Count>0){
                    while (iterator.MoveNext())
                    {
                        Console.WriteLine(iterator.Current.OuterXml);
                        //Console.WriteLine(iterator.Current.InnerXml);
                    }
                }else{
                    Console.WriteLine("No result.");
                }

            }catch(Exception ex){
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private static void LoopXml(string xmlfilepath){
            XPathDocument doc = new XPathDocument(xmlfilepath);
            XPathNavigator nav = doc.CreateNavigator();
            nav.MoveToRoot(); //<?xml ...>
            nav.MoveToFirstChild(); //<customers>
            if(nav.HasChildren){
                nav.MoveToFirstChild(); //<customer>
                do{
                    var id = nav.GetAttribute("id","");
                    Console.WriteLine($"--Customer.Id: {id}");

                    nav.MoveToFirstChild();
                    do{
                        Console.WriteLine($"----{nav.Name}: {nav.Value}");
                    }while(nav.MoveToNext());

                    nav.MoveToParent();
                }while(nav.MoveToNext());
            }
        }

        private static XPathNavigator CreateNavigator(bool isXmlDoc, string xmlfilepath){
            XmlDocument document;
            XPathDocument xPathDocument;
            XPathNavigator _nav = null;
            if(isXmlDoc){
                document=new XmlDocument();
                document.Load(xmlfilepath);
                _nav = document.CreateNavigator();
            }else{
                xPathDocument=new XPathDocument(xmlfilepath);
                _nav = xPathDocument.CreateNavigator();
            }

            return _nav;
        }

    }
}
