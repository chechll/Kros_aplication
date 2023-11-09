using AutoMapper;
using Kros_aplication.Dto;
using Kros_aplication.Interfaces;
using Kros_aplication.Models;
using Kros_aplication.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Kros_aplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirmController : Controller
    {
        private readonly IFirmRepository _firmRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IDividionRepository _dividionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly Kros_ZadanieContext _context;
        private readonly IMapper _mapper;
        public FirmController(IFirmRepository firmRepository,
            IWorkerRepository workerRepository,
            IDividionRepository dividionRepository,
            IDepartmentRepository departmentRepository,
            IProjectRepository projectRepository,
            Kros_ZadanieContext context,
                IMapper mapper) 
        {
            _firmRepository = firmRepository;
            _workerRepository = workerRepository;
            _dividionRepository = dividionRepository;
            _departmentRepository = departmentRepository;
            _projectRepository = projectRepository;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Firm>))]
        public IActionResult GetFirm()
        {
            var Firm = _mapper.Map<List<FirmDto>>(_firmRepository.GetFirm());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Firm);
        }

        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Firm>))]
        [ProducesResponseType(400)]
        public IActionResult GetFirm(int id)
        {
            if (!_firmRepository.IsFirmExists(id))
                return NotFound();

            var worker = _mapper.Map<FirmDto>(_firmRepository.GetFirm(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("Manager")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Firm>))]
        [ProducesResponseType(400)]
        public IActionResult GetManager(int id)
        {
            if (!_firmRepository.IsFirmExists(id))
                return NotFound();

            var worker = _mapper.Map<WorkerDto>(_firmRepository.GetManager(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("Division")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Firm>))]
        [ProducesResponseType(400)]
        public IActionResult GetDivisions(int id)
        {
            if (!_firmRepository.IsFirmExists(id))
                return NotFound();

            var worker = _mapper.Map<List<DivisionDto>>(_firmRepository.GetDivisions(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateFirm([FromQuery] int idManager, [FromBody] FirmDto firmCreate)
        {
            if (firmCreate == null)
                return BadRequest();

            var firm = _firmRepository.GetFirm()
                .Where(c => c.Name.Trim().ToUpper() == firmCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (firm != null)
            {
                ModelState.AddModelError("", "Firm already exusts");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var firmMap = _mapper.Map<Firm>(firmCreate);

            if (_workerRepository.IsWorkerExists(idManager))
            {
                firmMap.IdManager = idManager;
            }
            else
            {
                return BadRequest();
            }

            if (!_firmRepository.CreateFirm(firmMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{firmId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFirm(int firmId,
            [FromQuery] int idManager,
            [FromBody] FirmDto updatedFirm)
        {
            if (updatedFirm == null)
                return BadRequest(ModelState);

            if (firmId != updatedFirm.Id)
                return BadRequest(ModelState);

            if (!_firmRepository.IsFirmExists(firmId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var firmMap = _mapper.Map<Firm>(updatedFirm);

            if (_workerRepository.IsWorkerExists(idManager))
            {
                firmMap.IdManager = idManager;
            } 
            else
            {
                firmMap.IdManager = _context.Firms.Where(p => p.Id == firmMap.Id).Select(c => c.IdManager).FirstOrDefault();
            }

            if (!_firmRepository.UpdateFirm(firmMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{firmId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFirm(int firmId)
        {
            if (!_firmRepository.IsFirmExists(firmId))
            {
                return NotFound();
            }

            var firmToDelete = _firmRepository.GetFirm(firmId);
            var divisionsToDelete = _dividionRepository.GetDivisionsByFirmId(firmId).ToList();

            foreach (var division in divisionsToDelete)
            {
                var projectsToDelete = _projectRepository.GetProjectsByDivisiontId(division.Id).ToList();

                foreach (var project in projectsToDelete)
                {
                    var departmentsToDelete = _departmentRepository.GetDepartmentsByProjectId(project.Id).ToList();
                    if (!_departmentRepository.DeleteDepartments(departmentsToDelete))
                    {
                        ModelState.AddModelError("", "Something went wrong deleting owner");
                        return StatusCode(500, ModelState);
                    }
                }
                if (!_projectRepository.DeleteProjects(projectsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting owner");
                    return StatusCode(500, ModelState);
                }
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_dividionRepository.DeleteDivision(divisionsToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);
            }

            if (!_firmRepository.DeleteFirm(firmToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
