using System;
using NeuralCompressed.Network;

namespace NeuralCompressed.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            NeuralNetwork net = new NeuralNetwork();
            NeuralNetwork.Train(net);
            NeuralNetwork.Test(net);
            Console.ReadKey();
        }
    }
}
