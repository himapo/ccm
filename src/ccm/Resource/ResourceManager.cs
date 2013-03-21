using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ccm
{
    public class ResourceManager
    {
        static ResourceManager instance;

        Microsoft.Xna.Framework.Game Game { get; set; }

        Dictionary<string, Object> resourceDic;

        public static void CreateInstance(Microsoft.Xna.Framework.Game game)
        {
            instance = new ResourceManager(game);
        }

        public static ResourceManager GetInstance()
        {
            return instance;
        }

        ResourceManager(Microsoft.Xna.Framework.Game game)
        {
            Game = game;
            resourceDic = new Dictionary<string, object>();
        }

        public ResourceType Load<ResourceType>(string name)
        {
            if (!resourceDic.ContainsKey(name))
            {
                resourceDic[name] = Game.Content.Load<ResourceType>(name);
            }
            return (ResourceType)resourceDic[name];
        }

        public void RequestLoad(string name)
        {
        }

        public void RequestRelease(string name)
        {
        }
    }
}
