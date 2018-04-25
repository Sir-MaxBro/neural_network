using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralCompressed.Network.Abstract;
using NeuralCompressed.Network.Attributes;

namespace NeuralCompressed.Network
{
    [LayerMemory("output_layer")]
    internal class OutputLayer : Layer
    {
        public OutputLayer(int neuronsCount, int previousNeuronsCount)
            : base(neuronsCount, previousNeuronsCount) { }

        public override void Recognize(NeuralNetwork network, Layer nextLayer)
        {
            for (int i = 0; i < Neurons.Length; ++i)
            {
                network._outputs[i] = Neurons[i].Output;
            }
        }

        public override double[] BackwardPass(double[] errors)
        {
            double[] gradientSum = new double[_previousNeuronsCount];
            for (int i = 0; i < gradientSum.Length; ++i)//вычисление градиентных сумм выходного слоя
            {
                double sum = 0;
                for (int j = 0; j < Neurons.Length; ++j)
                {
                    double gradientor = Neurons[j].Gradientor(errors[j], Neurons[j].Derivativator(Neurons[j].Output), 0);
                    sum += Neurons[j].Weights[i] * gradientor; //через ошибку и производную
                    Neurons[j].Weights[i] += Neurons[j].Inputs[i] * LEARNING_GRATE * gradientor; //коррекция весов         
                }
                gradientSum[i] = sum;
            }
            return gradientSum;
        }
    }
}
