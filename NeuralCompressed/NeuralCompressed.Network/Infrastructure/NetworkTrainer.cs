using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralCompressed.Network
{
    public class NetworkTrainer
    {
        protected internal static readonly double THRESHOLD = 0.001d; //порог ошибки
        public static void Train(NeuralNetwork network, IList<Tuple<double[], double[]>> trainset) //backpropagation method
        {
            double[] iterationError = new double[trainset.Count]; //массив для хранения ошибок итераций
            double eraError = 0; //текущее значение ошибки по эпохе
            do
            {
                for (int i = 0; i < trainset.Count; ++i)
                {
                    //прямой проход
                    network.Inputs = trainset[i].Item1;
                    var outputs = network.Outputs();

                    //вычисление ошибки по итерации
                    double[] errors = new double[trainset[i].Item2.Length];

                    for (int x = 0; x < errors.Length; ++x)
                    {
                        // расчет ошибки
                        errors[x] = trainset[i].Item2[x] - outputs[x];
                    }
                    iterationError[i] = network.GetIterationError(errors);
                    //обратный проход и коррекция весов
                    double[] gradientSums = network.outputLayer.BackwardPass(errors);
                    network.hiddenLayer.BackwardPass(gradientSums);
                }
                eraError = network.GetEraError(iterationError);//вычисление ошибки по эпохе
                //debugging
                Console.WriteLine(eraError.ToString("f16"));
            } while (eraError > THRESHOLD);
            //загрузка скорректированных весов в "память"
            network.hiddenLayer.SetWeight();
            network.outputLayer.SetWeight();
        }

        public static void Test(NeuralNetwork network, IList<Tuple<double[], double[]>> trainset)
        {
            for (int i = 0; i < trainset.Count; ++i)
            {
                network.hiddenLayer.Data = trainset[i].Item1;
                network.hiddenLayer.Recognize(null, network.outputLayer);
                network.outputLayer.Recognize(network, null);
                for (int j = 0; j < network._outputs.Length; ++j)
                {
                    Console.WriteLine(network._outputs[j]);
                }
                Console.WriteLine();
            }
        }
    }
}
