using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Texture;
using ccm.Input;
using ccm.Util;
using ccm.Render;

namespace ccm.Item
{
    enum ItemIconState
    {
        Default,    // 普通にウィンドウに置かれている
        Drag,       // ドラッグされている
        Drop,       // ドロップされそうになってる
    }

    class ItemIconInfo
    {
        public int Type { get; set; }
        public ItemIconState State { get; set; }
    }

    public class ItemWindow : StateMachine
    {
        Vector3 Position = Vector3.Zero;

        List<ItemIconInfo> IconInfoList = new List<ItemIconInfo>();

        bool PushMouseMain { get { return InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.MouseMain); } }
        bool PressMouseMain { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.MouseMain); } }
        bool PushMouseSub { get { return InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.MouseSub); } }
        bool PushItemWindow { get { return InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.ItemWindow); } }
        int CursorX { get { return InputAccessor.GetX(ControllerLabel.Main, PointingDeviceLabel.Mouse0); } }
        int CursorY { get { return InputAccessor.GetY(ControllerLabel.Main, PointingDeviceLabel.Mouse0); } }
        int CursorMoveX { get { return InputAccessor.GetMoveX(ControllerLabel.Main, PointingDeviceLabel.Mouse0); } }
        int CursorMoveY { get { return InputAccessor.GetMoveY(ControllerLabel.Main, PointingDeviceLabel.Mouse0); } }

        bool Drag = false;

        //float Rot;

        IBillboard Billboard = BillboardFactory.Instance.Create();

        HudBillboardRenderParameter BillboardRenderParam = new HudBillboardRenderParameter();

        public ItemWindow()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;
        }

        void UpdateStateInit()
        {
            InitItem();

            UpdateState = UpdateStateClosed;
            DrawState = DrawStateClosed;
        }

        void InitItem()
        {
            IconInfoList.Add(new ItemIconInfo { Type = 0, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 1, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 2, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 0, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 1, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 2, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 0, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 1, State = ItemIconState.Default });
            IconInfoList.Add(new ItemIconInfo { Type = 2, State = ItemIconState.Default });
        }

        void UpdateStateClosed()
        {
            if (PushItemWindow)
            {
                UpdateState = UpdateStateOpening;
                DrawState = DrawStateOpening;
            }
        }

        void UpdateStateOpening()
        {
            UpdateState = UpdateStateOpen;
            DrawState = DrawStateOpen;
        }

        void UpdateStateOpen()
        {
            // ドラッグ処理
            if (PushMouseMain)
            {
                Drag = false;

                // ドラッグ領域のウィンドウ中心からの相対位置
                const float DRAG_RECT_TOP = 128.0f;
                const float DRAG_RECT_BOTTOM = DRAG_RECT_TOP - 52.0f;
                const float DRAG_RECT_LEFT = -128.0f;
                const float DRAG_RECT_RIGHT = 128.0f;

                var x = (float)CursorX;
                var y = (float)CursorY;

                if (x > Position.X + DRAG_RECT_LEFT
                    && x < Position.X + DRAG_RECT_RIGHT
                    && y < Position.Y + DRAG_RECT_TOP
                    && y > Position.Y + DRAG_RECT_BOTTOM)
                {
                    Drag = true;
                }
            }
            else if (Drag && PressMouseMain)
            {
                Position.X += CursorMoveX;
                Position.Y += CursorMoveY;
            }

            // アイコンドラッグ処理
            DragIcon();

            // 閉じる
            if (PushItemWindow)
            {
                UpdateState = UpdateStateClosing;
                DrawState = DrawStateClosing;
            }
        }

        void DragIcon()
        {
            var mouseOnIcon = -1;   // マウスが乗ってるアイコン番号

            for (var i = 0; i < IconInfoList.Count; ++i)
            {
                var iconInfo = IconInfoList[i];

                var x = (float)CursorX;
                var y = (float)CursorY;

                var iconPos = new Vector2();
                iconPos.X = Position.X + 51.0f * (i % 5 - 2);
                iconPos.Y = Position.Y - 51.0f * (i / 5 - 1);

                const float ICON_WIDTH_HALF = 25.0f;

                if (x > iconPos.X - ICON_WIDTH_HALF
                    && x < iconPos.X + ICON_WIDTH_HALF
                    && y < iconPos.Y + ICON_WIDTH_HALF
                    && y > iconPos.Y - ICON_WIDTH_HALF)
                {
                    mouseOnIcon = i;
                }
                else if (iconInfo.State == ItemIconState.Drop)
                {
                    iconInfo.State = ItemIconState.Default;
                }
            }

            // ドラッグされているものがあればそれを処理
            for (var i = 0; i < IconInfoList.Count; ++i)
            {
                if (i == mouseOnIcon)
                    continue;

                var iconInfo = IconInfoList[i];
                if (iconInfo.State == ItemIconState.Drag)
                {
                    if (mouseOnIcon == -1)
                    {
                        if (PushMouseSub)
                        {
                            // 移動キャンセル
                            iconInfo.State = ItemIconState.Default;
                        }
                    }
                    else
                    {
                        if (PushMouseMain)
                        {
                            // 入れ替え
                            iconInfo.State = ItemIconState.Default;
                            IconInfoList[mouseOnIcon].State = ItemIconState.Default;
                            GeneralUtil.Swap<ItemIconInfo>(IconInfoList, i, mouseOnIcon);
                        }
                        else if (PushMouseSub)
                        {
                            // 移動キャンセル
                            iconInfo.State = ItemIconState.Default;
                            IconInfoList[mouseOnIcon].State = ItemIconState.Default;
                        }
                        else
                        {
                            // ドラッグアイコンが上に乗ってたらドロップ状態にする
                            IconInfoList[mouseOnIcon].State = ItemIconState.Drop;
                        }
                    }

                    return;
                }
            }

            // ドラッグされているものがなければ新たにドラッグされたか判定
            if (PushMouseMain && mouseOnIcon != -1)
            {
                IconInfoList[mouseOnIcon].State = ItemIconState.Drag;
            }
        }

        void UpdateStateClosing()
        {
            UpdateState = UpdateStateClosed;
            DrawState = DrawStateClosed;
        }

        void DrawStateInit()
        {
        }

        void DrawStateClosed()
        {
        }

        void DrawStateOpening()
        {
        }

        void DrawStateOpen()
        {
            DrawWindow();
            DrawIcons();
        }

        void DrawWindow()
        {
            BillboardRenderParam.Texture = TextureFactory.Instance.CreateFromImage("Texture/ItemWindow000");
            BillboardRenderParam.Transform = new AffineTransform(
                Vector3.One,
                Vector3.Zero,
                Position).WorldMatrix;
            BillboardRenderParam.Alpha = 1.0f;

            RenderSceneManager.Instance.RenderBillboard(Billboard, BillboardRenderParam);
        }

        void DrawIcons()
        {
            for (var i = 0; i < IconInfoList.Count; ++i)
            {
                var iconInfo = IconInfoList[i];

                var renderParam = new HudBillboardRenderParameter();

                renderParam.Texture = TextureFactory.Instance.CreateFromImage("Texture/ItemIcon000");

                var pos = new Vector3();

                if (iconInfo.State == ItemIconState.Default)
                {
                    pos.X = Position.X + 51.0f * (i % 5 - 2);
                    pos.Y = Position.Y - 51.0f * (i / 5 - 1);
                    pos.Z = 0.0f;
                }
                else if (iconInfo.State == ItemIconState.Drag)
                {
                    pos.X = CursorX;
                    pos.Y = CursorY;
                    pos.Z = -0.5f;
                }
                else if (iconInfo.State == ItemIconState.Drop)
                {
                    pos.X = Position.X + 51.0f * (i % 5 - 2) + 8.0f;
                    pos.Y = Position.Y - 51.0f * (i / 5 - 1) + 8.0f;
                    pos.Z = -0.1f;
                }

                renderParam.Transform = new AffineTransform(
                    Vector3.One,
                    Vector3.Zero,
                    pos).WorldMatrix;

                renderParam.RectOffset = new Vector2(
                    1.0f + 51.0f * (iconInfo.Type % 5),
                    1.0f + 51.0f * (iconInfo.Type / 5));
                renderParam.RectSize = new Vector2(50.0f, 50.0f);
                renderParam.Alpha = 0.5f;

                RenderSceneManager.Instance.RenderBillboard(Billboard, renderParam);
            }
        }

        void DrawStateClosing()
        {
        }
    }
}
