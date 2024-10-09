using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class BusinessException : System.Exception
    {
        public BusinessException() { }

        public BusinessException(string message) : base(message)
        {
        }
    }
}
