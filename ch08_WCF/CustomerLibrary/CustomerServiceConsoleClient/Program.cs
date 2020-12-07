using CustomerLibrary;
using System;
using System.ServiceModel;

namespace CustomerServiceHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Type t = typeof(CustomerService);
                Uri tcp = new Uri("net.tcp://localhost:8732/CustomerService");
                Uri http = new Uri("http://localhost:8733/CustomerService");
                ServiceHost host = new ServiceHost(t, tcp, http);
                host.Open();
                Console.WriteLine("Published");
                Console.ReadLine();
                host.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.ReadLine();
            }
        }
    }
}
