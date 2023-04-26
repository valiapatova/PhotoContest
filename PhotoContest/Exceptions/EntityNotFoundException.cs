using System;

namespace PhotoContest.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}
