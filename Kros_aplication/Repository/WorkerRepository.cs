using Kros_aplication.Interfaces;
using Kros_aplication.Models;

namespace Kros_aplication.Repository 
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly Kros_ZadanieContext _context;

        public WorkerRepository(Kros_ZadanieContext context)
        {
            _context = context;
        }

        public bool CreateWorker(Worker worker)
        {
            _context.Add(worker);

            return Save();
        }

        public bool DeleteWorker(Worker worker)
        {
            _context.Remove(worker);

            return Save();
        }

        public Worker GetWorker(int id)
        {
            return _context.Workers.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Worker> GetWorkers()
        {
            return _context.Workers.OrderBy(p => p.Id).ToList();
        }

        public bool IsWorkerExists(int id)
        {
            return _context.Workers.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateWorker(Worker worker)
        {
            _context.Update(worker);
            return Save();
        }
    }
}
