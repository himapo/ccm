using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA.Model;
using MikuMikuDance.XNA.Accessory;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Content;

namespace HimaLib.Model
{
    public class DynamicModelMMDX : IModel
    {
        public string Name { get; set; }

        public MMDXModel Model { get; set; }

        List<MMDAccessory> accessoryModels = new List<MMDAccessory>();

        AffineTransform transform = new AffineTransform();

        MMDAccessoryLoader accessoryLoader = new MMDAccessoryLoader();

        MMDVACLoader vacLoader = new MMDVACLoader();

        public DynamicModelMMDX()
        {
        }

        public void Render(IModelRenderParameter param)
        {
            var renderer = ModelRendererFactoryMMDX.Instance.Create(param);
            renderer.Render(Model, transform);

            foreach (var accessory in accessoryModels)
            {
                renderer.Render(accessory);
            }
        }

        public void AddAttachment(string name)
        {
            var accessory = accessoryLoader.Load("Accessory/" + name);
            var vac = vacLoader.Load("Accessory/" + name + "-vac");
            Model.BindAccessory(accessory, vac);
            accessoryModels.Add(accessory);
        }

        public void RemoveAttachment(string name)
        {
            
        }
    }
}
