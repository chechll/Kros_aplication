using Kros_aplication.Interfaces;
using Kros_aplication.Models;

namespace Kros_aplication.Repository
{
    public class FirmRepository : IFirmRepository
    {
        private readonly Kros_ZadanieContext _context;

        public FirmRepository(Kros_ZadanieContext context)
        {
            _context = context;
        }

        public bool CreateFirm(Firm firm)
        {
            _context.Add(firm);

            return Save();
        }

        public bool DeleteFirm(Firm firm)
        {
            _context.Remove(firm);

            return Save();
        }

        public ICollection<Division> GetDivisions(int id)
        {
            return _context.Divisions.Where(p => p.FirmId == id).ToList();
        }

        public ICollection<Firm> GetFirm()
        {
            return _context.Firms.OrderBy(p => p.Id).ToList();
        }

        public Firm GetFirm(int id)
        {
            return _context.Firms.Where(p => p.Id == id).FirstOrDefault();
        }

        public Worker GetManager(int id)
        {
            var idMan = _context.Firms.Where(p => p.Id == id).Select(c => c.IdManager).FirstOrDefault();

            return _context.Workers.Where(p => p.Id == idMan).FirstOrDefault();
        }

        public bool IsFirmExists(int id)
        {
            return _context.Firms.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateFirm(Firm firm)
        {
            _context.Update(firm);
            return Save();
        }

        public bool IsWorkerExists(int id)
        {
            return _context.Firms.Any(p => p.IdManager == id);
        }
    }
}
