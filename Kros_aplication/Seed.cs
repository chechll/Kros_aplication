using Kros_aplication.Models;
using System.Diagnostics.Metrics;

namespace Kros_aplication
{
    public class Seed
    {
        private readonly Kros_ZadanieContext dataContext;
        public Seed(Kros_ZadanieContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Workers.Any())
            {
                var workers = new List<Worker>()
                {
                    new Worker()
                    {
                        Name = "Alan",
                        Surname = "Kvet",
                        PhoneNumber = 123456789,
                        Email = "kvet@mail.com",
                        Title = "Ing",
                    },
                    new Worker()
                    {
                        Name = "Rudolf",
                        Surname = "Zajac",
                        PhoneNumber = 123456779,
                        Email = "zajac@mail.com",
                        Title = "Ing",
                    },
                    new Worker()
                    {
                        Name = "Bob",
                        Surname = "Martin",
                        PhoneNumber = 122456789,
                        Email = "martin@mail.com",
                        Title = "Ing",
                    },
                    new Worker()
                    {
                        Name = "Steoh",
                        Surname = "Pink",
                        PhoneNumber = 122356789,
                        Email = "pink@mail.com",
                        Title = "bc",
                    },
                    new Worker()
                    {
                        Name = "Luter",
                        Surname = "Black",
                        PhoneNumber = 142456789,
                        Email = "black@mail.com",
                        Title = "Ing",
                    },
                    new Worker()
                    {
                        Name = "Zuzana",
                        Surname = "Fialova",
                        PhoneNumber = 122656789,
                        Email = "fialova@mail.com",
                        Title = "Ing",
                    },
                    new Worker()
                    {
                        Name = "Patrik",
                        Surname = "Konecny",
                        PhoneNumber = 122446789,
                        Email = "konecny@mail.com",
                        Title = "Phd",
                    },
                    new Worker()
                    {
                        Name = "Kristina",
                        Surname = "Markovna",
                        PhoneNumber = 132456799,
                        Email = "markovna@mail.com",
                        Title = "Ing",
                    },
                };
                dataContext.Workers.AddRange(workers);
                dataContext.SaveChanges();
            }
            if (!dataContext.Firms.Any())
            {
                var firms = new List<Firm>()
                {
                    new Firm()
                    {
                        Name = "Firm1",
                        IdManager = 1,
                    },
                };
                dataContext.Firms.AddRange(firms);
                dataContext.SaveChanges();
            }
            if (!dataContext.Divisions.Any())
            {
                var divisions = new List<Division>()
                {
                    new Division()
                    {
                        Name = "Div1",
                        IdManager = 2,
                        FirmId = 1,
                    },
                    new Division()
                    {
                        Name = "Div2",
                        IdManager = 3,
                        FirmId = 1,
                    },
                };
                dataContext.Divisions.AddRange(divisions);
                dataContext.SaveChanges();
            }
            if (!dataContext.Projects.Any())
            {
                var projects = new List<Project>()
                {
                    new Project()
                    {
                        Name = "Pr1",
                        IdManager = 4,
                        DivisionId = 1,
                    },
                    new Project()
                    {
                        Name = "Pr2",
                        IdManager = 5,
                        DivisionId = 2,
                    },
                };
                dataContext.Projects.AddRange(projects);
                dataContext.SaveChanges();
            }
            if (!dataContext.Departments.Any())
            {
                var departments = new List<Department>()
                {
                    new Department()
                    {
                        Name = "Dep1",
                        IdManager = 6,
                        ProjectId = 1,
                    },
                    new Department()
                    {
                        Name = "Dep2",
                        IdManager = 7,
                        ProjectId = 1,
                    },
                    new Department()
                    {
                        Name = "Dep3",
                        IdManager = 8,
                        ProjectId = 2,
                    },
                };
                dataContext.Departments.AddRange(departments);
                dataContext.SaveChanges();
            }
        }
    }
}
