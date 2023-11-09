namespace Kros_aplication.Interfaces
{
    public interface IProjectRepository
    {
        ICollection<Project> GetProject();
        Project GetProject(int id);
        Worker GetManager(int id);
        Division GetDivision(int id);
        ICollection<Department> GetDepartments(int id);
        bool IsProjectExists(int id);
        bool CreateProject(Project project);
        bool UpdateProject(Project Project);
        bool DeleteProject(Project Project);
        bool DeleteProjects(List<Project> projects);
        public bool IsWorkerExists(int id);
        ICollection<Project> GetProjectsByDivisiontId(int divisionId);
        bool Save();
    }
}
