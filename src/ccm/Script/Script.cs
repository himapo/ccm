using System;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace ccm
{
    class Script
    {
        CSharpCodeProvider codeProvider;
        CompilerParameters compileParameters;
        Assembly assembly;

        public Script()
        {
            codeProvider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
            compileParameters = new CompilerParameters();
            compileParameters.IncludeDebugInformation = true;
            compileParameters.GenerateInMemory = false;
            compileParameters.ReferencedAssemblies.Add(@"ccm.exe");
            compileParameters.ReferencedAssemblies.Add(@"System.dll");
            compileParameters.ReferencedAssemblies.Add(@"System.Core.dll");
            compileParameters.ReferencedAssemblies.Add(@"System.Xml.Linq.dll");
            compileParameters.ReferencedAssemblies.Add(@"Microsoft.Xna.Framework.dll");
            compileParameters.ReferencedAssemblies.Add(@"Microsoft.Xna.Framework.Game.dll");
            compileParameters.ReferencedAssemblies.Add(@"Microsoft.Xna.Framework.Graphics.dll");

            assembly = null;
        }

        public bool Load(string path)
        {
            var result = codeProvider.CompileAssemblyFromFile(compileParameters, path);
            if (result.Errors.Count > 0)
            {
                Console.WriteLine("Errors building {0} into {1}", path, result.PathToAssembly);
                foreach (CompilerError ce in result.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
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
