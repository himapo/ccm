using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Sound;

namespace ccm.Sound
{
    public class SoundManager
    {
        static readonly SoundManager instance = new SoundManager();

        public static SoundManager Instance { get { return instance; } private set { } }

        ISound Sound;

        SoundManager()
        {
            Sound = new SoundXACT()
            {
                SettingsFile = @"Content\Sound\ccm.xgs",
                EffectWaveBankFile = @"Content\Sound\SE Bank.xwb",
                StreamWaveBankFile = @"Content\Sound\BGM Bank.xwb",
                SoundBankFile = @"Content\Sound\Sound Bank.xsb",
            };
        }

        public bool Initialize()
        {
            return Sound.Initialize();
        }

        public void Update()
        {
            Sound.Update();
        }

        public void PlaySoundEffect(string name)
        {
            Sound.PlaySoundEffect(name);
        }

        public void PlaySoundStream(string name)
        {
            Sound.PlaySoundStream(name);
        }

        public void StopSoundStream(string name)
        {
            Sound.StopSoundStream(name);
        }
    }
}
