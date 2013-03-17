using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    public enum ParticleLabel
    {
        Prototype,
    }

    public class ParticleInfo
    {
        public ParticleLabel Type { get; set; }

        // デコなどの基準座標
        public Vector3 BasePosition { get; set; }

        // 所属する親デコのID
        public int DecoID { get; set; }

        // これらはマネージャが設定する
        public string ScriptClass { get; set; }
    }

    public interface IParticleService
    {
        int InstanceNum { get; }

        int AliveNum { get; }

        int DeadNum { get; }

        void Add(ParticleInfo info);

        void Remove(int ID);
    }
}
