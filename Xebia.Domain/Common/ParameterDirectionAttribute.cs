using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterDirectionAttribute
        : Attribute
    {
        public ParameterDirection ParameterDirection { get; private set; }

        public ParameterDirectionAttribute(ParameterDirection parameterDirection)
        {
            this.ParameterDirection = parameterDirection;
        }
    }
}
