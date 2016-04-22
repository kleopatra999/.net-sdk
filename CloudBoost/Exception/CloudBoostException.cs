using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Exception
{
    public class CloudBoostException : System.Exception
    {
        public CloudBoostException(string message) : base(message)
        {
            // do nothing. 
        }
    }
}
