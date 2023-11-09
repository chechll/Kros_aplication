using AutoMapper;
using Kros_aplication.Dto;
using Kros_aplication.Interfaces;
using Kros_aplication.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Kros_aplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private readonly IDividionRepository _divisionRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IFirmRepository _firmRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly Kros_ZadanieContext _context;
        private readonly IMapper _mapper;
        public DivisionController(IDividionRepository divisionRepository,
            IWorkerRepository workerRepository,
            IFirmRepository firmRepository,
            IProjectRepository projectRepository,
            IDepartmentRepository departmentRepository,
            Kros_ZadanieContext context,
                IMapper mapper)
        {
            _divisionRepository = divisionRepository;
            _workerRepository = workerRepository;
            _firmRepository = firmRepository;
            _projectRepository = projectRepository;
            _departmentRepository = departmentRepository;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Division>))]
        public IActionResult GetDivision()
        {
            var Division = _mapper.Map<List<DivisionDto>>(_divisionRepository.GetDivision());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Division);
        }

        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Division>))]
        [ProducesResponseType(400)]
        public IActionResult GetDivision(int id)
        {
            if (!_divisionRepository.IsDivisionExists(id))
                return NotFound();

            var Division = _mapper.Map<DivisionDto>(_divisionRepository.GetDivision(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Division);
        }

        [HttpGet("Manager")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Division>))]
        [ProducesResponseType(400)]
        public IActionResult GetManager(int id)
        {
            if (!_divisionRepository.IsDivisionExists(id))
                return NotFound();

            var worker = _mapper.Map<WorkerDto>(_divisionRepository.GetManager(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("Projects")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Division>))]
        [ProducesResponseType(400)]
        public IActionResult GetProjects(int id)
        {
            if (!_divisionRepository.IsDivisionExists(id))
                return NotFound();

            var worker = _mapper.Map<List<ProjectDto>>(_divisionRepository.GetProjects(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("Firm")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Division>))]
        [ProducesResponseType(400)]
        public IActionResult GetFirm(int id)
        {
            if (!_divisionRepository.IsDivisionExists(id))
                return NotFound();

            var worker = _mapper.Map<FirmDto>(_divisionRepository.GetFirm(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDivision([FromQuery] int idManager, [FromQuery] int firmId, [FromBody] DivisionDto divisionCreate)
        {
            if (divisionCreate == null)
                return BadRequest();

            var division = _divisionRepository.GetDivision()
                .Where(c => c.Name.Trim().ToUpper() == divisionCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (division != null)
            {
                ModelState.AddModelError("", "Department already exusts");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var divisionMap = _mapper.Map<Division>(divisionCreate);

            if (_workerRepository.IsWorkerExists(idManager) && _firmRepository.IsFirmExists(firmId))
            {
                divisionMap.IdManager = idManager;
                divisionMap.FirmId = firmId;
            }
            else
            {
                return BadRequest();
            }

            if (!_divisionRepository.CreateDivision(divisionMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{divisionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDivision(int divisionId,
            [FromQuery] int idManager, [FromQuery] int firmId,
            [FromBody] DivisionDto updatedDivision)
        {
            if (updatedDivision == null)
                return BadRequest(ModelState);

            if (divisionId != updatedDivision.Id)
                return BadRequest(ModelState);

            if (!_divisionRepository.IsDivisionExists(divisionId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var divisionMap = _mapper.Map<Division>(updatedDivision);

            if (_workerRepository.IsWorkerExists(idManager))
            {
                divisionMap.IdManager = idManager;
            }
            else
            {
                divisionMap.IdManager = _context.Divisions.Where(p => p.Id == divisionMap.Id).Select(c => c.IdManager).FirstOrDefault();
            }

            if (_firmRepository.IsFirmExists(firmId))
            {
                divisionMap.FirmId = firmId;
            }
            else
            {
                divisionMap.FirmId = _context.Divisions.Where(p => p.Id == divisionMap.Id).Select(c => c.FirmId).FirstOrDefault();
            }

            if (!_divisionRepository.UpdateDivision(divisionMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{divisionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProject(int divisionId)
        {
            if (!_divisionRepository.IsDivisionExists(divisionId))
            {
                return NotFound();
            }

            var divisionToDelete = _divisionRepository.GetDivision(divisionId);
            var projectToDelete = _projectRepository.GetProjectsByDivisiontId(divisionId).ToList();

            if (projectToDelete.Count != 0)
            {
                foreach (var project in projectToDelete)
                {
                    var departmentsToDelete = _departmentRepository.GetDepartmentsByProjectId(project.Id).ToList();
                    if (!_departmentRepository.DeleteDepartments(departmentsToDelete))
                    {
                        ModelState.AddModelError("", "Something went wrong ");
                        return StatusCode(500, ModelState);
                    }
                }


                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_projectRepository.DeleteProjects(projectToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong ");
                    return StatusCode(500, ModelState);
                }
            }
            if (!_divisionRepository.DeleteDivision(divisionToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
