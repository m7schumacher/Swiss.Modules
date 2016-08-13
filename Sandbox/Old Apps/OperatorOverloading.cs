using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Old_Apps
{
    class OpBrick
    {
        public int X { get; set; }
        public int Y { get; set; }

        public OpBrick(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static OpBrick operator +(OpBrick first, OpBrick second)
        {
            return new OpBrick(first.X + second.X, first.Y + second.Y);
        }

        public static bool operator ==(OpBrick first, OpBrick second)
        {
            return first.X == second.Y;
        }

        public static bool operator !=(OpBrick first, OpBrick second)
        {
            return !(first == second);
        }
    }

    public class OperatorOverloading : Project
    {
        public override void Execute()
        {
            OpBrick first = new OpBrick(2, 1);
            OpBrick second = new OpBrick(1, 2);

            OpBrick sum = first + second;

            bool areEqual = first == second;
            bool areReferenceEqual = first.Equals(second);
        }
    }
}
