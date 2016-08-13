using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class API
    {
        protected static T SafelyMakeAPICall<T>(Func<T> method)
        {
            try
            {
                var response = method();
                return response;
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}
