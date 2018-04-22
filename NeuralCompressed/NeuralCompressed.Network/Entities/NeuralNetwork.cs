using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralCompressed.Network
{
    public class NeuralNetwork
    {
        //public HiddenLayer hidden_layer = new HiddenLayer(12, 6, NeuronType.Hidden);
        //public OutputLayer output_layer = new OutputLayer(2, 12, NeuronType.Output);

        //public HiddenLayer hidden_layer = new HiddenLayer(16, 8, NeuronType.Hidden);
        //public OutputLayer output_layer = new OutputLayer(2, 16, NeuronType.Output);

        public HiddenLayer hiddenLayer = new HiddenLayer(8, 4, NeuronType.Hidden);
        public OutputLayer outputLayer = new OutputLayer(2, 8, NeuronType.Output);

        private static IList<Tuple<double[], double[]>> _trainset;

        //private static IList<Tuple<double[], double[]>> _trainset = new List<Tuple<double[], double[]>>
        //{
        //    Tuple.Create<double[], double[]>(new double[]{ 0, 0 }, new double[]{ 0, 1 }),
        //    Tuple.Create<double[], double[]>(new double[]{ 0, 1 }, new double[]{ 1, 0 }),
        //    Tuple.Create<double[], double[]>(new double[]{ 1, 0 }, new double[]{ 1, 0 }),
        //    Tuple.Create<double[], double[]>(new double[]{ 1, 1 }, new double[]{ 0, 1 })
        //};

        public NeuralNetwork()
        {
            //_trainset = new List<Tuple<double[], double[]>>
            //{
            //    Tuple.Create<double[], double[]>(new double[]{ 97, 85, 97, 85, 97, 85, 97, 85 }, new double[]{ 1.0 / 97, 1.0 / 85 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 97, 85, 90, 120, 90, 120, 90, 120 }, new double[]{ 1.0 / 90, 1.0 / 120 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 90, 95, 93, 93, 95, 93, 95, 93 }, new double[]{ 1.0 / 95, 1.0 / 93 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 120, 110, 111, 111, 110, 111, 110, 120 }, new double[]{ 1.0 / 111, 1.0 / 110 })
            //};

            //_trainset = new List<Tuple<double[], double[]>>
            //{
            //    Tuple.Create<double[], double[]>(new double[]{ 1, 1, 0, 0, 0, 0, 1, 0 }, new double[]{ 0, 0 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 1, 1, 0, 0, 1, 0, 1, 0 }, new double[]{ 1, 0 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 1, 0, 1, 0, 1, 0, 1, 0 }, new double[]{ 0, 1 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 1, 0, 0, 1, 1, 0, 0, 0 }, new double[]{ 1, 0 })
            //};

            //_trainset = new List<Tuple<double[], double[]>>
            //{
            //    Tuple.Create<double[], double[]>(new double[]{ 1, 1, 0, 0, 0, 0 }, new double[]{ 0, 0 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 1, 1, 0, 0, 1, 0 }, new double[]{ 1, 0 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 1, 0, 1, 0, 1, 0 }, new double[]{ 1, 0 }),
            //    Tuple.Create<double[], double[]>(new double[]{ 0, 0, 0, 1, 0, 0 }, new double[]{ 0, 0 })
            //};

            _trainset = new List<Tuple<double[], double[]>>
            {
                Tuple.Create<double[], double[]>(new double[]{ 1, 1, 1, 1 }, new double[]{ 1, 1 }),
                Tuple.Create<double[], double[]>(new double[]{ 1, 0, 1, 0 }, new double[]{ 1, 0 }),
                Tuple.Create<double[], double[]>(new double[]{ 0, 0, 0, 0 }, new double[]{ 0, 0 }),
                Tuple.Create<double[], double[]>(new double[]{ 0, 0, 1, 0 }, new double[]{ 0, 0 })
            };
        }

        //массив для хранения выхода сети
        public double[] fact = new double[2];
        //ошибка одной итерации обучения
        private double GetIterationError(double[] errors)
        {
            double sum = 0;
            for (int i = 0; i < errors.Length; ++i)
            {
                sum += Math.Pow(errors[i], 2);
            }
            return 0.5d * sum;
        }
        //ошибка эпохи
        private double GetEraError(double[] iterationsError)
        {
            double sum = 0;
            for (int i = 0; i < iterationsError.Length; ++i)
            {
                sum += iterationsError[i];
            }
            return (sum / iterationsError.Length);
        }

        public static void Train(NeuralNetwork network)//backpropagation method
        {
            const double THRESHOLD = 0.001d; //порог ошибки
            //const double THRESHOLD = 0.001d;
            double[] iterationsError = new double[_trainset.Count]; //массив для хранения ошибок итераций
            //double[] iterationsError = new double[_trainset.Count];
            double eraError = 0; //текущее значение ошибки по эпохе
            //double errorsOfTheEra = 0;
            do
            {
                for (int i = 0; i < _trainset.Count; ++i)
                {
                    //прямой проход
                    network.hiddenLayer.Data = _trainset[i].Item1;
                    network.hiddenLayer.Recognize(null, network.outputLayer);
                    network.outputLayer.Recognize(network, null);
                    //вычисление ошибки по итерации
                    double[] errors = new double[_trainset[i].Item2.Length];
                    for (int x = 0; x < errors.Length; ++x)
                    {
                        errors[x] = _trainset[i].Item2[x] - network.fact[x];
                    }
                    iterationsError[i] = network.GetIterationError(errors);
                    //обратный проход и коррекция весов
                    double[] gsums = network.outputLayer.BackwardPass(errors);
                    network.hiddenLayer.BackwardPass(gsums);
                }
                eraError = network.GetEraError(iterationsError);//вычисление ошибки по эпохе
                //debugging
                Console.WriteLine(eraError.ToString("f90"));
            } while (eraError > THRESHOLD);
            //загрузка скорректированных весов в "память"
            network.hiddenLayer.WeightInitialize(MemoryMode.SET);
            network.outputLayer.WeightInitialize(MemoryMode.SET);
        }

        public static void Test(NeuralNetwork network)
        {
            for (int i = 0; i < _trainset.Count; ++i)
            {
                network.hiddenLayer.Data = _trainset[i].Item1;
                network.hiddenLayer.Recognize(null, network.outputLayer);
                network.outputLayer.Recognize(network, null);
                for (int j = 0; j < network.fact.Length; ++j)
                {
                    Console.WriteLine(network.fact[j]);
                }
                Console.WriteLine();
            }
        }
    }
}
