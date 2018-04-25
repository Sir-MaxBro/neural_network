using System;
using NeuralCompressed.Network;
using System.Collections.Generic;

namespace NeuralCompressed.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<Tuple<double[], double[]>> trainset = new List<Tuple<double[], double[]>>
            {
                Tuple.Create<double[], double[]>(new double[]{ 97, 98, 97, 98 }, new double[]{ 1.0 / 97, 1.0 / 98 }),
                Tuple.Create<double[], double[]>(new double[]{ 98, 98, 98, 98 }, new double[]{ 1.0 / 98, 1.0 / 98 }),
                Tuple.Create<double[], double[]>(new double[]{ 95, 101, 95, 101 }, new double[]{ 1.0 / 95, 1.0 / 101 }),
                Tuple.Create<double[], double[]>(new double[]{ 120, 121, 120, 121 }, new double[]{ 1.0 / 120, 1.0 / 121 })
            };

            NeuralNetwork net = NeuralNetwork.GetInstance(inputs: null);
            NetworkTrainer.Train(net, trainset);
            NetworkTrainer.Test(net, trainset);

            Console.ReadKey();
        }
    }
}
