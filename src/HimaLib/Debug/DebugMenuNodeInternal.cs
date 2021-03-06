﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeInternal : DebugMenuNodeExecutable
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
                if (HasChildren)
                {
                    return children[selectedChildIndex];
                }
                else
                {
                    return new DebugMenuNode()
                    {
                        Label = "ノードがありません",
                    };
                }
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

        public override void ClearChildren()
        {
            children.Clear();
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
