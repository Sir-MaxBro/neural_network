using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NeuralCompressed.Network.Attributes;

namespace NeuralCompressed.Network.Entities
{
    internal class Memory
    {
        private string _fileName;
        private XmlDocument _memory;
        private const string STATIC_PATH = "_memory.xml";
        public Memory(string fileName)
        {
            _fileName = fileName;
            MemoryInitialize();
        }

        private void MemoryInitialize()
        {
            string filePath = _fileName + STATIC_PATH;
            _memory = new XmlDocument();
            _memory.Load(filePath);
        }

        public XmlElement MemoryDocumentElement
        {
            get { return _memory.DocumentElement; }
        }

        public XmlDocument MemoryDocument
        {
            get { return _memory; }
        }

        public void Save()
        {
            if (_memory != null)
            {
                _memory.Save(_fileName);
            }
        }
    }
}
