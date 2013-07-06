using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeSelectable : DebugMenuNodeTunable<int>
    {
        List<string> Labels = new List<string>();

        List<Action> Actions = new List<Action>();

        string SelectedLabel
        {
            get
            {
                return (Val < Labels.Count) ? Labels[Val] : "No Choice";
            }
            set
            {
            }
        }

        Action SelectedAction
        {
            get
            {
                return (Val < Actions.Count) ? Actions[Val] : new Action(() => { });
            }
            set
            {
            }
        }

        public DebugMenuNodeSelectable()
        {
        }

        public override void OnPushOK()
        {
            SelectedAction();
        }

        public override void OnPushLeft()
        {
            if (--Val < 0)
            {
                Val = Labels.Count;
            }
        }

        public override void OnPushRight()
        {
            if (++Val >= Labels.Count)
            {
                Val = 0;
            }
        }

        public void AddChoice(string label, Action action)
        {
            Labels.Add(label);
            Actions.Add(action);
        }

        protected override string GetValString()
        {
            return SelectedLabel;
        }
    }
}
