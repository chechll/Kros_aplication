using Kros_aplication.Interfaces;
using Kros_aplication.Models;

namespace Kros_aplication.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly Kros_ZadanieContext _context;

        public ProjectRepository(Kros_ZadanieContext context)
        {
            _context = context;
        }

        public bool CreateProject(Project project)
        {
            _context.Add(project);

            return Save();
        }

        public bool DeleteProject(Project Project)
        {
            _context.Remove(Project);

            return Save();
        }

        public ICollection<Department> GetDepartments(int id)
        {
            return _context.Departments.Where(p => p.ProjectId == id).ToList();
        }

        public Division GetDivision(int id)
        {
            var idPr = _context.Projects.Where(p => p.Id == id).Select(c => c.DivisionId).FirstOrDefault();

            return _context.Divisions.Where(p => p.Id == idPr).FirstOrDefault();
        }

        public Worker GetManager(int id)
        {
            var idMan = _context.Projects.Where(p => p.Id == id).Select(c => c.IdManager).FirstOrDefault();

            return _context.Workers.Where(p => p.Id == idMan).FirstOrDefault();
        }

        public ICollection<Project> GetProject()
        {
            return _context.Projects.OrderBy(p => p.Id).ToList();
        }

        public Project GetProject(int id)
        {
            return _context.Projects.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool IsProjectExists(int id)
        {
            return _context.Projects.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProject(Project Project)
        {
            _context.Update(Project);
            return Save();
        }

        public bool IsWorkerExists(int id)
        {
            return _context.Projects.Any(p => p.IdManager == id);
        }

        public bool DeleteProjects(List<Project> projects)
        {
            _context.RemoveRange(projects);

            return Save();
        }

        public ICollection<Project> GetProjectsByDivisiontId(int divisionId)
        {
            return _context.Projects.Where(p => p.DivisionId == divisionId).ToList();

        }
    }
}
