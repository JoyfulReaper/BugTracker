using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace BugTracker.Models
{
    public class Company
    {
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; }

        [DisplayName("Company Description")]
        public string Description { get; set; }

        ////////////
        
        public virtual ICollection<BTUser> Memebers { get; set; }

        public virtual ICollection<Projcet> Projcets { get; set; }
    }
}
