using System;
using System.Collections.Generic;

namespace Kros_aplication.Models
{
    public partial class Worker
    {
        public Worker()
        {
            Departments = new HashSet<Department>();
            Divisions = new HashSet<Division>();
            Firms = new HashSet<Firm>();
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public int? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual ICollection<Firm> Firms { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
