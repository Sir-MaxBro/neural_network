using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralCompressed.Network.Entities
{
    internal class Neuron
    {
        private double[] _weights;
        private double[] _inputs;

        public Neuron(double[] weights)
            : this(null, weights) { }

        public Neuron(double[] inputs, double[] weights)
        {
            _weights = weights;
            _inputs = inputs;
        }

        public double[] Weights
        {
            get { return _weights; }
            set { _weights = value; }
        }

        public double[] Inputs
        {
            get { return _inputs; }
            set { _inputs = value; }
        }

        public double Output
        {
            get { return Activator(_inputs, _weights); }
        }

        private double Activator(double[] inputs, double[] weights) //преобразования
        {
            double sum = 0;
            for (int i = 0; i < inputs.Length; ++i)
            {
                sum += inputs[i] * weights[i]; //линейные
            }
            return Math.Pow(1 + Math.Exp(-sum), -1); //нелинейные
        }

        public double Derivativator(double outsignal)
        {
            return outsignal * (1.0d - outsignal); //формула производной для текущей функции активации
        }

        public double Gradientor(double error, double dif, double gradientSum)
        {
            return error * dif + gradientSum * dif; //g_sum - это сумма градиентов следующего слоя
        }
    }
}
