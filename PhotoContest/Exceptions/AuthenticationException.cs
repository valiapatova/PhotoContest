using System;

namespace PhotoContest.Exceptions
{
    public class AuthenticationException : ApplicationException
    {
        public AuthenticationException(string message): base(message)
        {

        }
    }
}
