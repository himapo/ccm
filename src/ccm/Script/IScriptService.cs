﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    interface IScriptService
    {
        bool Load(string name);

        Script Get(string name);
    }
}
