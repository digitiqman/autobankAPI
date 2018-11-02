using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBank.Helpers
{
    [Serializable]
    public class AccountException : ApplicationException
    {
        public AccountException(string Message,
                     Exception innerException) : base(Message, innerException) { }
        public AccountException(string Message) : base(Message) { }
        public AccountException() { }

        #region Serializeable Code
        public AccountException(SerializationInfo info,
              StreamingContext context) : base(info, context) { }
        #endregion Serializeable Code
    }
}