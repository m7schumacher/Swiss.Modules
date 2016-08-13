using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Util.Arrayutil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Old_Apps
{
    public class NeuralNet : Project
    {
        public override void Execute()
        {
            double[] scores = new double[] { 40.0, 42.5, 44.5, 46.2, 48.3 };

            NormalizeArray weightNorm = new NormalizeArray();
            weightNorm.NormalizedHigh = 1.0;
            weightNorm.NormalizedLow = -1.0;

            double[] normWeights = weightNorm.Process(scores);

            //double[][] input =
            //{
            //    ConvertToHashArray("how weather"),
            //    ConvertToHashArray("how weather"),
            //    ConvertToHashArray("how weather"),
            //    ConvertToHashArray("how weather"),
            //    ConvertToHashArray("how weather"),
            //    ConvertToHashArray("how weather"),
            //    ConvertToHashArray("how weather"),
            //    ConvertToHashArray("what weather"),
            //    ConvertToHashArray("what time"),
            //    //ConvertToHashArray("where am I"),
            //    //ConvertToHashArray("any fantasy news"),
            //    //ConvertToHashArray("what is the weather like"),
            //    ConvertToHashArray("what day"),
            //    //ConvertToHashArray("how far is home from here"),
            //    ConvertToHashArray("how windy"),
            //    //ConvertToHashArray("how is my fantasy team doing")
            //};

            string[] commands = new string[]
            {
                "how weather",
                "what weather like",
                "what time",
                "how windy",
                "how does weather look",
                "what day"
            };

            double[][] input =
            {
                new double[] { Math.Abs(commands[0].GetHashCode()) },
                new double[] { Math.Abs(commands[1].GetHashCode()) },
                new double[] { Math.Abs(commands[2].GetHashCode()) },
                new double[] { Math.Abs(commands[3].GetHashCode()) },
                new double[] { Math.Abs(commands[4].GetHashCode()) },
                new double[] { Math.Abs(commands[5].GetHashCode()) },
            };

            input = input.OrderByDescending(row => row.Length).ToArray();

            double[][] ideal =
            {
                new [] { 0.0 },
                new [] { 0.0 },
                new [] { 1.0 },
                new [] { 0.0 },
                new [] { 0.0 },
                new [] { 1.0 },
            };

            var trainingSet = new BasicMLDataSet(input, ideal);

            BasicNetwork network = CreateNetwork();

            var trainer = new ResilientPropagation(network, trainingSet);

            int epoch = 1;

            do
            {
                trainer.Iteration();
                epoch++;
                Console.WriteLine("Iteration:{0} - Error:{1}", epoch, trainer.Error);
            }
            while (trainer.Error > .001 && epoch < 10000);

            Console.WriteLine();
            Console.WriteLine();

            string userInput = string.Empty;

            while (!userInput.Equals("exit"))
            {
                userInput = Console.ReadLine();

                BasicMLData data = new BasicMLData(ConvertToHashArray(userInput));

                var output = network.Compute(data);
                Console.WriteLine("Input: {0} - Actual: {1}", userInput, output[0]);
            }

            Console.ReadLine();
        }

        private static double[] ConvertToHashArray(string str)
        {
            string[] splitter = str.Split(' ');
            double[] hashes = new double[splitter.Length];

            for (int i = 0; i < splitter.Length; i++)
            {
                string bit = splitter[i];
                hashes[i] = bit.GetHashCode();
            }

            return hashes;
        }

        private static BasicNetwork CreateNetwork()
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 2));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 2));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            return network;
        }
    }
}
