using System.IO;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;

namespace ccm
{
    class ScriptManager : MyGameComponent, IScriptService
    {
        Dictionary<string, Script> scriptDic;

        public ScriptManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IScriptService), this);

            scriptDic = new Dictionary<string, Script>();
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。

            base.Initialize();
        }

        public bool Load(string name)
        {
            var script = new Script();
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
