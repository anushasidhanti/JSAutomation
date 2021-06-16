using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDataRef
{
    public class J3DMemberDefs : CollectionBase
    {
        public J3DMemberDef this[int index]
        {
            get
            {
                return (J3DMemberDef)this.List[index];
            }
            set
            {
                this.List[index] = (object)value;
            }
        }

        public J3DMemberDef this[string item]
        {
            get
            {
                return this.List.Cast<J3DMemberDef>().Where<J3DMemberDef>((Func<J3DMemberDef, bool>)(x =>
                {
                    if (!(x.Name == item))
                        return x.oid == item;
                    return true;
                })).ToList<J3DMemberDef>()[0];
            }
        }

        public int IndexOf(J3DMemberDef memberDef)
        {
            int num = 0;
            foreach (J3DMemberDef x3DmemberDef in (IEnumerable)this.List)
            {
                if (x3DmemberDef.oid.Equals(memberDef.oid))
                    return num;
                ++num;
            }
            return -1;
        }

        public int Add(J3DMemberDef memberDef)
        {
            int num = -1;
            if (memberDef != null && !this.List.Contains((object)memberDef))
                num = this.List.Add((object)memberDef);
            return num;
        }
    }
}
