using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNode
    {
        public virtual string Label { get; set; }

        public bool Selectable { get; set; }

        public virtual bool HasChildren { get; protected set; }

        public virtual DebugMenuNode SelectedChild { get; protected set; }

        public DebugMenuNode()
        {
            Label = "";
            Selectable = true;
            HasChildren = false;
            SelectedChild = null;
        }

        public virtual void AddChild(DebugMenuNode node)
        {
        }

        public virtual void ClearChildren()
        {
        }

        public virtual void OnPushOK()
        {
        }

        public virtual void OnPushLeft()
        {
        }

        public virtual void OnPushRight()
        {
        }

        public virtual void SelectPrevChild()
        {
        }

        public virtual void SelectNextChild()
        {
        }

        public virtual void DrawMenu(IDebugMenuDrawer drawer)
        {
        }
    }
}
