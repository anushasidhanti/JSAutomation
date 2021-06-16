using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDataRef
{
    public class J3DClassDef
    {
        public string oid { get; set; }

        public string Name { get; set; }

        public J3DInterfaceDefs InterfaceDefs { get; set; }
    }
}
