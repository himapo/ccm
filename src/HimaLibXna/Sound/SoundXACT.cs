using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using HimaLib.Debug;

namespace HimaLib.Sound
{
    public class SoundXACT : ISound
    {
        public string SettingsFile { get; set; }

        public string WaveBankFile { get; set; }
        
        public string SoundBankFile { get; set; }

        AudioEngine AudioEngine;

        WaveBank WaveBank;

        SoundBank SoundBank;

        Dictionary<string, Cue> StreamCueDic = new Dictionary<string,Cue>();

        public SoundXACT()
        {
        }

        public bool Initialize()
        {
            try
            {
                AudioEngine = new AudioEngine(SettingsFile);
            }
            catch (ArgumentException)
            {
                DebugPrint.PrintLine("AudioEngineの生成に失敗(XACTデータが不正)");
                return false;
            }

            try
            {
                WaveBank = new WaveBank(AudioEngine, WaveBankFile);
            }
            catch (Exception)
            {
                DebugPrint.PrintLine("WaveBankの生成に失敗");
                return false;
            }

            try
            {
                SoundBank = new SoundBank(AudioEngine, SoundBankFile);
            }
            catch (ArgumentException)
            {
                DebugPrint.PrintLine("SoundBankの生成に失敗(XACTデータが不正)");
                return false;
            }

            return true;
        }

        public void Update()
        {
            AudioEngine.Update();
        }

        public void PlaySoundStream(string name)
        {
            StopSoundStream(name);
            StreamCueDic[name] = SoundBank.GetCue(name);
            StreamCueDic[name].Play();
        }

        public void StopSoundStream(string name)
        {
            if (StreamCueDic.ContainsKey(name))
            {
                StreamCueDic[name].Stop(AudioStopOptions.AsAuthored);
            }
        }

        public void PlaySoundEffect(string name)
        {
            SoundBank.PlayCue(name);
        }
    }
}
