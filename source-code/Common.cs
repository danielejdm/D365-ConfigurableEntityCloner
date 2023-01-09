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
    public class CopyEntityInfo
    {
       public string EntityName { get; set; }
       public IEnumerable<string> FieldsToCopy { get; set; }
       public IEnumerable<string> BlacklistFields { get; set; }
    }
}
