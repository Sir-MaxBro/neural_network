using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralCompressed.Network.Abstract;

namespace NeuralCompressed.Network
{
    public class OutputLayer : Layer
    {
        public OutputLayer(int neuronsCount, int previousNeuronsCount, NeuronType neuronType)
            : base(neuronsCount, previousNeuronsCount, neuronType) { }

        public override void Recognize(NeuralNetwork network, Layer nextLayer)
        {
            for (int i = 0; i < Neurons.Length; ++i)
            {
                network.fact[i] = Neurons[i].Output;
            }
        }

        public override double[] BackwardPass(double[] errors)
        {
            double[] gr_sum = new double[_previousNeuronsCount];
            for (int j = 0; j < gr_sum.Length; ++j)//вычисление градиентных сумм выходного слоя
            {
                double sum = 0;
                for (int k = 0; k < Neurons.Length; ++k)
                {
                    sum += Neurons[k].Weights[j] * Neurons[k].Gradientor(errors[k], Neurons[k].Derivativator(Neurons[k].Output), 0);//через ошибку и производную
                }
                gr_sum[j] = sum;
            }
            for (int i = 0; i < _neuronsCount; ++i)
            {
                for (int n = 0; n < _previousNeuronsCount; ++n)
                {
                    Neurons[i].Weights[n] += LEARNING_GRATE * Neurons[i].Inputs[n] * Neurons[i].Gradientor(errors[i], Neurons[i].Derivativator(Neurons[i].Output), 0);//коррекция весов
                }
            }
            return gr_sum;
        }
    }
}
