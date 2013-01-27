using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Player
{
    public class Player
    {
        string ModelName = "petit_miku_mix2";

        IModel Model;

        AffineTransform transform = new AffineTransform();
        public AffineTransform Transform { get { return transform; } }

        public Player()
        {
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
            updater.Update(Model, Transform);
        }

        public void Draw(IPlayerDrawer drawer)
        {
            drawer.Draw(Model, Transform);
        }
    }
}
