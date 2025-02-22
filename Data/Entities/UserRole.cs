using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_spr321.Data.Entities
{
    public class UserRole
    {
        public String Id          { get; set; } = null!;
        public String Description { get; set; } = null!;
        public int    CanCreate   { get; set; }
        public int    CanRead     { get; set; }
        public int    CanUpdate   { get; set; }
        public int    CanDelete   { get; set; }


        public override string ToString()
        {
            return $"{Id} ({Description}): {CanCreate}/{CanRead}/{CanUpdate}/{CanDelete}";
        }
    }
}
