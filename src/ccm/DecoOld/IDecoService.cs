using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    public enum DecoLabel
    {
        Prototype,
    }

    public class DecoInfo
    {
        public DecoLabel Type { get; set; }
        public Vector3 Position { get; set; }

        // これらはマネージャが設定する
        public string ScriptClass { get; set; }
    }

    public interface IDecoService
    {
        int InstanceNum { get; }

        int AliveNum { get; }

        int DeadNum { get; }

        void Add(DecoInfo info);

        void Remove(int ID);

        void NotifyFinishParticle(int decoID, int particleID);
    }
}
