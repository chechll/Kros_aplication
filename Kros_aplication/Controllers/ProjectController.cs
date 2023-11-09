using AutoMapper;
using Kros_aplication.Dto;
using Kros_aplication.Interfaces;
using Kros_aplication.Models;
using Kros_aplication.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kros_aplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly Kros_ZadanieContext _context;
        private readonly IDividionRepository _dividionRepository;
        private readonly IMapper _mapper;
        public ProjectController(IProjectRepository projectRepository,
            IWorkerRepository workerRepository,
            IDepartmentRepository departmentRepository,
            Kros_ZadanieContext context,
            IDividionRepository idividionRepository,
                IMapper mapper)
        {
            _projectRepository = projectRepository;
            _workerRepository = workerRepository;
            _departmentRepository = departmentRepository;
            _context = context;
            _dividionRepository = idividionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        public IActionResult GetProject()
        {
            var Project = _mapper.Map<List<ProjectDto>>(_projectRepository.GetProject());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Project);
        }

        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(400)]
        public IActionResult GetProject(int id)
        {
            if (!_projectRepository.IsProjectExists(id))
                return NotFound();

            var Project = _mapper.Map<ProjectDto>(_projectRepository.GetProject(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Project);
        }

        [HttpGet("Manager")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(400)]
        public IActionResult GetManager(int id)
        {
            if (!_projectRepository.IsProjectExists(id))
                return NotFound();

            var worker = _mapper.Map<WorkerDto>(_projectRepository.GetManager(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("Departments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(400)]
        public IActionResult GetDepartments(int id)
        {
            if (!_projectRepository.IsProjectExists(id))
                return NotFound();

            var worker = _mapper.Map<List<DepartmentDto>>(_projectRepository.GetDepartments(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("Division")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(400)]
        public IActionResult GetDivision(int id)
        {
            if (!_projectRepository.IsProjectExists(id))
                return NotFound();

            var worker = _mapper.Map< DivisionDto > (_projectRepository.GetDivision(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProject([FromQuery] int idManager, [FromQuery] int divisionId, [FromBody] ProjectDto projectCreate)
        {
            if (projectCreate == null)
                return BadRequest();

            var project = _projectRepository.GetProject()
                .Where(c => c.Name.Trim().ToUpper() == projectCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (project != null)
            {
                ModelState.AddModelError("", "Project already exusts");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var projectMap = _mapper.Map<Project>(projectCreate);

            if (_workerRepository.IsWorkerExists(idManager) && _dividionRepository.IsDivisionExists(divisionId))
            {
                projectMap.IdManager = idManager;
                projectMap.DivisionId = divisionId;
            }
            else
            {
                return BadRequest();
            }

            if (!_projectRepository.CreateProject(projectMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProject(int projectId,
            [FromQuery] int idManager, [FromQuery] int divisionId,
            [FromBody] ProjectDto updatedProject)
        {
            if (updatedProject == null)
                return BadRequest(ModelState);

            if (projectId != updatedProject.Id)
                return BadRequest(ModelState);

            if (!_projectRepository.IsProjectExists(projectId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var projectMap = _mapper.Map<Project>(updatedProject);

            if (_workerRepository.IsWorkerExists(idManager))
            {
                projectMap.IdManager = idManager;
            }
            else
            {
                projectMap.IdManager = _context.Projects.Where(p => p.Id == projectMap.Id).Select(c => c.IdManager).FirstOrDefault();
            }

            if (_dividionRepository.IsDivisionExists(divisionId))
            {
                projectMap.DivisionId = divisionId;
            }
            else
            {
                projectMap.DivisionId = _context.Projects.Where(p => p.Id == projectMap.Id).Select(c => c.DivisionId).FirstOrDefault();
            }

            if (!_projectRepository.UpdateProject(projectMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProject(int projectId)
        {
            if (!_projectRepository.IsProjectExists(projectId))
            {
                return NotFound();
            }

            var projectToDelete = _projectRepository.GetProject(projectId);
            var departmentsToDelete = _departmentRepository.GetDepartmentsByProjectId(projectId).ToList();

            if (departmentsToDelete.Count != 0)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_departmentRepository.DeleteDepartments(departmentsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return StatusCode(500, ModelState);
                }
            }
            if (!_projectRepository.DeleteProject(projectToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
