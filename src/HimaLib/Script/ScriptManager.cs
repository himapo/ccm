using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace HimaLib.Script
{
    public class ScriptManager
    {
        static readonly ScriptManager instance = new ScriptManager();

        public static ScriptManager Instance { get { return instance; } private set { } }

        Dictionary<string, Script> scriptDic = new Dictionary<string,Script>();

        List<string> ReferencedAssemblies = new List<string>();

        ScriptManager()
        {
        }

        public void AddReferencedAssembliy(string name)
        {
            ReferencedAssemblies.Add(name);
        }

        public void AddScriptName(string name)
        {
            scriptDic[name] = null;
        }

        public void LoadAll()
        {
            foreach (var name in scriptDic.Keys)
            {
                Load(name);
            }
        }

        public bool Load(string name)
        {
            var script = new Script(ReferencedAssemblies);
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path) + @"\Script\Code\" + name;
            if (!script.Load(path))
            {
                return false;
            }
            scriptDic[name] = script;
            return true;
        }

        public Script Get(string name)
        {
            Script script = null;
            scriptDic.TryGetValue(name, out script);
            return script;
        }
    }
}
