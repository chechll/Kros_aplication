namespace Kros_aplication.Interfaces
{
    public interface IDividionRepository
    {
        ICollection<Division> GetDivision();
        Division GetDivision(int id);
        Worker GetManager(int id);
        Firm GetFirm(int id);
        ICollection<Project> GetProjects(int id);
        bool IsDivisionExists(int id);
        bool CreateDivision(Division division);
        bool UpdateDivision(Division division);
        bool DeleteDivision(Division division);
        bool DeleteDivision(List<Division> divisions);
        public bool IsWorkerExists(int id);
        ICollection<Division> GetDivisionsByFirmId(int firmId);
        bool Save();
    }
}
