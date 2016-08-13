using Sandbox.Projects;
using Swiss;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Dynamic;
using Swiss.DB;
using System.Data.Entity;
using ForecastIO;
using Swiss.Web;
using Swiss.API;
using System.Threading;
using Swiss.Utilities.Applications;
using Swiss.Machine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace Sandbox
{
    class Program
    {
        public static void Main(string[] args)
        {
            SpeechRecognizer recog = new SpeechRecognizer();
            recog.Initialize();

            Console.WriteLine("Listening!");
            Console.ReadLine();
        }

        private static void ExecuteProject(Project proj)
        {
            proj.Execute();
        }
    }
}
