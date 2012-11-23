using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace HimaLib.Input
{
    /// <summary>
    /// XNAのデフォルトのキーボード
    /// </summary>
    public class DefaultKeyboard : IKeyboard
    {
        public bool IsKeyDown(KeyboardKeyLabel key)
        {
            return Keyboard.GetState().IsKeyDown(ConvertKey(key));
        }

        static Keys ConvertKey(KeyboardKeyLabel key)
        {
            switch (key)
            {
                case KeyboardKeyLabel.Back: return Keys.Back;
                case KeyboardKeyLabel.Tab: return Keys.Tab;
                case KeyboardKeyLabel.Enter: return Keys.Enter;
                case KeyboardKeyLabel.CapsLock: return Keys.CapsLock;
                case KeyboardKeyLabel.Escape: return Keys.Escape;
                case KeyboardKeyLabel.Space: return Keys.Space;

                case KeyboardKeyLabel.Left: return Keys.Left;
                case KeyboardKeyLabel.Up: return Keys.Up;
                case KeyboardKeyLabel.Right: return Keys.Right;
                case KeyboardKeyLabel.Down: return Keys.Down;

                case KeyboardKeyLabel.A: return Keys.A;
                case KeyboardKeyLabel.B: return Keys.B;
                case KeyboardKeyLabel.C: return Keys.C;
                case KeyboardKeyLabel.D: return Keys.D;
                case KeyboardKeyLabel.E: return Keys.E;
                case KeyboardKeyLabel.F: return Keys.F;
                case KeyboardKeyLabel.G: return Keys.G;
                case KeyboardKeyLabel.H: return Keys.H;
                case KeyboardKeyLabel.I: return Keys.I;
                case KeyboardKeyLabel.J: return Keys.J;
                case KeyboardKeyLabel.K: return Keys.K;
                case KeyboardKeyLabel.L: return Keys.L;
                case KeyboardKeyLabel.M: return Keys.M;
                case KeyboardKeyLabel.N: return Keys.N;
                case KeyboardKeyLabel.O: return Keys.O;
                case KeyboardKeyLabel.P: return Keys.P;
                case KeyboardKeyLabel.Q: return Keys.Q;
                case KeyboardKeyLabel.R: return Keys.R;
                case KeyboardKeyLabel.S: return Keys.S;
                case KeyboardKeyLabel.T: return Keys.T;
                case KeyboardKeyLabel.U: return Keys.U;
                case KeyboardKeyLabel.V: return Keys.V;
                case KeyboardKeyLabel.W: return Keys.W;
                case KeyboardKeyLabel.X: return Keys.X;
                case KeyboardKeyLabel.Y: return Keys.Y;
                case KeyboardKeyLabel.Z: return Keys.Z;

                case KeyboardKeyLabel.F1: return Keys.F1;
                case KeyboardKeyLabel.F2: return Keys.F2;
                case KeyboardKeyLabel.F3: return Keys.F3;
                case KeyboardKeyLabel.F4: return Keys.F4;
                case KeyboardKeyLabel.F5: return Keys.F5;
                case KeyboardKeyLabel.F6: return Keys.F6;
                case KeyboardKeyLabel.F7: return Keys.F7;
                case KeyboardKeyLabel.F8: return Keys.F8;
                case KeyboardKeyLabel.F9: return Keys.F9;
                case KeyboardKeyLabel.F10: return Keys.F10;
                case KeyboardKeyLabel.F11: return Keys.F11;
                case KeyboardKeyLabel.F12: return Keys.F12;

                case KeyboardKeyLabel.LeftShift: return Keys.LeftShift;
                case KeyboardKeyLabel.RightShift: return Keys.RightShift;
                case KeyboardKeyLabel.LeftControl: return Keys.LeftControl;
                case KeyboardKeyLabel.RightControl: return Keys.RightControl;
                case KeyboardKeyLabel.LeftAlt: return Keys.LeftAlt;
                case KeyboardKeyLabel.RightAlt: return Keys.RightAlt;

                default: break;
            }

            return Keys.None;
        }
    }
}
