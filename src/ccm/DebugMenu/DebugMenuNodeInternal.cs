using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// 子供を持つ中間ノード
    /// </summary>
    class DebugMenuNodeInternal : DebugMenuNode
    {
        public int Selected { get; set; }

        List<DebugMenuNode> children;

        public override DebugMenuNode SelectedChild
        {
            get
            {
                return children[Selected];
            }
        }

        public DebugMenuNodeInternal(Game game)
            : base(game)
        {
            Selected = 0;
            children = new List<DebugMenuNode>();
        }

        public void AddChild(DebugMenuNode node)
        {
            children.Add(node);
        }

        bool HasChildren()
        {
            return children.Count > 0;
        }

        public override void OnPushOK()
        {
            GetService<IDebugMenuService>().Advance(this);
        }

        public override void CursorUp()
        {
            if (!HasChildren())
                return;

            do
            {
                if (--Selected < 0)
                {
                    Selected = children.Count - 1;
                }
            } while (!SelectedChild.Selectable);
        }

        public override void CursorDown()
        {
            if (!HasChildren())
                return;

            do
            {
                if (++Selected >= children.Count)
                {
                    Selected = 0;
                }
            } while (!SelectedChild.Selectable);
        }

        public override void DrawMenu()
        {
            for (var i = 0; i < children.Count; ++i )
            {
                var child = children[i];
                var info = new DebugFontInfo();
                info.Output = child.Label;
                info.Position = new Vector2(160.0f, 120.0f + 25.0f * i);
                info.FontColor = (i == Selected) ? Color.Red : Color.White;
                info.BGColor = (i == Selected) 
                    ? (new Color(1.0f, 1.0f, 0.0f, 0.3f)) 
                    : (new Color(0.0f, 0.0f, 0.0f, 0.3f));
                DebugFontManager.GetInstance().DrawString(info);
            }
        }
    }
}
