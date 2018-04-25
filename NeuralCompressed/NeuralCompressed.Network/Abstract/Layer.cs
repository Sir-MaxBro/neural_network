using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralCompressed.Network.Entities;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using NeuralCompressed.Network.Attributes;

namespace NeuralCompressed.Network.Abstract
{
    internal abstract class Layer
    {
        // current neurons number
        protected int _neuronsCount;
        // neurons number on the previous layer
        protected int _previousNeuronsCount;
        // learning grate
        protected const double LEARNING_GRATE = 0.1d;
        private Neuron[] _neurons;
        protected Layer(int neuronsCount, int previousNeuronsCount)
        {
            _neuronsCount = neuronsCount;
            _previousNeuronsCount = previousNeuronsCount;
            _neurons = new Neuron[neuronsCount];
            // weights initialize
            double[,] weightsSource = GetWeights();
            for (int i = 0; i < _neuronsCount; ++i)
            {
                double[] weights = new double[_previousNeuronsCount];
                for (int j = 0; j < _previousNeuronsCount; ++j)
                {
                    weights[j] = weightsSource[i, j];
                }
                // initialize neurouns
                Neurons[i] = new Neuron(weights);
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
                    Neurons[i].Inputs = value.Select(x => x == 0 ? x : 1.0d / x).ToArray();
                }
            }//а только после вычисления выходов предыдущего слоя
        }

        public double[,] GetWeights()
        {
            double[,] weights = new double[_neuronsCount, _previousNeuronsCount];
            // get file name
            string filePath = GetLayerName();
            // get memory file
            Memory memory = new Memory(filePath);
            XmlElement memoryElements = memory.MemoryDocumentElement;
            for (int i = 0; i < weights.GetLength(0); ++i)
            {
                for (int j = 0; j < weights.GetLength(1); ++j)
                {
                    // initialize weight
                    weights[i, j] = double.Parse(memoryElements.ChildNodes.Item(j + weights.GetLength(1) * i).InnerText.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return weights;
        }

        public void SetWeight()
        {
            // get file name
            string filePath = GetLayerName();
            // get memory file
            Memory memory = new Memory(filePath);
            XmlElement memoryElements = memory.MemoryDocumentElement;
            for (int i = 0; i < _neuronsCount; ++i)
            {
                for (int j = 0; j < _previousNeuronsCount; ++j)
                {
                    // set weight
                    memoryElements.ChildNodes.Item(j + _previousNeuronsCount * i).InnerText = Neurons[i].Weights[j].ToString();
                }
            }
            // save
            memory.Save();
        }

        private string GetLayerName()
        {
            // get memory file attribute
            var attributes = this.GetType()
                .GetCustomAttributes(typeof(LayerMemoryAttribute), false)
                .Cast<LayerMemoryAttribute>();
            // get layer name
            string layerName = attributes
                .ElementAt(0)
                .LayerName;
            return layerName;
        }

        abstract public void Recognize(NeuralNetwork net, Layer nextLayer); //для прямых проходов
        abstract public double[] BackwardPass(double[] stuff); //и обратных  
    }
}
