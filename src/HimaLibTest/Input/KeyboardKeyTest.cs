using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HimaLib.Input
{
    class KeyboardKeyTest
    {
        FakeKeyboard keyboard;

        KeyboardKey keyA;
        KeyboardKey keyLeftControl;

        [SetUp]
        protected void SetUp()
        {
            keyboard = new FakeKeyboard();
            keyA = new KeyboardKey(keyboard, KeyboardKeyLabel.A);
            keyLeftControl = new KeyboardKey(keyboard, KeyboardKeyLabel.LeftControl);
        }

        [TestCase]
        public void IsPush()
        {
            Assert.False(keyA.IsRelease());
            Assert.False(keyLeftControl.IsRelease());

            // LeftControlを押す
            keyboard.KeyDownState[KeyboardKeyLabel.LeftControl] = true;

            // Aを離す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = false;
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPush());
            Assert.True(keyLeftControl.IsPush());

            // Aを押す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = true;
            keyA.Update();
            keyLeftControl.Update();
            Assert.True(keyA.IsPush());
            Assert.False(keyLeftControl.IsPush());

            // Aを離す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = false;
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPush());
            Assert.False(keyLeftControl.IsPush());

            // LeftControlを離す
            keyboard.KeyDownState[KeyboardKeyLabel.LeftControl] = false;

            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPush());
            Assert.False(keyLeftControl.IsPush());

            // そのまま
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPush());
            Assert.False(keyLeftControl.IsPush());
        }

        [TestCase]
        public void IsPress()
        {
            Assert.False(keyA.IsRelease());
            Assert.False(keyLeftControl.IsRelease());

            // LeftControlを押す
            keyboard.KeyDownState[KeyboardKeyLabel.LeftControl] = true;

            // Aを離す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = false;
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPress());
            Assert.True(keyLeftControl.IsPress());

            // Aを押す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = true;
            keyA.Update();
            keyLeftControl.Update();
            Assert.True(keyA.IsPress());
            Assert.True(keyLeftControl.IsPress());

            // Aを離す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = false;
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPress());
            Assert.True(keyLeftControl.IsPress());

            // LeftControlを離す
            keyboard.KeyDownState[KeyboardKeyLabel.LeftControl] = false;

            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPress());
            Assert.False(keyLeftControl.IsPress());

            // そのまま
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsPress());
            Assert.False(keyLeftControl.IsPress());
        }

        [TestCase]
        public void IsRelease()
        {
            Assert.False(keyA.IsRelease());
            Assert.False(keyLeftControl.IsRelease());

            // LeftControlを押す
            keyboard.KeyDownState[KeyboardKeyLabel.LeftControl] = true;
            // Aを離す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = false;
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsRelease());
            Assert.False(keyLeftControl.IsRelease());

            // Aを押す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = true;
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsRelease());
            Assert.False(keyLeftControl.IsRelease());

            // Aを離す
            keyboard.KeyDownState[KeyboardKeyLabel.A] = false;
            keyA.Update();
            keyLeftControl.Update();
            Assert.True(keyA.IsRelease());
            Assert.False(keyLeftControl.IsRelease());

            // LeftControlを離す
            keyboard.KeyDownState[KeyboardKeyLabel.LeftControl] = false;
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsRelease());
            Assert.True(keyLeftControl.IsRelease());

            // そのまま
            keyA.Update();
            keyLeftControl.Update();
            Assert.False(keyA.IsRelease());
            Assert.False(keyLeftControl.IsRelease());
        }

    }
}
