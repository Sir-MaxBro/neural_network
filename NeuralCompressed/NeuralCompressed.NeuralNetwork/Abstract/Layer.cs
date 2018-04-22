using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralCompressed.NeuralNetwork.Entities;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NeuralCompressed.NeuralNetwork.Abstract
{
    public abstract class Layer
    {
        protected int _neuronsCount;
        protected int _previousNeuronsCount;
        protected const double LEARNING_GRATE = 0.1d;
        private Neuron[] _neurons;
        private NeuronType _neuronType;
        protected Layer(int neuronsCount, int previousNeuronsCount, NeuronType neuronType)
        {
            _neuronsCount = neuronsCount;
            _previousNeuronsCount = previousNeuronsCount;
            _neurons = new Neuron[neuronsCount];
            _neuronType = neuronType;
        }

        public Neuron[] Neurons
        {
            get { return _neurons; }
            set { _neurons = value; }
        }

        public double[,] WeightInitialize(MemoryMode memoryMode)
        {
            double[,] weights = new double[_neuronsCount, _previousNeuronsCount];

            string fileName = _neuronType.ToString() + "_memory.xml";
            XDocument memoryDoc = XDocument.Load(fileName);
            var nodes = memoryDoc.Root.Element("weights").Elements("weight");
            switch (memoryMode)
            {
                case MemoryMode.GET:
                    for (int i = 0; i < weights.GetLength(0); i++)
                    {
                        for (int j = 0; j < weights.GetLength(1); j++)
                        {
                            weights[i, j] = double.Parse(nodes.ElementAt(j + weights.GetLength(1) * i).Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);//parsing stuff
                        }
                    }
                    break;
                case MemoryMode.SET:
                    for (int i = 0; i < weights.GetLength(0); i++)
                    {
                        for (int j = 0; j < weights.GetLength(1); j++)
                        {
                            nodes.ElementAt(j + weights.GetLength(1) * i).Value = _neurons[i].Weights[j].ToString();
                            //weights[i, j] = double.Parse(nodes.ElementAt(j + weights.GetLength(1) * i).Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);//parsing stuff
                        }
                    }
                    break;
            }

            

            return weights;
        }
    }
}
