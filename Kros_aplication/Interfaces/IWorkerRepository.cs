namespace Kros_aplication.Interfaces
{
    public interface IWorkerRepository
    {
        ICollection<Worker> GetWorkers();
        Worker GetWorker(int id);
        bool IsWorkerExists(int id);
        bool CreateWorker(Worker worker);
        bool UpdateWorker(Worker worker);
        bool DeleteWorker(Worker worker);
        bool Save();
    }
}
