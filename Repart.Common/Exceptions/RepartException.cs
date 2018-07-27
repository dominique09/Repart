using System;

namespace Repart.Common.Exceptions
{
    public class RepartException : Exception
    {
        public string Code { get; }

        public RepartException(){}

        public RepartException(string code)
        {
            Code = code;
        }

        public RepartException(string message, params object[] args) : this(string.Empty, message, args){}
        public RepartException(string code, string message, params object[] args) : this(null, code, message, args){}
        public RepartException(Exception innerException, string message, params object[] args) : this(innerException, string.Empty, message, args){}

        public RepartException(Exception innerException, string code, string message, params object[] args) : base(
            string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
