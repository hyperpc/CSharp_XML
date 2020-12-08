using System;
using System.Configuration;
using System.Data;

//dotnet add package System.Configuration.ConfigurationManager --version 4.7.0
namespace CustomerLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ICustomerService" in both code and config file together.
    public class CustomerService : ICustomerService
    {
        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}

        public Customer GetCustomer(int id)
        {
            try
            {

                var customer = new Customer();
                //throw new NotImplementedException();
                var ds = GetCustomers();
                if (ds == null || ds.Tables.Count < 1)
                    return null;

                var columnCnt = ds.Tables[0].Columns.Count;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row == null)
                        continue;

                    var _id = row["id"].ToString();
                    if (_id == id.ToString())
                    {
                        for (int i = 0; i < columnCnt; i++)
                        {
                            var columnName = ds.Tables[0].Columns[i].ColumnName.ToString();
                            switch (columnName)
                            {
                                case "firstname":
                                    customer.FirstName = row[columnName].ToString();
                                    break;
                                case "lastname":
                                    customer.LastName = row[columnName].ToString();
                                    break;
                                case "balance":
                                    customer.Balance = row[columnName].ToString();
                                    break;
                                case "country":
                                    customer.Country = row[columnName].ToString();
                                    break;
                                case "homephone":
                                    customer.Homephone = row[columnName].ToString();
                                    break;
                                case "notes":
                                    customer.Notes = row[columnName].ToString();
                                    break;
                                case "id":
                                    var custId = row[columnName].ToString();
                                    int _custId = 0;
                                    if(Int32.TryParse(custId, out _custId))
                                    {
                                        customer.CustomerId = _custId;
                                    }
                                    break;
                            }
                        }
                    }
                }
                return customer;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public DataSet GetCustomers()
        {
            try
            {
                //throw new NotImplementedException();
                var filepath = ConfigurationManager.AppSettings["filepath"];
                DataSet ds = new DataSet();
                ds.ReadXml(filepath + "customers.xml");
                return ds;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
