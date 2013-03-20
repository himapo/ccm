using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.Ally
{
    public class AllyManager
    {
        public AllyCreator Creator { get; set; }

        List<Ally> AliveList = new List<Ally>();

        List<Ally> DeleteList = new List<Ally>();

        public AllyManager()
        {
        }

        public void Update()
        {
            foreach (var enemy in AliveList)
            {
                enemy.Update();
            }

            DeleteList.ForEach((enemy) =>
            {
                AliveList.Remove(enemy);
            });
            DeleteList.Clear();
        }

        public void Draw()
        {
            foreach (var enemy in AliveList)
            {
                enemy.Draw();
            }
        }

        public void CreateAlly(AllyType type, AffineTransform transform)
        {
            AliveList.Add(Creator.Create(type, transform));
        }

        public void DeleteAlly(Ally enemy)
        {
            DeleteList.Add(enemy);
        }
    }
}
