using System.Runtime.Serialization;
using System.Dynamic;
//using System.Net.Http.Headers;
using System.Text;
using System;
namespace XmlSerializeDemo
{
    [Serializable]
    public class SoapCustomer{
        public string CustomerId{get;set;}
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public string Balance{get;set;}
        public string Country{get;set;}
        public string Homephone{get;set;}
        public string Notes{get;set;}
    }
    [Serializable]
    public class SoapCustomer2{
        public string CustomerId{get;set;}
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public string Balance{get;set;}
        public string Country{get;set;}
        public string Homephone{get;set;}
        public string Notes{get;set;}

        private string Encode(string str){
            byte[] data = ASCIIEncoding.ASCII.GetBytes(str);
            return Convert.ToBase64String(data);
        }
        private string Decode(string str){
            byte[] data = Convert.FromBase64String(str);
            return ASCIIEncoding.ASCII.GetString(data);
        }
        
        [OnSerializingAttribute]
        public void OnSerializing(StreamingContext context){
            this.CustomerId=Encode(this.CustomerId);
            this.FirstName=Encode(this.FirstName);
            this.LastName=Encode(this.LastName);
            this.Balance=Encode(this.Balance);
            this.Country=Encode(this.Country);
            this.Homephone=Encode(this.Homephone);
            this.Notes=Encode(this.Notes);
        }
        [OnSerializedAttribute]
        public void OnSerialized(StreamingContext context){
            this.CustomerId=Decode(this.CustomerId);
            this.FirstName=Decode(this.FirstName);
            this.LastName=Decode(this.LastName);
            this.Balance=Decode(this.Balance);
            this.Country=Decode(this.Country);
            this.Homephone=Decode(this.Homephone);
            this.Notes=Decode(this.Notes);
        }
        
        [OnDeserializingAttribute]
        public void OnDeserializing(StreamingContext context){
        }
        [OnDeserializedAttribute]
        public void OnDeserialized(StreamingContext context){
            this.CustomerId=Decode(this.CustomerId);
            this.FirstName=Decode(this.FirstName);
            this.LastName=Decode(this.LastName);
            this.Balance=Decode(this.Balance);
            this.Country=Decode(this.Country);
            this.Homephone=Decode(this.Homephone);
            this.Notes=Decode(this.Notes);
        }
    }
}