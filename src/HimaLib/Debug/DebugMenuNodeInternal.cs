using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeInternal : DebugMenuNode
    {
        public override bool HasChildren
        {
            get
            {
                return children.Count > 0;
            }
        }

        public override DebugMenuNode SelectedChild
        {
            get
            {
                return children[selectedChildIndex];
            }
        }

        List<DebugMenuNode> children;

        int selectedChildIndex;

        public DebugMenuNodeInternal()
        {
            children = new List<DebugMenuNode>();
            selectedChildIndex = 0;
        }

        public override void AddChild(DebugMenuNode node)
        {
            children.Add(node);
        }

        public override void SelectPrevChild()
        {
            if (!HasChildren)
                return;

            do
            {
                if (--selectedChildIndex < 0)
                {
                    selectedChildIndex = children.Count - 1;
                }
            } while (!SelectedChild.Selectable);
        }

        public override void SelectNextChild()
        {
            if (!HasChildren)
                return;

            do
            {
                if (++selectedChildIndex >= children.Count)
                {
                    selectedChildIndex = 0;
                }
            } while (!SelectedChild.Selectable);
        }

        public override void DrawMenu(IDebugMenuDrawer drawer)
        {
            drawer.Draw(Label, children, selectedChildIndex);
        }
    }
}
