using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDataRef
{
    class JServer
    {
        private string serverPath;
        private string uid;
        private string plantName;

        public string ServerPath
        {
            get
            {
                return this.serverPath;
            }
            set
            {
                this.serverPath = value;
            }
        }

        public string UID
        {
            get
            {
                return this.uid;
            }
            set
            {
                this.uid = value;
            }
        }

        public string PlantName
        {
            get
            {
                return this.plantName;
            }
            set
            {
                this.plantName = value;
            }
        }
    }
}
