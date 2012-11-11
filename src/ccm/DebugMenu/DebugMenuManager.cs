using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class DebugMenuManager : MyGameComponent, IDebugMenuService
    {
        Dictionary<string, DebugMenuNode> nodeDic;

        Stack<DebugMenuNode> menuStack;

        DebugMenuNode RootNode { get; set; }

        public DebugMenuManager(Game game)
            : base(game)
        {
            nodeDic = new Dictionary<string, DebugMenuNode>();

            menuStack = new Stack<DebugMenuNode>();

            RootNode = new DebugMenuNodeInternal(game)
            {
                Label = "root",
                Selectable = false,
            };
            nodeDic[RootNode.Label] = RootNode;

            game.Services.AddService(typeof(IDebugMenuService), this);

            AddComponents();
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            AddChild(RootNode.Label, new DebugMenuNodeExecutable(Game)
            {
                Label = "menu level0",
                Selectable = true,
                ExecFunc = () => { }
            });
            AddChild(RootNode.Label, new DebugMenuNodeExecutable(Game)
            {
                Label = "executable",
                Selectable = true,
                ExecFunc = () => { }
            });
            AddChild(RootNode.Label, new DebugMenuNodeTunableInt(Game, 0)
            {
                Label = "tunable",
                Selectable = true
            });
            AddChild(RootNode.Label, new DebugMenuNodeInternal(Game)
            {
                Label = "internal",
                Selectable = true
            });
            AddChild("internal", new DebugMenuNodeExecutable(Game)
            {
                Label = "menu level1",
                Selectable = true,
                ExecFunc = () => { }
            });
                
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {

        }

        public override void OnSceneEnd(SceneLabel sceneLabel)
        {

        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            var input = InputManager.GetInstance();

            if (input.IsPush(InputLabel.OpenDebugMenu))
            {
                Open();
                input.Mode = InputMode.DebugMenu;
                return;
            }
            else if (input.IsPush(InputLabel.CloseDebugMenu))
            {
                Close();
                input.Mode = InputMode.Game;
                return;
            }
            else if (input.IsPush(InputLabel.DebugMenuCancel))
            {
                Back();
                if (!IsOpen())
                {
                    input.Mode = InputMode.Game;
                }
            }

            if (!IsOpen())
                return;

            if (input.IsPush(InputLabel.DebugMenuOK))
            {
                GetSelectedChild().OnPushOK();
            }
            if (input.IsPush(InputLabel.DebugMenuUp))
            {
                GetSelectedChild().OnPushUp();
            }
            if (input.IsPush(InputLabel.DebugMenuDown))
            {
                GetSelectedChild().OnPushDown();
            }
            if (input.IsPush(InputLabel.DebugMenuLeft))
            {
                GetSelectedChild().OnPushLeft();
            }
            if (input.IsPush(InputLabel.DebugMenuRight))
            {
                GetSelectedChild().OnPushRight();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsOpen())
                return;

            DrawMenu();
        }

        public bool AddChild(string parent, DebugMenuNode node)
        {
            if (!nodeDic.ContainsKey(parent))
                return false;

            var parentNode = nodeDic[parent] as DebugMenuNodeInternal;

            if (parentNode == null)
                return false;

            parentNode.AddChild(node);
            nodeDic[node.Label] = node;

            return true;
        }

        DebugMenuNode GetSelectedChild()
        {
            return menuStack.Peek().SelectedChild;
        }

        bool IsOpen()
        {
            return menuStack.Count > 0;
        }

        public void Open()
        {
            if (IsOpen())
            {
                return;
            }

            menuStack.Push(RootNode);
        }

        public void Close()
        {
            menuStack.Clear();
        }

        public void Advance(DebugMenuNode node)
        {
            menuStack.Push(node);
        }

        public void Back()
        {
            menuStack.Pop();
        }

        public void CursorUp()
        {
            menuStack.Peek().CursorUp();
        }

        public void CursorDown()
        {
            menuStack.Peek().CursorDown();
        }

        void DrawMenu()
        {
            menuStack.Peek().DrawMenu();
        }
    }
}
