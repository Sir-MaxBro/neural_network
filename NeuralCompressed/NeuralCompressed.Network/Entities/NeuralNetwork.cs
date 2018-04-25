using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralCompressed.Network
{
    public class NeuralNetwork
    {
        internal HiddenLayer hiddenLayer = new HiddenLayer(8, 4);
        internal OutputLayer outputLayer = new OutputLayer(2, 8);


        public double[] _outputs = new double[2]; //вывод сети
        private double[] _inputs; //входы
        private static NeuralNetwork _instance;

        private NeuralNetwork(double[] inputs)
        {
            _inputs = inputs;
        }

        public static NeuralNetwork GetInstance(double[] inputs)
        {
            if (_instance == null)
            {
                _instance = new NeuralNetwork(inputs);
            }
            return _instance;
        }

        public double[] Inputs
        {
            get { return _inputs; }
            set { _inputs = value; }
        }

        // сделать ввод одних данных
        public double[] Outputs()
        {
            hiddenLayer.Data = _inputs;
            hiddenLayer.Recognize(null, outputLayer);
            outputLayer.Recognize(this, null);
            return _outputs;
        }

        //ошибка одной итерации обучения
        protected internal double GetIterationError(double[] errors)
        {
            double sum = 0;
            for (int i = 0; i < errors.Length; ++i)
            {
                sum += Math.Pow(errors[i], 2);
            }
            return 0.5d * sum;
        }

        //ошибка эпохи
        protected internal double GetEraError(double[] iterationsError)
        {
            double sum = 0;
            for (int i = 0; i < iterationsError.Length; ++i)
            {
                sum += iterationsError[i];
            }
            return (sum / iterationsError.Length);
        }
    }
}
