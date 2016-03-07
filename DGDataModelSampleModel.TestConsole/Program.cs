using System;

namespace DG.DataModelSample.Model.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            bool runSolo = false;
            Type testclass = typeof(DGDataModelTest);
            if (runSolo)
                NUnit.ConsoleRunner.Runner.Main(new string[] {
                    "/include:RunSolo", testclass.Assembly.Location, "/basepath="+"../../../" + testclass.Assembly.ManifestModule.Name.Substring(0, testclass.Assembly.ManifestModule.Name.Length - 4) + "/bin/Debug"
                });
            else
                NUnit.ConsoleRunner.Runner.Main(new string[] {
                    testclass.Assembly.Location, "/basepath="+"../../../" + testclass.Assembly.ManifestModule.Name.Substring(0, testclass.Assembly.ManifestModule.Name.Length - 4) + "/bin/Debug"
                });
            Console.ReadKey();
        }
    }
}
