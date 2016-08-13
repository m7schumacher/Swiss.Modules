using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.Machine
{
    public class NeuralNetwork
    {
        private BasicNetwork _network;

        public NeuralNetwork(BasicNetwork net)
        {
            _network = net;
        }

        public IMLData Compute(double[] dubs)
        {
            BasicMLData data = new BasicMLData(dubs);
            return _network.Compute(data);
        }
    }
}
