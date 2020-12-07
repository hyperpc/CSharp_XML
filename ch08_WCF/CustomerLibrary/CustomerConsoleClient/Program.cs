using System;
using System.Data;

namespace CustomerConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GetCustomers();

            GetCustomerById(3);

            Console.ReadLine();
        }

        private static void GetCustomers()
        {
            CustomerServiceReference.CustomerServiceClient proxy =
                new CustomerServiceReference.CustomerServiceClient("BasicHttpBinding_ICustomerService");
            DataSet ds = proxy.GetCustomers();
            if (ds == null || ds.Tables.Count < 1)
            {
                return;
            }

            Console.WriteLine($"TableName: {ds.Tables[0].TableName}");
            var columnCnt = ds.Tables[0].Columns.Count;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row == null)
                    continue;

                Console.WriteLine("#######################################");
                for (int i = 0; i < columnCnt; i++)
                {
                    var columnName = ds.Tables[0].Columns[i].ColumnName.ToString().PadLeft(10);
                    Console.WriteLine($"-----{columnName}: {row[columnName.Trim()].ToString()}");
                }
            }
            Console.WriteLine("#######################################");

            proxy.Close();
        }

        private static void GetCustomerById(int id)
        {
            CustomerServiceReference.CustomerServiceClient proxy =
                new CustomerServiceReference.CustomerServiceClient("BasicHttpBinding_ICustomerService");
            var customer = proxy.GetCustomer(id);
            if (customer == null)
            {
                return;
            }
            Console.WriteLine("#######################################");
            Console.WriteLine($"Customer.FirstName: {customer.FirstName}");
            Console.WriteLine($" Customer.LastName: {customer.LastName}");
            Console.WriteLine($"  Customer.Balance: {customer.Balance}");
            Console.WriteLine($"  Customer.Country: {customer.Country}");
            Console.WriteLine($"Customer.Homephone: {customer.Homephone}");
            Console.WriteLine($"    Customer.Notes: {customer.Notes}");
            Console.WriteLine($"       Customer.Id: {customer.CustomerId}");
            Console.WriteLine("#######################################");

            proxy.Close();
        }
    }
}
