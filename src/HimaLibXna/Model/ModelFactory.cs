using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Content;

namespace HimaLib.Model
{
    public class ModelFactory
    {
        static readonly ModelFactory instance = new ModelFactory();

        public static ModelFactory Instance { get { return instance; } private set { } }

        ModelLoader modelLoaderXna = new ModelLoader();

        MMDXModelLoader modelLoaderMMDX = new MMDXModelLoader();

        ModelFactory()
        {
        }

        public IModel Create(string name)
        {
            // TODO : モデルの種類判定

            IModel result;

            if (name == "petit_miku_mix2")
            {
                result = new DynamicModelMMDX() { Name = name, Model = modelLoaderMMDX.Load("Model/" + name) };
            }
            else if (name == "petit_miku_mix2_fbx" || name == "dude")
            {
                result = new DynamicModelXna() { Name = name, Model = modelLoaderXna.Load("Model/" + name) };
            }
            else
            {
                result = new StaticModelXna() { Name = name, Model = modelLoaderXna.Load("Model/" + name) };
            }

            if (!result.Init())
            {
                return new NullModel() { Name = name };
            }

            return result;
        }
    }
}
