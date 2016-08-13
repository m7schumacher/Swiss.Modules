using System;
using System.Threading.Tasks;

namespace Swiss
{
    public class TaskUtility
    {
        public void LaunchTask(Action act)
        {
            Task.Factory.StartNew(act);
        }
    }
}
