using System;
using System.Collections.Generic;

namespace Kros_aplication.Models
{
    public partial class Division
    {
        public Division()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdManager { get; set; }
        public int FirmId { get; set; }

        public virtual Firm Firm { get; set; } = null!;
        public virtual Worker IdManagerNavigation { get; set; } = null!;
        public virtual ICollection<Project> Projects { get; set; }
    }
}
