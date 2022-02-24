using System;
using System.Runtime.Serialization;

namespace DAO
{
    [Serializable]
    public class DuplicateBookNameException : Exception
    {
        public DuplicateBookNameException()
        {
        }

        public DuplicateBookNameException(string message) : base(message)
        {
        }

        public DuplicateBookNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateBookNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}