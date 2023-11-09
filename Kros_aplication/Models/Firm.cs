using System;
using System.Collections.Generic;

namespace Kros_aplication.Models
{
    public partial class Firm
    {
        public Firm()
        {
            Divisions = new HashSet<Division>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IdManager { get; set; }

        public virtual Worker IdManagerNavigation { get; set; } = null!;
        public virtual ICollection<Division> Divisions { get; set; }
    }
}
