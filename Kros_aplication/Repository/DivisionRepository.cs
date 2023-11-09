using Kros_aplication.Interfaces;
using Kros_aplication.Models;

namespace Kros_aplication.Repository
{
    public class DivisionRepository : IDividionRepository
    {
        private readonly Kros_ZadanieContext _context;

        public DivisionRepository(Kros_ZadanieContext context)
        {
            _context = context;
        }

        public ICollection<Division> GetDivision()
        {
            return _context.Divisions.OrderBy(p => p.Id).ToList();
        }

        public Division GetDivision(int id)
        {
            return _context.Divisions.Where(p => p.Id == id).FirstOrDefault();
        }

        public Worker GetManager(int id)
        {
            var idMan = _context.Divisions.Where(p => p.Id == id).Select(c => c.IdManager).FirstOrDefault();

            return _context.Workers.Where(p => p.Id == idMan).FirstOrDefault();
        }

        public ICollection<Project> GetProjects(int id)
        {
            return _context.Projects.Where(p => p.DivisionId == id).ToList();
        }

        public Firm GetFirm(int id)
        {
            var idPr = _context.Divisions.Where(p => p.Id == id).Select(c => c.FirmId).FirstOrDefault();

            return _context.Firms.Where(p => p.Id == idPr).FirstOrDefault();
        }

        public bool IsDivisionExists(int id)
        {
            return _context.Divisions.Any(p => p.Id == id);
        }

        public bool CreateDivision(Division division)
        {
            _context.Add(division);

            return Save();
        }

        public bool UpdateDivision(Division division)
        {
            _context.Update(division);
            return Save();
        }

        public bool DeleteDivision(Division division)
        {
            _context.Remove(division);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool IsWorkerExists(int id)
        {
            return _context.Divisions.Any(p => p.IdManager == id);
        }

        public bool DeleteDivision(List<Division> divisions)
        {
            _context.RemoveRange(divisions);

            return Save();
        }

        public ICollection<Division> GetDivisionsByFirmId(int firmId)
        {
            return _context.Divisions.Where(p => p.FirmId == firmId).ToList();
        }
    }
}
