using Swiss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Projects.Web
{
    public class BaseballScraper : Project
    {
        public BaseballScraper()
        {

        }

        public override void Execute()
        {
            /*var path = "http://www.baseball-reference.com/leagues/AL/2016-standard-batting.shtml";
            var page = WebScraper.Scrape(path);

            var tables = page.Tables;
            var target = tables[1];

            var rows = Factory.GenerateStemsFromGrid(target.Header, target.Grid);

            var players = rows.Select(pl => new Player(pl.RawValues))
                .OrderByDescending(pl => pl.HR)
                .ToList();

            using (Microsoft.CSharp.CSharpCodeProvider foo = new Microsoft.CSharp.CSharpCodeProvider())
            {
                var res = foo.CompileAssemblyFromSource(new System.CodeDom.Compiler.CompilerParameters()
                    {
                        GenerateInMemory = true
                    },
                    "public class FooClass { public string Execute() { return \"output!\";}}"
                );

                var type = res.CompiledAssembly.GetType("FooClass");
                var obj = Activator.CreateInstance(type);
                var output = type.GetMethod("Execute").Invoke(obj, new object[] { });
            }*/
        }
    }
}
