﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Player
{
    public class Player
    {
        string modelName = "petit_miku_mix2";

        IModel model;

        AffineTransform transform = new AffineTransform(Vector3.One, Vector3.Zero, Vector3.Zero);

        public Player()
        {
        }

        public void InitModel()
        {
            model = ModelFactory.Instance.Create(modelName);
        }

        public void AddAttachment(string attachmentName)
        {
            model.AddAttachment(attachmentName);
        }

        public void RemoveAttackment(string attachmentName)
        {
            model.RemoveAttachment(attachmentName);
        }

        public void Draw(IPlayerDrawer drawer)
        {
            drawer.Draw(model, transform);
        }
    }
}
