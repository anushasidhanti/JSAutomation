using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDataRef
{   
    public class J3DPackageDefs : CollectionBase
    {
        public J3DPackageDef this[int index]
        {
            get
            {
                return (J3DPackageDef)this.List[index];
            }
        }

        public J3DPackageDef this[string item]
        {
            get
            {
                return this.List.Cast<J3DPackageDef>().Where<J3DPackageDef>((Func<J3DPackageDef, bool>)(x =>
                {
                    if (!(x.Name == item))
                        return x.oid == item;
                    return true;
                })).ToList<J3DPackageDef>()[0];
            }
        }

        public int Add(J3DPackageDef packageDef)
        {
            int num = -1;
            if (packageDef != null && !this.List.Contains((object)packageDef))
                num = this.List.Add((object)packageDef);
            return num;
        }
    }
}
