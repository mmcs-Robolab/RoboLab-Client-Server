using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    class DynamicCompiler
    {
        public static object Compile(String classCode, String mainClass, Object[] requiredAssemblies)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>
              {
                 { "CompilerVersion", "v4.0" }
              });

            CompilerParameters parameters = new CompilerParameters
            {
                GenerateExecutable = false,       // Create a dll
                GenerateInMemory = true,          // Create it in memory
                WarningLevel = 3,                 // Default warning level
                CompilerOptions = "/optimize",    // Optimize code
                TreatWarningsAsErrors = false     // Better be false to avoid break in warnings
            };
            
            parameters.ReferencedAssemblies.Add("system.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("RoboLab.dll");
            
            foreach (var extraAsm in requiredAssemblies)
                parameters.ReferencedAssemblies.Add(extraAsm as string);
            
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, classCode);
            
            if (results.Errors.Count != 0)
                return results.Errors;
            return results.CompiledAssembly.GetType(mainClass);
        }
    }
}
