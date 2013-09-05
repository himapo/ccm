using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using HimaLib.System;

namespace HimaLib.Content
{
    public class ContentLoader<ContentType>
    {
        ContentManager Content { get { return XnaGame.Instance.Content; } }

        protected static Dictionary<string, ContentType> resourceDic = new Dictionary<string, ContentType>();

        public ContentLoader()
        {
        }

        public ContentType Load(string name)
        {
            if (!resourceDic.ContainsKey(name))
            {
                resourceDic[name] = Content.Load<ContentType>(name);
            }
            return resourceDic[name];
        }

        public void RequestLoad(string name)
        {
        }

        public void RequestRelease(string name)
        {
        }
    }
}
