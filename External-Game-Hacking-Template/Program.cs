using System;
using System.Linq;
using System.Reflection;

namespace External_Game_Hacking_Template
{
    class Program
    {
        private static readonly Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
        private static readonly string[] EmbeddedLibraries = ExecutingAssembly.GetManifestResourceNames().Where(x => x.EndsWith(".dll")).ToArray();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs e)
        {
            // ... If any hooks have been added, remove them here.
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Unhandled exception happened");
        }

        /// <summary>
        /// Instead of having the *.dll files in the directory, we can embed them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Get assembly name
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";

            // Get resource name
            var resourceName = EmbeddedLibraries.FirstOrDefault(x => x.EndsWith(assemblyName));
            if (resourceName == null) return null;

            // Load assembly from resource
            using var stream = ExecutingAssembly.GetManifestResourceStream(resourceName);
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return Assembly.Load(bytes);
        }
    }
}
