using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Content
{
    public class ContentLoader : ContentUser
    {
        static Dictionary<string, Object> resourceDic = new Dictionary<string,object>();

        public ContentLoader()
        {
        }

        public ContentType Load<ContentType>(string name)
        {
            if (!resourceDic.ContainsKey(name))
            {
                resourceDic[name] = Content.Load<ContentType>(name);
            }
            return (ContentType)resourceDic[name];
        }

        public void RequestLoad(string name)
        {
        }

        public void RequestRelease(string name)
        {
        }
    }
}
