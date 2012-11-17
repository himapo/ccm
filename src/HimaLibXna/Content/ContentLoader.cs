using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Content
{
    public class ContentLoader<ContentType> : ContentUser
    {
        static Dictionary<string, ContentType> resourceDic = new Dictionary<string, ContentType>();

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
