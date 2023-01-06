using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableEntityCloner
{
    public class Common
    {
        public enum EntityLinkType
        {
            Relation,
            Association,
            Connection
        };

        public enum CloneBehaviour
        {
            Clone,
            Associate
        };
    }
}
