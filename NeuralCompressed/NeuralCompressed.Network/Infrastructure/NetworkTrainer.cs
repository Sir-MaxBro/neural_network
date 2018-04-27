using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NeuralCompressed.Network
{
    public class NetworkTrainer
    {
        protected internal static readonly double THRESHOLD = 0.001d; //порог ошибки
        public static void Train(NeuralNetwork network/*, IList<Tuple<double[], double[]>> trainset*/) //backpropagation method
        {
            var trainset = GetTrainset();
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

        private static IList<Tuple<double[], double[]>> GetTrainset()
        {
            var trainset = new List<Tuple<double[], double[]>>();
            string path = ".\\trainset\\trainset_8_2.xml";
            XmlDocument memory = new XmlDocument();
            memory.Load(path);
            var memoryElements = memory.SelectSingleNode("trainsets").SelectNodes("trainset");
            for (int i = 0; i < memoryElements.Count; i++)
            {
                var inputs = memoryElements.Item(i).SelectSingleNode("inputs").ChildNodes;
                var outputs = memoryElements.Item(i).SelectSingleNode("outputs").ChildNodes;
                double[] arrInputs = new double[inputs.Count];
                double[] arrOutputs = new double[outputs.Count];
                for (int j = 0; j < inputs.Count; j++)
                {
                    arrInputs[j] = double.Parse(inputs.Item(j).InnerText.ToString());
                }

                for (int j = 0; j < outputs.Count; j++)
                {
                    arrOutputs[j] = 1.0 / double.Parse(outputs.Item(j).InnerText.ToString());
                }

                var tuple = Tuple.Create<double[], double[]>(arrInputs, arrOutputs);
                trainset.Add(tuple);
            }

            return trainset;
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
