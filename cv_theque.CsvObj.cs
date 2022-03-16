using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CV_Theque
{
    public class CSV_Obj
    {

        //
        private string? _id;
        private string? _lastname;
        private string? _firstname;
        private string? _age;
        private string? _birthday;
        private string? _profil;
        private string? _pc;
        private string? _city;
        private string? _email;

        private List<string>? _adress;
        private List<string>? _phone;
        private List<string>? _skills;
        private List<string>? _links;
        //
        public static Dictionary<int, string> methodDic = new Dictionary<int, string>()
        {{   0, "Id"         }, {   1, "LastName"   }, {   2, "FirstName"  },
         {   3, "Age"        }, {   4, "Birthday"   }, {   5, "Adress"     },
         {   7, "PostalCode" }, {   8, "City"       }, {   9, "Phone"      },
         {   11, "Email"     }, {   12, "Profil"    }, {   13, "Skills"    }, 
         {   23, "Links"     }
        };

        public string Id(string? val = null)
        {
            if (val != null) { _id = val; } 
            return _id;
        }

        public string LastName( string? val = null )
        {
            if (val != null) { _lastname = val; }
            return _lastname;
        }

        public string FirstName( string? val = null )
        {
            if (val != null) { _firstname = val; }
            return _firstname;
        }

        public string Age( string? val = null )
        {
            if (val != null) { _age = val; }
            return _age;
        }

        public string Birthday(string? val = null)
        {
            if (val != null) { _birthday = val; }
            return _birthday;
        }

        public string PostalCode(string? val = null)
        {
            if (val != null) { _pc = val; }
            return _pc;
        }

        public string City(string? val = null)
        {
            if (val != null) { _city = val; }
            return _city;
        }

        public string Email(string? val = null)
        {
            if (val != null) { _email = val; }
            return _email;
        }

        public string Profil(string? val = null)
        {
            if (val != null) { _profil = val; }
            return _profil;
        }

        public List<string> Phone(string? val = null)
        {
            if (_phone == null) { _phone = new List<string>(); }
            if (val != null) { _phone.Add(val); }
            return _phone;
        }

        public List<string> Adress(string? val = null)
        {
            if (_adress == null) { _adress = new List<string>(); }
            if (val != null) { _adress.Add(val); }
            return _adress;
        }

        public List<string> Skills(string? val = null)
        {
            if (_skills == null) { _skills = new List<string>(); }
            if (val != null) { _skills.Add(val); }
            return _skills;
        }

        public List<string> Links(string? val = null)
        {
            if (_links == null) { _links = new List<string>(); }
            if (val != null) { _links.Add(val); }
            return _links;
        }

        public static int GetMethodIndex(int idx)
        {
            int methodIndex = idx;

            switch (methodIndex)
            {
                case >= 23:
                    methodIndex = 23;
                    break;
                case > 13:
                    methodIndex = 13;
                    break;
                case 10:
                    methodIndex = 9;
                    break;
                case 6:
                    methodIndex = 5;
                    break;
                default:
                    break;
            }

            return methodIndex;
        }

        public static CSV_Obj GenerateObj( string data )
        {
            Dictionary<int, string[]> paramDic = new Dictionary<int, string[]>();
            string[] values = data.Split( ';' );
            CSV_Obj self = new CSV_Obj();

            for ( int i = 0; i < values.Length; i++ )
            {
                int methodIndex = GetMethodIndex(i);

                string[] TempArg = new string[1] { values[i].ToString() };
                MethodInfo? SelfMethod = self.GetType()
                                             .GetMethod( methodDic[ methodIndex ] );
                if ( SelfMethod != null)
                {
                    SelfMethod.Invoke(self, TempArg);
                }
            }

            return self;
        }
    }
}
