using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Exceptions
{
    [Serializable]
    public class GenderNotFoundException: Exception
    {
        public GenderNotFoundException(string message) 
            : base(message) { }
        
    }
}
