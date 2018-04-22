using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralCompressed.Network.Abstract;

namespace NeuralCompressed.Network
{
    public class HiddenLayer : Layer
    {
        public HiddenLayer(int neuronsCount, int previousNeuronsCount, NeuronType neuronType)
            : base(neuronsCount, previousNeuronsCount, neuronType) { }

        public override void Recognize(NeuralNetwork net, Layer nextLayer)
        {
            double[] hiddenOut = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; ++i)
            {
                hiddenOut[i] = Neurons[i].Output;
            }
            nextLayer.Data = hiddenOut;
        }

        public override double[] BackwardPass(double[] gr_sums)
        {
            double[] gr_sum = null;
            //сюда можно всунуть вычисление градиентных сумм для других скрытых слоёв
            //но градиенты будут вычисляться по-другому, то есть
            //через градиентные суммы следующего слоя и производные
            for (int i = 0; i < _neuronsCount; ++i)
            {
                for (int n = 0; n < _previousNeuronsCount; ++n)
                {
                    Neurons[i].Weights[n] += LEARNING_GRATE * Neurons[i].Inputs[n] * Neurons[i].Gradientor(0, Neurons[i].Derivativator(Neurons[i].Output), gr_sums[i]);//коррекция весов
                }
            }
            return gr_sum;
        }
    }
}
