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

        public void AddKeyboardKey(BooleanDeviceLabel virtualKey, KeyboardKeyLabel realKey)
        {
            AddBooleanDevice((int)virtualKey, new KeyboardKey(keyboard, realKey));
        }

        public void AddMouseButton(BooleanDeviceLabel virtualKey, MouseButtonLabel realKey)
        {
            AddBooleanDevice((int)virtualKey, new MouseButton(mouse, realKey));
        }

        public void AddMouseAxis()
        {
            AddPointingDevice(0, new MouseAxis(mouse));
        }

        public void AddMouseWheel()
        {
            AddDigitalDevice(0, new MouseWheel(mouse));
        }
    }
}
