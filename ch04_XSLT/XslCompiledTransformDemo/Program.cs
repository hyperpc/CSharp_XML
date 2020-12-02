using System.IO;
using System.Xml.Linq;
using System.Xml.Xsl;
using System;
using System.Configuration;
using System.Xml;
//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0

namespace XslCompiledTransformDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var filepath = ConfigurationManager.AppSettings["filepath"];

            //TransformXmlToHtml(filepath);

            //XslArguments(filepath);
            
            NewBalance(filepath);
        }

        private static void NewBalance(string filepath){
            var xmlfilepath = filepath + ConfigurationManager.AppSettings["xmlfilename"];
            var xslfilepath = filepath + "cust-conv.xsl";
            var htmlfilepath = filepath + "Customers-conv.html";

            XsltSettings settings = new XsltSettings{ EnableScript=true};
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslfilepath, settings, null);

            XsltArgumentList args = new XsltArgumentList();
            var balance = new Balance();
            args.AddExtensionObject("urn:balance-conv", balance);

            var fs = new FileStream(htmlfilepath, FileMode.Create);
            xslt.Transform(xmlfilepath, args, fs);
            fs.Close();
        }

        private static void XslArguments(string filepath){
            var xmlfilepath = filepath + ConfigurationManager.AppSettings["xmlfilename"];
            var xslfilepath = filepath + "cust-argument.xsl";
            var htmlfilepath = filepath + "Customers-args.html";

            var stream = new FileStream(htmlfilepath, FileMode.Create);

            XslCompiledTransform xslt= new XslCompiledTransform();
            xslt.Load(xslfilepath);
            XsltArgumentList args = new XsltArgumentList();
            args.AddParam("firstname", "", "Annie");
            xslt.Transform(xmlfilepath, args, stream);
            stream.Close();
        }

        private static void TransformXmlToHtml(string filepath){
            var xmlfilepath = filepath + ConfigurationManager.AppSettings["xmlfilename"];
            var xslfilepath = filepath + ConfigurationManager.AppSettings["xslfilename"];
            var htmlfilepath = filepath + "Customers.html";

            var xslt = new XslCompiledTransform();
            xslt.Load(xslfilepath);
            xslt.Transform(xmlfilepath, htmlfilepath);
        }

    }
}
