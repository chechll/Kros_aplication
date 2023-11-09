using AutoMapper;
using Kros_aplication.Dto;
using Kros_aplication.Interfaces;
using Kros_aplication.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Kros_aplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : Controller
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IFirmRepository _firmRepository;
        private readonly IDividionRepository _idividionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public WorkerController(IWorkerRepository workerRepository,
            IFirmRepository firmRepository,
            IDividionRepository idividionRepository,
            IDepartmentRepository departmentRepository,
            IProjectRepository projectRepository,
            IMapper mapper)
        {
            _workerRepository = workerRepository;
            _firmRepository = firmRepository;
            _idividionRepository = idividionRepository;
            _departmentRepository = departmentRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Worker>))]
        public IActionResult GetWorker()
        {
            var worker = _mapper.Map<List<WorkerDto>>(_workerRepository.GetWorkers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Worker>))]
        [ProducesResponseType(400)]
        public IActionResult GetWorker(int id)
        {
            if (!_workerRepository.IsWorkerExists(id))
                return NotFound();

            var worker = _mapper.Map<WorkerDto>(_workerRepository.GetWorker(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(worker);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDepartment([FromBody] WorkerDto workerCreate)
        {
            if (workerCreate == null)
                return BadRequest();

            var worker = _workerRepository.GetWorkers()
                .Where(c => c.Id == workerCreate.Id)
                .FirstOrDefault();

            if (worker != null)
            {
                ModelState.AddModelError("", "worker already exusts");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workerMap = _mapper.Map<Worker>(workerCreate);

            if (!_workerRepository.CreateWorker(workerMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWorker(int workerId,
            [FromBody] WorkerDto updatedWorker)
        {
            if (updatedWorker == null)
                return BadRequest(ModelState);

            if (workerId != updatedWorker.Id)
                return BadRequest(ModelState);

            if (!_workerRepository.IsWorkerExists(workerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var workerMap = _mapper.Map<Worker>(updatedWorker);

            if (!_workerRepository.UpdateWorker(workerMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteWorker(int workerId)
        {
            if (!_workerRepository.IsWorkerExists(workerId))
            {
                return NotFound();
            }

            var workerToDelete = _workerRepository.GetWorker(workerId);
            
            if (_firmRepository.IsWorkerExists(workerId) || _idividionRepository.IsWorkerExists(workerId) ||
                _departmentRepository.IsWorkerExists(workerId) || _projectRepository.IsWorkerExists(workerId))
            {
                ModelState.AddModelError("", "Worker is in some other table. Update that table first");
                return StatusCode(500, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_workerRepository.DeleteWorker(workerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
