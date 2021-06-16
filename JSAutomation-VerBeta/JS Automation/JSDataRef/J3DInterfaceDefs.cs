using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDataRef
{
    public class J3DInterfaceDefs : CollectionBase
    {
        public J3DInterfaceDef this[int index]
        {
            get
            {
                return (J3DInterfaceDef)this.List[index];
            }
            set
            {
                this.List[index] = (object)value;
            }
        }

        public J3DInterfaceDef this[string item]
        {
            get
            {
                return this.List.Cast<J3DInterfaceDef>().Where<J3DInterfaceDef>((Func<J3DInterfaceDef, bool>)(x =>
                {
                    if (!(x.oid == item))
                        return x.Name == item;
                    return true;
                })).ToList<J3DInterfaceDef>()[0];
            }
        }

        public int IndexOf(J3DInterfaceDef interfaceDef)
        {
            int num = 0;
            foreach (J3DInterfaceDef x3DinterfaceDef in (IEnumerable)this.List)
            {
                if (x3DinterfaceDef.oid.Equals(interfaceDef.oid))
                    return num;
                ++num;
            }
            return -1;
        }

        public int Add(J3DInterfaceDef interfaceDef)
        {
            int num = -1;
            if (interfaceDef != null && !this.List.Contains((object)interfaceDef))
                num = this.List.Add((object)interfaceDef);
            return num;
        }
    }
}
