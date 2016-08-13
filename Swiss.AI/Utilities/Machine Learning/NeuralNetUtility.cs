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
using Swiss;

namespace Swiss.Machine
{
    public class NeuralNetUtility
    {
        public static double[][] NormalizeData(double[][] training, double high = 1.0, double low = -1.0)
        {
            NormalizeArray weightNorm = new NormalizeArray();
            weightNorm.NormalizedHigh = high;
            weightNorm.NormalizedLow = low;

            double[][] flipped = new double[training.Width()][];

            for (int i = 0; i < flipped.Length; i++)
            {
                flipped[i] = weightNorm.Process(training.GetColumn(i));
            }

            return flipped.Invert();
        }

        public static NeuralNetwork GenerateTrainedNeuralNet(double[][] normalizedTrainingData, double[][] ideal, double error = .01, double epochs = 10000)
        {
            var trainingSet = new BasicMLDataSet(normalizedTrainingData, ideal);

            BasicNetwork network = CreateNetwork(normalizedTrainingData.Width(), ideal.Width());

            var trainer = new ResilientPropagation(network, trainingSet);

            int epoch = 1;

            do
            {
                trainer.Iteration();
                epoch++;
            }
            while (trainer.Error > error && epoch < epochs);

            return new NeuralNetwork(network);
        }

        private static BasicNetwork CreateNetwork(int inputs, int outputs)
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, inputs));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, inputs));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, outputs));
            network.Structure.FinalizeStructure();
            network.Reset();

            return network;
        }
    }
}
