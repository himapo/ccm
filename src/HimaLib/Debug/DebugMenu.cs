﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    /// <summary>
    /// 階層型リスト形式のデバッグメニュー
    /// </summary>
    public class DebugMenu : IDebugMenu
    {
        public bool IsOpen { get; private set; }

        Dictionary<string, DebugMenuNode> nodeDic;

        Stack<DebugMenuNode> menuStack;

        public DebugMenuNode RootNode { get; private set; }

        int Depth { get { return menuStack.Count; } }

        DebugMenuNode CurrentNode
        {
            get
            {
                if (IsOpen)
                {
                    return menuStack.Peek();
                }
                else
                {
                    return new DebugMenuNode();
                }
            }
        }

        public DebugMenu(string rootLabel)
        {
            IsOpen = false;

            nodeDic = new Dictionary<string, DebugMenuNode>();

            menuStack = new Stack<DebugMenuNode>();

            RootNode = new DebugMenuNodeInternal()
            {
                Label = rootLabel,
                Selectable = false,
            };

            nodeDic[RootNode.Label] = RootNode;

            menuStack.Push(RootNode);
        }

        public bool AddChild(string parentFullPath, DebugMenuNode node)
        {
            if (!nodeDic.ContainsKey(parentFullPath))
                return false;

            var childFullPath = parentFullPath + "." + node.Label;

            if (nodeDic.ContainsKey(childFullPath))
                return false;

            var parentNode = nodeDic[parentFullPath];
            parentNode.AddChild(node);
            nodeDic[childFullPath] = node;

            return true;
        }

        public bool ClearChildren(string parentFullPath)
        {
            if (!nodeDic.ContainsKey(parentFullPath))
                return false;

            nodeDic[parentFullPath].ClearChildren();

            // クエリで列挙しながらだとnodeDic.Remove()できないので
            // ToListで完成リストにしてしまう
            var children = nodeDic.Keys
                .Where(key => { return key.Contains(parentFullPath + "."); })
                .Select(key => key).ToList();

            foreach (var key in children)
            {
                nodeDic.Remove(key);
            }

            return true;
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void OnPushOK()
        {
            Advance();

            CurrentNode.OnPushOK();

            if (!CurrentNode.HasChildren)
            {
                Back();
            }
        }

        public void OnPushCancel()
        {
            if (Depth == 1)
            {
                Close();
            }
            else
            {
                Back();
            }
        }

        void Advance()
        {
            menuStack.Push(CurrentNode.SelectedChild);
        }

        void Back()
        {
            menuStack.Pop();
        }

        public void OnPushUp()
        {
            CurrentNode.SelectPrevChild();
        }

        public void OnPushDown()
        {
            CurrentNode.SelectNextChild();
        }

        public void OnPushLeft()
        {
            CurrentNode.SelectedChild.OnPushLeft();
        }

        public void OnPushRight()
        {
            CurrentNode.SelectedChild.OnPushRight();
        }

        public void Draw(IDebugMenuDrawer drawer)
        {
            CurrentNode.DrawMenu(drawer);
        }
    }
}
