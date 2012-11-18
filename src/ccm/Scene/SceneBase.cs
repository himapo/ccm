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
        public bool ChangeScene { get; protected set; }

        public SceneBase NextScene { get; protected set; }

        public string Name { get; protected set; }

        protected SceneBase()
        {
            ChangeScene = false;
            NextScene = null;
            Name = "SceneBase";
        }
    }
}
