using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    enum LightAttribute
    {
        StageMain,
    }

    interface ILightService
    {
        bool Add(DirectionalLight light);

        bool Remove(DirectionalLight light);

        DirectionalLight Get(LightAttribute attribute);

        //List<DirectionalLight> Get(LightAttribute attribute, int max);


    }
}
