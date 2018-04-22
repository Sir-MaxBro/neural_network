using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGenerator
{
    public interface IHashValue
    {
        int Key { get; set; }

        int Value { get; set; }
    }
}
