using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    class LightBase
    {
        public bool Enabled { get; set; }

        public List<LightAttribute> Attributes { get; private set; }

        public LightBase()
        {
            Enabled = true;
            Attributes = new List<LightAttribute>();
        }

        public bool AddAttribute(LightAttribute attribute)
        {
            if (Attributes.Contains(attribute))
            {
                return false;
            }

            Attributes.Add(attribute);

            return true;
        }

        public bool RemoveAttribute(LightAttribute attribute)
        {
            return Attributes.Remove(attribute);
        }
    }
}
