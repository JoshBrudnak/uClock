using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueITApplication
{
    /// <summary>
    /// Used to create a list of users
    /// </summary>
    public class Person
    {
        private string _name;

        public string Name
        {
            get { return _name; }

            set { _name = value; }
        }
    }
}
