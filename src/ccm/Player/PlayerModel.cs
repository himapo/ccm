using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ccm
{
    enum PlayerModelType
    {
        MMD,
        FBX,
    }

    class PlayerModelLoadContext
    {
    }

    class PlayerModelChangeMotionContext
    {
        public string MotionName { get; set; }

        public float ShiftTime { get; set; }
    }

    class PlayerModelUpdateContext
    {
        public GameTime GameTime { get; set; }
    }

    class PlayerModelDrawContext
    {
        public GraphicsDevice GraphicsDevice { get; set; }
    }

    class PlayerModel : IDisposable
    {
        public Matrix Transform { get; set; }

        bool disposed = false;

        public static PlayerModel CreateInstance(PlayerModelType type)
        {
            switch (type)
            {
                case PlayerModelType.MMD:
                    return new PlayerModelMMD();
                case PlayerModelType.FBX:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException();
            }
        }

        ~PlayerModel()
        {
            if (!disposed)
            {
                Dispose(false);
                disposed = true;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public virtual void Load(PlayerModelLoadContext contextBase) { }

        public virtual void Reset() { }

        public virtual void ChangeMotion(PlayerModelChangeMotionContext contextBase) { }

        public virtual void Update(PlayerModelUpdateContext contextBase) { }

        public virtual void Draw(PlayerModelDrawContext contextBase) { }
    }
}
