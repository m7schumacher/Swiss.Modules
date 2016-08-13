using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.Test
{
    public class Bar
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Text { get { return X.ToString() + "-" + Y.ToString(); } }

        private string Hidden;

        public Bar()
        {
            X = 0;
            Y = 0;

            Hidden = "I am hidden";
        }

        public Bar(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            Bar incoming = obj as Bar;

            return incoming.X == X && incoming.Y == Y;
        }
    }
}
