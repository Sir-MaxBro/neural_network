using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashGenerator;

namespace HashGenerator.Generator
{
    public class HashGen
    {
        private readonly int _startValue;
        private readonly int _endValue;
        private readonly int _step;
        private const int START_GAP = 0;
        private const int END_GAP = 1000;
        private Hashtable _hashList = new Hashtable();

        public HashGen(int start, int end)
        {
            _startValue = start;
            _endValue = end;
            _step = (END_GAP - START_GAP) / (_endValue - _startValue);
        }

        public int Step
        {
            get { return _step; }
        }

        public Hashtable Generate()
        {
            int value = _startValue;
            for (long i = START_GAP; i <= END_GAP; i += _step)
            {
                _hashList.Add(key: i, value: value);
                value++;
            }
            return _hashList;
        }
    }
}
