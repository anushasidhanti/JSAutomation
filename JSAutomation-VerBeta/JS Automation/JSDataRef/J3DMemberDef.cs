﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDataRef
{
    public class J3DMemberDef
    {
        public string oid { get; set; }

        public string Name { get; set; }

        public J3DPackageDefs PackageDefs { get; set; }
    }
}
