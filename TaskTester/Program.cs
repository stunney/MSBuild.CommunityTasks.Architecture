using MSBuild.Community.Architecture.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string modelingProjectPath = @"sample.modelproj";
            //string sequenceDiagramPath = @"C:\Stephen.Tunney_Wat-STunney_5.3_XeroxStaples_5801\Architecture\Partners\Xerox.Staples\Xerox.Staples\CreditCardLogin.sequencediagram";
            string outputPath = @"out\images";

            ArchitectureDiagramsToImagesTask task = new ArchitectureDiagramsToImagesTask();
            task.ModelingProjectPath = modelingProjectPath;
            task.OutputPath = outputPath;
            task.Execute();
        }
    }
}
