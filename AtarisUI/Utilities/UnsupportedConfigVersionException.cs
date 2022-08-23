using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtarisUI
{
    public class UnsupportedConfigVersionException : Exception
    {
        public UnsupportedConfigVersionException(int version) : base($"Config version \"{version}\" is not supported by AtarisUI ${App.Version}")
        {

        }
    }
}
