﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RS485.Common.Model
{
    /// <summary>
    /// type of command
    /// </summary>
    public enum CommandMode
    {
        [Description("Rozkaz 1")]
        One = 1,
        [Description("Rozkaz 2")]
        Two = 2
        
    }
}
