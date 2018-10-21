using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace Xebia.DatabaseCore.Common
{
    public class DatabaseEntityNotFoundException
        : Exception
    {
        public DatabaseEntityNotFoundException()
        {
        }

        public DatabaseEntityNotFoundException(string message)
            : base(message)
        {
        }

        public DatabaseEntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecuritySafeCritical]
        protected DatabaseEntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public static DatabaseEntityNotFoundException Create(Type type, QueryParameter[] parameters)
        {
            var message = "Failed to query type " + type.FullName;

            if (parameters != null && parameters.Length > 0)
            {
                message += " with query parameters " +
                                String.Join(", ", parameters.Select(x => string.Format("{0}={1}", x.Name, x.Value)));
            }

            return new DatabaseEntityNotFoundException(message + ".");
        }
    }
}
