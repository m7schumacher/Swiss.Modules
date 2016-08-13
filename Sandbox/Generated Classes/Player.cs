using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiss;

namespace Sandbox
{
	public class Player
	{
		public double Rk { get; set; }
		public string Name { get; set; }
		public double Age { get; set; }
		public string Tm { get; set; }
		public double G { get; set; }
		public double PA { get; set; }
		public double AB { get; set; }
		public double R { get; set; }
		public double H { get; set; }
		public double TwoB { get; set; }
		public double ThreeB { get; set; }
		public double HR { get; set; }
		public double RBI { get; set; }
		public double SB { get; set; }
		public double CS { get; set; }
		public double BB { get; set; }
		public double SO { get; set; }
		public double BA { get; set; }
		public double OBP { get; set; }
		public double SLG { get; set; }
		public double OPS { get; set; }
		public double TB { get; set; }
		public double GDP { get; set; }
		public double HBP { get; set; }
		public double SH { get; set; }
		public double SF { get; set; }
		public double IBB { get; set; }
		public double PosSummary { get; set; }

		public Player(object[] values)
		{
            Rk = !String.IsNullOrEmpty(values[0].ToString()) ? values[0].MakeOwnType() : default(double);
            Name = !String.IsNullOrEmpty(values[1].ToString()) ? values[1].MakeOwnType() : default(string);
            Age = !String.IsNullOrEmpty(values[2].ToString()) ? values[2].MakeOwnType() : default(double);
            Tm = !String.IsNullOrEmpty(values[3].ToString()) ? values[3].MakeOwnType() : default(string);
            G = !String.IsNullOrEmpty(values[4].ToString()) ? values[4].MakeOwnType() : default(double);
            PA = !String.IsNullOrEmpty(values[5].ToString()) ? values[5].MakeOwnType() : default(double);
            AB = !String.IsNullOrEmpty(values[6].ToString()) ? values[6].MakeOwnType() : default(double);
            R = !String.IsNullOrEmpty(values[7].ToString()) ? values[7].MakeOwnType() : default(double);
            H = !String.IsNullOrEmpty(values[8].ToString()) ? values[8].MakeOwnType() : default(double);
            TwoB = !String.IsNullOrEmpty(values[9].ToString()) ? values[9].MakeOwnType() : default(double);
            ThreeB = !String.IsNullOrEmpty(values[10].ToString()) ? values[10].MakeOwnType() : default(double);
            HR = !String.IsNullOrEmpty(values[11].ToString()) ? values[11].MakeOwnType() : default(double);
            RBI = !String.IsNullOrEmpty(values[12].ToString()) ? values[12].MakeOwnType() : default(double);
            SB = !String.IsNullOrEmpty(values[13].ToString()) ? values[13].MakeOwnType() : default(double);
            CS = !String.IsNullOrEmpty(values[14].ToString()) ? values[14].MakeOwnType() : default(double);
            BB = !String.IsNullOrEmpty(values[15].ToString()) ? values[15].MakeOwnType() : default(double);
            SO = !String.IsNullOrEmpty(values[16].ToString()) ? values[16].MakeOwnType() : default(double);
            BA = !String.IsNullOrEmpty(values[17].ToString()) ? values[17].MakeOwnType() : default(double);
            OBP = !String.IsNullOrEmpty(values[18].ToString()) ? values[18].MakeOwnType() : default(double);
            SLG = !String.IsNullOrEmpty(values[19].ToString()) ? values[19].MakeOwnType() : default(double);
            OPS = !String.IsNullOrEmpty(values[20].ToString()) ? values[20].MakeOwnType() : default(double);
            TB = !String.IsNullOrEmpty(values[21].ToString()) ? values[21].MakeOwnType() : default(double);
            GDP = !String.IsNullOrEmpty(values[22].ToString()) ? values[22].MakeOwnType() : default(double);
            HBP = !String.IsNullOrEmpty(values[23].ToString()) ? values[23].MakeOwnType() : default(double);
            SH = !String.IsNullOrEmpty(values[24].ToString()) ? values[24].MakeOwnType() : default(double);
            SF = !String.IsNullOrEmpty(values[25].ToString()) ? values[25].MakeOwnType() : default(double);
            IBB = !String.IsNullOrEmpty(values[26].ToString()) ? values[26].MakeOwnType() : default(double);
            PosSummary = !String.IsNullOrEmpty(values[27].ToString()) ? values[27].MakeOwnType() : default(double);
		}
	}
}

