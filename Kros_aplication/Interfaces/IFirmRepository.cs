namespace Kros_aplication.Interfaces
{
    public interface IFirmRepository
    {
        ICollection<Firm> GetFirm();
        Firm GetFirm(int id);
        Worker GetManager(int id);
        ICollection<Division> GetDivisions(int id);
        bool IsFirmExists(int id);
        bool CreateFirm(Firm firm);
        bool UpdateFirm(Firm firm);
        bool DeleteFirm(Firm firm);
        public bool IsWorkerExists(int id);
        bool Save();
    }
}
