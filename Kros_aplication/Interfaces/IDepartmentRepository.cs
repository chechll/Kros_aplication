using Microsoft.EntityFrameworkCore;

namespace Kros_aplication.Interfaces
{
    public interface IDepartmentRepository
    {
        ICollection<Department> GetDepartment();
        Department GetDepartment(int id);
        Worker GetManager(int id);
        Project GetProject(int id);
        bool IsDepartmentExists(int id);

        bool CreateDepartment(Department department);
        bool UpdateDepartment(Department department);
        bool DeleteDepartment(Department department);
        bool DeleteDepartments(List<Department> departments);
        public bool IsWorkerExists(int id);
        ICollection<Department> GetDepartmentsByProjectId(int projectId);
        bool Save();
    }
}
