using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.Deco
{
    public enum DecoType
    {
        DamageValue,
    }

    public class DecoManager
    {
        //static readonly DecoManager instance = new DecoManager();

        //public static DecoManager Instance { get { return instance; } private set { } }

        List<Deco> AliveList = new List<Deco>();

        List<Deco> DeleteList = new List<Deco>();

        public DecoManager()
        {
        }

        public void Update()
        {
            AliveList.ForEach((a) =>
            {
                if (!a.Update())
                {
                    DeleteList.Add(a);
                }
            });
            DeleteList.ForEach((a) => { AliveList.Remove(a); });
            DeleteList.Clear();
        }

        public void Draw()
        {
            AliveList.ForEach((a) => { a.Draw(); });
        }

        public void Add(Deco deco)
        {
            AliveList.Add(deco);
        }

        public void Remove(Deco deco)
        {
            // TODO : どっちがいい？
            AliveList.Remove(deco);
            //DeleteList.Add(deco);
        }
    }
}
