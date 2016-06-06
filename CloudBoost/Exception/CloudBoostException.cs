using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Exception
{
    /// <summary>
    /// CloudBoost Exception
    /// </summary>
    public class CloudBoostException : System.Exception
    {
        /// <summary>
        /// Constructor of CloudBoost Exception
        /// </summary>
        /// <param name="message">Exception string</param>
        public CloudBoostException(string message) : base(message)
        {
            // do nothing. 
        }
    }
}
