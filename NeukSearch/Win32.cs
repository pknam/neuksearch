﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NeukSearch
{
    class Win32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();



    }
}