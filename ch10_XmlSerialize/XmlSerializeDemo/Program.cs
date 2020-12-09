using System.Net;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;

//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0
namespace XmlSerializeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var filepath = ConfigurationManager.AppSettings["filepath"];

            //Serializing(filepath);
            //Deserializing(filepath);
            //MySerializing(filepath);

            SoapSerializing(filepath);
            SoapDeserializing(filepath);

            Soap2Serializing(filepath);
            Soap2Deserializing(filepath);

            Console.ReadLine();
        }

        private static void Soap2Serializing(string filepath){            
            var customer = new SoapCustomer2{
                CustomerId = "5",
                FirstName = "Young",
                LastName="Bob5",
                Balance = "100.05",
                Country = "china",
                Homephone="12345678905",
                Notes=""
            };
            using(var fs = new FileStream(filepath + "customer_soap2.xml", FileMode.Create)){
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fs, customer);
            }
            Console.WriteLine("Soap 2 Serialized completed!");
        }

        private static void Soap2Deserializing(string filepath){            
            Console.WriteLine("########### Customer ############");
            using (var fs = new FileStream(filepath+"customer_soap2.xml", FileMode.Open))
            {
                SoapFormatter formatter = new SoapFormatter();
                var customer = (SoapCustomer2)formatter.Deserialize(fs);
                Console.WriteLine(string.Format("{0}{1}", "Id: ".PadLeft(20), customer.CustomerId.ToString()));
                Console.WriteLine(string.Format("{0}{1}", "FirstName: ".PadLeft(20), customer.FirstName));
                Console.WriteLine(string.Format("{0}{1}", "LastName: ".PadLeft(20), customer.LastName));
                Console.WriteLine(string.Format("{0}{1}", "Balance: ".PadLeft(20), customer.Balance));
                Console.WriteLine(string.Format("{0}{1}", "Country: ".PadLeft(20), customer.Country));
                Console.WriteLine(string.Format("{0}{1}", "Homephone: ".PadLeft(20), customer.Homephone));
                Console.WriteLine(string.Format("{0}{1}", "Notes: ".PadLeft(20), customer.Notes));
            }
            Console.WriteLine("Soap 2 Deserialized completed!");
        }

        private static void SoapSerializing(string filepath){            
            var customer = new SoapCustomer{
                CustomerId = "5",
                FirstName = "Young",
                LastName="Bob5",
                Balance = "100.05",
                Country = "china",
                Homephone="12345678905",
                Notes=""
            };
            using(var fs = new FileStream(filepath + "customer_soap.xml", FileMode.Create)){
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fs, customer);
            }
            Console.WriteLine("Soap Serialized completed!");
        }

        private static void SoapDeserializing(string filepath){            
            Console.WriteLine("########### Customer ############");
            using (var fs = new FileStream(filepath+"customer_soap.xml", FileMode.Open))
            {
                SoapFormatter formatter = new SoapFormatter();
                var customer = (SoapCustomer)formatter.Deserialize(fs);
                Console.WriteLine(string.Format("{0}{1}", "Id: ".PadLeft(20), customer.CustomerId.ToString()));
                Console.WriteLine(string.Format("{0}{1}", "FirstName: ".PadLeft(20), customer.FirstName));
                Console.WriteLine(string.Format("{0}{1}", "LastName: ".PadLeft(20), customer.LastName));
                Console.WriteLine(string.Format("{0}{1}", "Balance: ".PadLeft(20), customer.Balance));
                Console.WriteLine(string.Format("{0}{1}", "Country: ".PadLeft(20), customer.Country));
                Console.WriteLine(string.Format("{0}{1}", "Homephone: ".PadLeft(20), customer.Homephone));
                Console.WriteLine(string.Format("{0}{1}", "Notes: ".PadLeft(20), customer.Notes));
            }
            Console.WriteLine("Soap Deserialized completed!");
        }

        private static void MySerializing(string filepath){
            var customer = new MyCustomer{
                CustomerId = "5",
                FirstName = "Young",
                LastName="Bob5",
                Balance = "100.05",
                Country = "china",
                Homephone="12345678905",
                Notes="",
                Type = MyCustomerType.Personal,
                Emails = new string[]{"yp1@test.com","yp2@test.com"}
            };
            using(var fs = new FileStream(filepath+"customer5.xml", FileMode.Create)){
                var serializer = new XmlSerializer(typeof(MyCustomer));
                serializer.Serialize(fs, customer);
            }
            Console.WriteLine("My Serialized completed!");
        }

        private static void Serializing(string filepath){
            var myAddress = new Address{
                Postcode = "610001",
                City = "CD"
            };
            var customer = new Customer{
                CustomerId = "5",
                FirstName = "Young",
                LastName="Bob5",
                Balance = "100.05",
                Country = "china",
                Homephone="12345678905",
                Notes="",
                Type = CustomerType.Personal,
                Emails = new string[]{"yp1@test.com","yp2@test.com"},
                MyAddress= myAddress
            };
            using(var fs = new FileStream(filepath+"customer4.xml", FileMode.Create)){
                var serializer = new XmlSerializer(typeof(Customer));
                serializer.Serialize(fs, customer);
            }
            using(var fs_vip = new FileStream(filepath+"customer_vip.xml", FileMode.Create)){
                VIPCustomer vip = new VIPCustomer{
                    CustomerId = "5",
                    FirstName = "Young",
                    LastName="Bob5",
                    Balance = "100.05",
                    Country = "china",
                    Homephone="12345678905",
                    Notes="",
                    Type = CustomerType.Personal,
                    Emails = new string[]{"yp1@test.com","yp2@test.com"},
                    MyAddress= myAddress,
                    VIPNo = 2020001
                };
                var serializer = new XmlSerializer(typeof(VIPCustomer));
                serializer.Serialize(fs_vip, vip);
            }
            Console.WriteLine("Serialized completed!");
        }

        private static void Deserializing(string filepath){
            Console.WriteLine("########### Customer ############");
            using (var fs = new FileStream(filepath+"customer4.xml", FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(Customer));
                serializer.UnknownAttribute += new XmlAttributeEventHandler(Serialize_UnknownAttribute);
                serializer.UnknownElement += new XmlElementEventHandler(Serialize_UnknownElement);
                serializer.UnknownNode += new XmlNodeEventHandler(Serialize_UnknownNode);
                Customer customer = (Customer)serializer.Deserialize(fs);
                Console.WriteLine(string.Format("{0}{1}", "Id: ".PadLeft(20), customer.CustomerId.ToString()));
                Console.WriteLine(string.Format("{0}{1}", "FirstName: ".PadLeft(20), customer.FirstName));
                Console.WriteLine(string.Format("{0}{1}", "LastName: ".PadLeft(20), customer.LastName));
                Console.WriteLine(string.Format("{0}{1}", "Balance: ".PadLeft(20), customer.Balance));
                Console.WriteLine(string.Format("{0}{1}", "Country: ".PadLeft(20), customer.Country));
                Console.WriteLine(string.Format("{0}{1}", "Homephone: ".PadLeft(20), customer.Homephone));
                Console.WriteLine(string.Format("{0}{1}", "Notes: ".PadLeft(20), customer.Notes));
                
                Console.WriteLine(string.Format("{0}{1}", "Type: ".PadLeft(20), customer.Type==CustomerType.Personal?"Personal":"Company"));
                Console.WriteLine(string.Format("{0}{1}", "Emails: ".PadLeft(20), string.Join(",", customer.Emails)));
                Console.WriteLine(string.Format("{0}{1}", "Address.Postcode: ".PadLeft(20), customer.MyAddress.Postcode));
                Console.WriteLine(string.Format("{0}{1}", "Address.City: ".PadLeft(20), customer.MyAddress.City));
            }
            Console.WriteLine("########### VIP Customer ############");
            using (var fs_vip = new FileStream(filepath+"customer_vip.xml", FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(VIPCustomer));
                serializer.UnknownAttribute += new XmlAttributeEventHandler(Serialize_UnknownAttribute);
                serializer.UnknownElement += new XmlElementEventHandler(Serialize_UnknownElement);
                serializer.UnknownNode += new XmlNodeEventHandler(Serialize_UnknownNode);
                VIPCustomer vip = (VIPCustomer)serializer.Deserialize(fs_vip);
                Console.WriteLine(string.Format("{0}{1}", "Id: ".PadLeft(20), vip.CustomerId.ToString()));
                Console.WriteLine(string.Format("{0}{1}", "VIPNo: ".PadLeft(20), vip.VIPNo.ToString()));
            }
            Console.WriteLine("Deserialized completed!");
        }

        private static void Serialize_UnknownNode(object sender, XmlNodeEventArgs e){
            Console.WriteLine($"Unknown Node [{e.Name}] found at line {e.LineNumber}.");
        }

        private static void Serialize_UnknownElement(object sender, XmlElementEventArgs e){
            Console.WriteLine($"Unknown Element [{e.Element.Name}] found at line {e.LineNumber}.");
        }

        private static void Serialize_UnknownAttribute(object sender, XmlAttributeEventArgs e){
            Console.WriteLine($"Unknown Attribute [{e.Attr.Name}] found at line {e.LineNumber}.");
        }

    }
}
