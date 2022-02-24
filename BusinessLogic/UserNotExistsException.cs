using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    public class UserNotExistsException : Exception
    {
        public UserNotExistsException()
        {
        }

        public UserNotExistsException(string message) : base(message)
        {
        }

        public UserNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}