using System.Runtime.Serialization;

namespace CustomerLibrary
{
    [DataContract]
    public class Customer
    {
        private int _CustId;
        private string _FName;
        private string _LName;
        private string _Balance;
        private string _Country;
        private string _Homephone;
        private string _Notes;

        [DataMember]
        public int CustomerId
        {
            get
            {
                return _CustId;
            }
            set
            {
                _CustId = value;
            }
        }

        [DataMember]
        public string FirstName
        {
            get
            {
                return _FName;
            }
            set
            {
                _FName = value;
            }
        }

        [DataMember]
        public string LastName
        {
            get
            {
                return _LName;
            }
            set
            {
                _LName = value;
            }
        }

        [DataMember]
        public string Balance
        {
            get
            {
                return _Balance;
            }
            set
            {
                _Balance = value;
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return _Country;
            }
            set
            {
                _Country = value;
            }
        }

        [DataMember]
        public string Homephone
        {
            get
            {
                return _Homephone;
            }
            set
            {
                _Homephone = value;
            }
        }

        [DataMember]
        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                _Notes = value;
            }
        }
    }
}
