using System.Xml.Xsl;
using System;
using System.Configuration;
using System.Xml;
using System.Data;
//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0
namespace ADONETDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var filepath = ConfigurationManager.AppSettings["filepath"];

            //DataSetReadXml(filepath);
            //DataSetReadInnerXmlSchema(filepath);
            //DataSetReadOuterXmlSchema(filepath);

            TransformXmlToHtml(filepath);
        }

        #region XmlDataDocument
        private static void TransformXmlToHtml(string filepath){
            DataSet ds = new DataSet();
            ds.ReadXml(filepath + "cust-conv.xml");
            XmlDataDocument doc = new XmlDataDocument(ds);
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(filepath + "cust-conv.xsl");
            var settings = new XmlWriterSettings{
                Indent=true,
                IndentChars="    "
            };
            var writer = XmlWriter.Create(filepath + "cust-conv.html", settings);
            transform.Transform(doc, writer);
            writer.Close();

            Console.WriteLine("Transformed successfully!");

            foreach (DataRow row in doc.DataSet.Tables[0].Rows)
            {
                XmlElement element= doc.GetElementFromRow(row);
                Console.WriteLine(element.OuterXml);
            }
        }
        #endregion

        #region DataSet
        private static void DataSetReadOuterXmlSchema(string filepath){
            DataSet ds = new DataSet();
            ds.InferXmlSchema(filepath+"schema3.xml", null);
            Console.WriteLine(ds.GetXmlSchema());
        }

        private static void DataSetReadInnerXmlSchema(string filepath){
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(filepath+"schema2.xml");
            Console.WriteLine(ds.GetXmlSchema());
        }

        private static void DataSetReadXml(string filepath){
            DataSet ds = new DataSet();
            ds.ReadXml(filepath + "schema.xml");
            Console.WriteLine("customers");
            var tableCount = ds.Tables.Count;
            Console.WriteLine("#TableCount: " + tableCount);
            for (int i = 0; i < tableCount; i++)
            {
                var columnCount = ds.Tables[i].Columns.Count;
                Console.WriteLine("#ColumnCount: " + columnCount);
                foreach (DataRow cust in ds.Tables[i].Rows)
                {
                    var id = cust["id"].ToString();
                    Console.WriteLine($"--customer: id={id}");
                    for (int j = 0; j < columnCount; j++)
                    {
                        var columnName = ds.Tables[i].Columns[j].ColumnName.ToString();
                        Console.WriteLine($"----{columnName}: {cust[columnName].ToString()}");
                    }

                    var relationCount = ds.Relations.Count;
                    Console.WriteLine("#RelationCount: " + relationCount);
                    for (int k = 0; k < relationCount; k++)
                    {
                        DataRow[] infos = cust.GetChildRows(ds.Relations[k]);
                        foreach (DataRow info in infos)
                        {
                            Console.WriteLine($"------{info[0].ToString()}");
                        }                        
                    }
                }                
            }
        }
        #endregion
    }
}
