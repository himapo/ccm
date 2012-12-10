using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.Player
{
    public class Player
    {
        string ModelName = "petit_miku_mix2";

        List<string> AttachmentNames = new List<string>();

        AffineTransform Transform = new AffineTransform(Vector3.One, Vector3.Zero, Vector3.Zero);

        public Player()
        {
        }

        public void AddAttachment(string attachmentName)
        {
            AttachmentNames.Add(attachmentName);
        }

        public void RemoveAttackment(string attachmentName)
        {
            AttachmentNames.Remove(attachmentName);
        }

        public void Draw(IPlayerDrawer drawer)
        {
            drawer.Draw(ModelName, AttachmentNames, Transform);
        }
    }
}
