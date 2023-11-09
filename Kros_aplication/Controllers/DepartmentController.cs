using AutoMapper;
using Kros_aplication.Dto;
using Kros_aplication.Interfaces;
using Kros_aplication.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Kros_aplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        
       
            private readonly IDepartmentRepository _departmentRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly Kros_ZadanieContext _context;
        private readonly IMapper _mapper;

            public DepartmentController(IDepartmentRepository departmentRepository,
                IWorkerRepository workerRepository,
                IProjectRepository projectRepository,
                Kros_ZadanieContext context,
                IMapper mapper)
            {
                _departmentRepository = departmentRepository;
                _workerRepository = workerRepository;
            _projectRepository = projectRepository;
            _context = context;
            _mapper = mapper;
            }

            [HttpGet]
            [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
            public IActionResult GetDepartment()
            {
                var Department = _mapper.Map<List<DepartmentDto>>(_departmentRepository.GetDepartment());

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(Department);
            }

            [HttpGet("id")]
            [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
            [ProducesResponseType(400)]
            public IActionResult GetDepartment(int id)
            {
                if (!_departmentRepository.IsDepartmentExists(id))
                    return NotFound();

                var worker = _mapper.Map<DepartmentDto>(_departmentRepository.GetDepartment(id));
                // var worker = _workerRepository.GetWorker(id);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(worker);
            }

        [HttpGet("Manager")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
        [ProducesResponseType(400)]
        public IActionResult GetManager(int id)
        {
            if (!_departmentRepository.IsDepartmentExists(id))
                return NotFound();

            var worker = _mapper.Map<WorkerDto>(_departmentRepository.GetManager(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("Project")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Worker>))]
        [ProducesResponseType(400)]
        public IActionResult GetProject(int id)
        {
            if (!_departmentRepository.IsDepartmentExists(id))
                return NotFound();

            var worker = _mapper.Map<ProjectDto>(_departmentRepository.GetProject(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDepartment([FromQuery] int idManager ,[FromQuery] int projectId ,[FromBody] DepartmentDto departmentCreate) 
        {
            if (departmentCreate == null)
                return BadRequest();

            var department = _departmentRepository.GetDepartment()
                .Where(c => c.Name.Trim().ToUpper() == departmentCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (department != null)
            {
                ModelState.AddModelError("","Department already exusts");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var departmentMap = _mapper.Map<Department>(departmentCreate);

            if (_workerRepository.IsWorkerExists(idManager) && _projectRepository.IsProjectExists(projectId))
            {
                departmentMap.IdManager = idManager;
                departmentMap.ProjectId = projectId;
            } else
            {
                return BadRequest();
            }

            if (!_departmentRepository.CreateDepartment(departmentMap))
            {
                ModelState.AddModelError("","Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{departmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDepartment(int departmentId,
            [FromQuery] int idManager, [FromQuery] int projectId,
            [FromBody] DepartmentDto updatedDepartment)
        {
            if (updatedDepartment == null)
                return BadRequest(ModelState);

            if (departmentId != updatedDepartment.Id)
                return BadRequest(ModelState);

            if (!_departmentRepository.IsDepartmentExists(departmentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var departmentMap = _mapper.Map<Department>(updatedDepartment);

            if (_workerRepository.IsWorkerExists(idManager))
            {
                departmentMap.IdManager = idManager;
            }
            else
            {
                departmentMap.IdManager = _context.Departments.Where(p => p.Id == departmentMap.Id).Select(c => c.IdManager).FirstOrDefault();
            }

            if (_projectRepository.IsProjectExists(projectId))
            {
                departmentMap.ProjectId = projectId;
            }
            else
            {
                departmentMap.ProjectId = _context.Departments.Where(p => p.Id == departmentMap.Id).Select(c => c.ProjectId).FirstOrDefault();
            }

            if (!_departmentRepository.UpdateDepartment(departmentMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{departmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDepartment(int departmentId)
        {
            if (!_departmentRepository.IsDepartmentExists(departmentId))
            {
                return NotFound();
            }

            var departmentToDelete = _departmentRepository.GetDepartment(departmentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_departmentRepository.DeleteDepartment(departmentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return Ok("Deleted successfully");
        }
    }
 }
