using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.System;

namespace ccm.Scene
{
    public class SceneBase : StateMachine
    {
        public bool IsChange { get; protected set; }

        public SceneBase NextScene { get; protected set; }

        public string Name { get; protected set; }

        protected SceneBase()
        {
            IsChange = false;
            NextScene = null;
            Name = "SceneBase";
        }

        protected void ChangeScene(SceneBase nextScene)
        {
            IsChange = true;
            NextScene = nextScene;
        }
    }
}
