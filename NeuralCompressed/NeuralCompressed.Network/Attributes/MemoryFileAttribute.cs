using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralCompressed.Network.Attributes
{
    internal class LayerMemoryAttribute : Attribute
    {
        private string _layerName;
        public LayerMemoryAttribute(string layerName)
        {
            _layerName = layerName;
        }

        public string LayerName
        {
            get { return _layerName; }
        }
    }
}
