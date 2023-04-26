using System;

namespace PhotoContest.Exceptions
{
    public class DuplicateEntityException : ApplicationException
    {
        public DuplicateEntityException(string message) : base(message)
        {

        }
    }
}
