using System.Xml.Serialization;
namespace XmlSerializeDemo
{
    public class VIPCustomer:Customer{
        public int VIPNo{get;set;}
    }
    public class Customer{
        public string CustomerId{get;set;}
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public string Balance{get;set;}
        public string Country{get;set;}
        public string Homephone{get;set;}
        public string Notes{get;set;}
        public CustomerType Type{get;set;}
        public string[] Emails{get;set;}
        public Address MyAddress{get;set;}
    }
    [XmlRootAttribute("Customer", Namespace="", IsNullable=false)]
    public class MyCustomer{
        [XmlAttributeAttribute(AttributeName="CustomerId")]
        public string CustomerId{get;set;}
        [XmlElementAttribute(ElementName="FName")]
        public string FirstName{get;set;}
        [XmlElementAttribute(ElementName="LName")]
        public string LastName{get;set;}
        [XmlIgnoreAttribute]
        public string Balance{get;set;}
        [XmlIgnoreAttribute]
        public string Country{get;set;}
        [XmlIgnoreAttribute]
        public string Homephone{get;set;}
        [XmlElementAttribute(ElementName="Remarks")]
        public string Notes{get;set;}
        [XmlElementAttribute(ElementName="CustomerType")]
        public MyCustomerType Type{get;set;}
        [XmlArrayAttribute(ElementName="EmailAddresses")]
        [XmlArrayItemAttribute(ElementName="Email")]
        public string[] Emails{get;set;}
        [XmlElementAttribute(IsNullable=true)]
        public Address MyAddress{get;set;}
    }

    public class Address
    {
        public string Postcode{get;set;}
        public string City{get;set;}
    }
    public enum CustomerType{
        Personal,
        Company
    }
    public enum MyCustomerType{
        [XmlEnumAttribute(Name="Personal Customer")]
        Personal,
        [XmlEnumAttribute(Name="Company Customer")]
        Company
    }
}