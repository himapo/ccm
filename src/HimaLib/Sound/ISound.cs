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

        void PlaySoundStream(string name);

        void StopSoundStream(string name);

        void PlaySoundEffect(string name);
    }
}
