using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralCompressed.Network.Entities;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace NeuralCompressed.Network.Abstract
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
            double[,] weightsSource = WeightInitialize(MemoryMode.GET);
            for (int i = 0; i < _neuronsCount; ++i)
            {
                double[] weights = new double[_previousNeuronsCount];
                for (int j = 0; j < _previousNeuronsCount; ++j)
                {
                    weights[j] = weightsSource[i, j];
                }
                Neurons[i] = new Neuron(null, weights, neuronType);//про подачу null на входы ниже
            }
        }

        public Neuron[] Neurons
        {
            get { return _neurons; }
            set { _neurons = value; }
        }

        public double[] Data//я подал null на входы нейронов, так как
        {//сначала нужно будет преобразовать информацию
            set//(видео, изображения, etc.)
            {//а загружать input'ы нейронов слоя надо не сразу,
                for (int i = 0; i < Neurons.Length; ++i)
                {
                    Neurons[i].Inputs = value;
                }
            }//а только после вычисления выходов предыдущего слоя
        }

        public double[,] WeightInitialize(MemoryMode memoryMode)
        {
            double[,] weights = new double[_neuronsCount, _previousNeuronsCount];

            string filePath = _neuronType.ToString() + "_memory.xml";
            XmlDocument memoryDoc = new XmlDocument();
            memoryDoc.Load(filePath);
            XmlElement memoryElements = memoryDoc.DocumentElement;
            switch (memoryMode)
            {
                case MemoryMode.GET:
                    for (int i = 0; i < weights.GetLength(0); ++i)
                    {
                        for (int j = 0; j < weights.GetLength(1); ++j)
                        {
                            weights[i, j] = double.Parse(memoryElements.ChildNodes.Item(j + weights.GetLength(1) * i).InnerText.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case MemoryMode.SET:
                    for (int i = 0; i < Neurons.Length; ++i)
                    {
                        for (int j = 0; j < _previousNeuronsCount; ++j)
                        {
                            memoryElements.ChildNodes.Item(j + _previousNeuronsCount * i).InnerText = Neurons[i].Weights[j].ToString();
                        }
                    }
                    break;
            }

            memoryDoc.Save(filePath);
            return weights;
        }

        abstract public void Recognize(NeuralNetwork net, Layer nextLayer); //для прямых проходов
        abstract public double[] BackwardPass(double[] stuff); //и обратных  
    }
}
