using System;
using System.Collections.Generic;

namespace Kros_aplication.Models
{
    public partial class Project
    {
        public Project()
        {
            Departments = new HashSet<Department>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdManager { get; set; }
        public int DivisionId { get; set; }

        public virtual Division Division { get; set; } = null!;
        public virtual Worker IdManagerNavigation { get; set; } = null!;
        public virtual ICollection<Department> Departments { get; set; }
    }
}
