using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using ccm.Battle;

namespace ccm.Player
{
    public class Player
    {
        string ModelName = "petit_miku_mix2";

        public IModel Model { get; private set; }

        //AffineTransform transform = new AffineTransform();
        public AffineTransform Transform { get; private set; }

        public ComboCounter ComboCounter { get; private set; }

        public Player()
        {
            Transform = new AffineTransform();
            ComboCounter = new ComboCounter();
        }

        public void InitModel()
        {
            Model = ModelFactory.Instance.Create(ModelName);
            Model.ChangeMotion("stand", 0.01f);
        }

        public void AddAttachment(string attachmentName)
        {
            Model.AddAttachment(attachmentName);
        }

        public void RemoveAttackment(string attachmentName)
        {
            Model.RemoveAttachment(attachmentName);
        }

        public void Update(IPlayerUpdater updater)
        {
            updater.Update(this);
        }

        public void Draw(IPlayerDrawer drawer)
        {
            drawer.Draw(this);
        }
    }
}
