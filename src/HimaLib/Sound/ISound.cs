using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Sound
{
    public interface ISound
    {
        bool Initialize();

        void Update();

        void PlaySoundEffect(string name);

        void PlaySoundStream(string name);

        void StopSoundStream(string name);
    }
}
