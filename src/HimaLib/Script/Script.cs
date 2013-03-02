using System;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using HimaLib.Debug;

namespace HimaLib.Script
{
    public class Script
    {
        CSharpCodeProvider codeProvider;
        CompilerParameters compileParameters;
        Assembly assembly;

        public Script(List<string> referencedAssemblies)
        {
            codeProvider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
            compileParameters = new CompilerParameters();
            compileParameters.IncludeDebugInformation = true;
            compileParameters.GenerateInMemory = false;

            foreach (var a in referencedAssemblies)
            {
                compileParameters.ReferencedAssemblies.Add(a);
            }
        }

        public bool Load(string path)
        {
            var result = codeProvider.CompileAssemblyFromFile(compileParameters, path);
            if (result.Errors.Count > 0)
            {
                DebugPrint.PrintLine("Errors building {0} into {1}", path, result.PathToAssembly);
                foreach (CompilerError ce in result.Errors)
                {
                    DebugPrint.PrintLine("  {0}", ce.ToString());
                }
                if (result.Errors.HasErrors)
                {
                    return false;
                }
            }
            assembly = result.CompiledAssembly;
            return true;
        }

        public object Call(string className, string methodName, object[] args)
        {
            var t = assembly.GetType(className);
            var retval = t.InvokeMember(methodName, BindingFlags.InvokeMethod, null, null, args);
            return retval;
        }
    }
}
