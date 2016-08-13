using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Projects
{
    public class DynamicMethodGeneration : Project
    {
        public override void Execute()
        {
            MethodInfo methodInfo = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) });
            MethodInfo stopperMethod = typeof(Console).GetMethod("ReadLine", new Type[] { });

            DynamicMethod method = new DynamicMethod("HelloWorld", typeof(void), new Type[] { });
            DynamicMethod stop = new DynamicMethod("StopExecution", typeof(void), new Type[] { });

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Hello, World");
            il.Emit(OpCodes.Call, methodInfo);
            il.Emit(OpCodes.Ret);

            Action action = (Action)method.CreateDelegate(typeof(Action));

            action();

            Console.ReadLine();
        }
    }
}
