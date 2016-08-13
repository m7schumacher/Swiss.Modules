using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Old_Apps
{
    public class Bar
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Text { get; set; }

        public Bar(int x, int y)
        {
            X = x;
            Y = y;

            Text = X.ToString() + "-" + Y.ToString();
        }
    }

    public class Brick : Bar
    {
        public int Placement { get; set; }

        public Brick(int place, int x, int y) : base(x, y)
        {
            Placement = place;
        }
    }

    public class DynamicCollections : Project
    {
       
        public dynamic GetValue(dynamic d, Func<dynamic, dynamic> func)
        {
            return func(d);
        }

        public dynamic GetDistinctFields(IEnumerable<dynamic> elements, Func<dynamic, dynamic> field)
        {
            return elements.GroupBy(field).Select(e => field(e.First()));
        }

        public override void Execute()
        {
            var strings = new string[] { "test", "yeah" };
            var numbers = new int[] { 1, 2, 3, 4 };
            var dates = new DateTime[] { DateTime.Now, DateTime.MinValue, DateTime.MaxValue };

            Bar br = new Bar(5, 5);
            Bar br1 = new Bar(5, 5);
            Bar br2 = new Bar(10, 5);
            Bar br3 = new Bar(15, 5);
            Bar br4 = new Bar(15, 5);

            Brick brick1 = new Brick(1, 2, 3);
            Brick brick2 = new Brick(2, 2, 3);

            Bar[] bars = new Bar[] { br, br1, br2, br3, br4 };
            Brick[] bricks = new Brick[] { brick1, brick2 };

            var result = GetValue(br, x => x.Y);

            var distinctBricks = GetDistinctFields(bricks, b => b.X);
            var res = GetDistinctFields(bars, r => r.Text);

            Console.WriteLine(result);


            Console.ReadLine();
        }
    }
}
