using System;
using System.Collections.Generic;

namespace Kros_aplication.Models
{
    public partial class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdManager { get; set; }
        public int ProjectId { get; set; }

        public virtual Worker IdManagerNavigation { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
    }
}
