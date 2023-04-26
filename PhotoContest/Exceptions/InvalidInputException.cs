using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Exceptions
{
    public class InvalidInputException: ApplicationException
    {
        public InvalidInputException(string message):base(message)
        {

        }
    }
}
