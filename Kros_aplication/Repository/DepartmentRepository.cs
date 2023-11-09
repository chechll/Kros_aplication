using Kros_aplication.Interfaces;
using Kros_aplication.Models;

namespace Kros_aplication.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly Kros_ZadanieContext _context;

        public DepartmentRepository(Kros_ZadanieContext context)
        {
            _context = context;
        }

        public bool CreateDepartment(Department department)
        {
            _context.Add(department);

            return Save();
        }

        public bool DeleteDepartment(Department department)
        {
            _context.Remove(department);

            return Save();
        }

        public bool DeleteDepartments(List<Department> departments)
        {
            _context.RemoveRange(departments);

            return Save();
        }

        public ICollection<Department> GetDepartment()
        {
            return _context.Departments.OrderBy(p => p.Id).ToList();
        }

        public Department GetDepartment(int id)
        {
            return _context.Departments.Where(p => p.Id == id).FirstOrDefault();
        }

        public Worker GetManager(int id)
        {
            var idMan = _context.Departments.Where(p => p.Id == id).Select(c => c.IdManager).FirstOrDefault();

            return _context.Workers.Where(p => p.Id == idMan).FirstOrDefault();
        }

        public Project GetProject(int id)
        {
            var idPr = _context.Departments.Where(p => p.Id == id).Select(c => c.ProjectId).FirstOrDefault();

            return _context.Projects.Where(p => p.Id == idPr).FirstOrDefault();
        }

        public bool IsDepartmentExists(int id)
        {
            return _context.Departments.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateDepartment(Department department)
        {
            _context.Update(department);
            return Save();
        }

        public bool IsWorkerExists(int id)
        {
            return _context.Departments.Any(p => p.IdManager == id);
        }

        public ICollection<Department> GetDepartmentsByProjectId(int projectId)
        {
            return _context.Departments.Where(p => p.ProjectId == projectId).ToList();
        }
    }
}
