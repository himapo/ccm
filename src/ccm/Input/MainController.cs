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

        IMouse mouse;

        public MainController(IKeyboard keyboard, IMouse mouse)
        {
            this.keyboard = keyboard;
            this.mouse = mouse;
        }

        public void AddKeyboardKey(DigitalDeviceLabel virtualKey, KeyboardKeyLabel realKey)
        {
            AddDigitalDevice((int)virtualKey, new KeyboardKey(keyboard, realKey));
        }

        public void AddMouseButton(DigitalDeviceLabel virtualKey, MouseButtonLabel realKey)
        {
            AddDigitalDevice((int)virtualKey, new MouseButton(mouse, realKey));
        }

        public void AddMouseAxis(PointingDeviceLabel virtualKey)
        {
            AddPointingDevice((int)virtualKey, new MouseAxis(mouse));
        }
    }
}
