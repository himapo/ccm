using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Input;

namespace ccm.Input
{
    /// <summary>
    /// ゲーム上のメイン仮想コントローラ
    /// キーボード、マウスに対応する
    /// </summary>
    public class MainController : Controller
    {
        IKeyboard keyboard;

        public MainController(IKeyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        public void AddKeyboardKey(VirtualKeyLabel virtualKey, KeyboardKeyLabel realKey)
        {
            AddDigitalDevice((int)virtualKey, new KeyboardKey(keyboard, realKey));
        }
    }
}
