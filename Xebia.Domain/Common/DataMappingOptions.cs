using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.DatabaseCore.Common
{
    public class DataMappingOptions
    {
        public DataMappingOptions()
        {
            ReadDateTimeAs = DateTimeKind.Local;
        }

        public DateTimeKind ReadDateTimeAs { get; set; }
    }
}
