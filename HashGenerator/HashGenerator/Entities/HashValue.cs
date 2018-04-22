using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGenerator
{
    public class HashValue : IHashValue
    {
        public int Key { get; set; }

        public int Value { get; set; }
    }
}
