using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSDataRef
{
    public class J3DClassDefs : CollectionBase
    {
        public J3DClassDef this[int index]
        {
            get
            {
                return (J3DClassDef)this.List[index];
            }
            set
            {
                this.List[index] = (object)value;
            }
        }

        public J3DClassDef this[string item]
        {
            get
            {
                return this.List.Cast<J3DClassDef>().Where<J3DClassDef>((Func<J3DClassDef, bool>)(J =>
                {
                    if (!(J.oid == item))
                        return J.Name == item;
                    return true;
                })).ToList<J3DClassDef>()[0];
            }
        }

        public int IndexOf(J3DClassDef classDef)
        {
            int num = 0;
            foreach (J3DClassDef J3DclassDef in (IEnumerable)this.List)
            {
                if (J3DclassDef.oid.Equals(classDef.oid))
                    return num;
                ++num;
            }
            return -1;
        }

        public int Add(J3DClassDef classDef)
        {
            int num = -1;
            if (classDef != null && !this.List.Contains((object)classDef))
                num = this.List.Add((object)classDef);
            else if (!this.List.Contains((object)classDef)) ;
            return num;
        }
    }
}
