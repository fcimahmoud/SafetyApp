
using Shared.ProblemTypeModels;

namespace Presentation
{
    public class ProblemTypeController(IServiceManager serviceManager)
        : ApiController
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var types = await serviceManager.ProblemTypeService.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            var type = await serviceManager.ProblemTypeService.GetByIdAsync(id);
            return type == null ? NotFound() : Ok(type);
        }

        [HttpPost]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> Create([FromBody] CreateProblemTypeDto dto)
        {
            var created = await serviceManager.ProblemTypeService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> Update(string id, [FromBody] CreateProblemTypeDto dto)
        {
            var success = await serviceManager.ProblemTypeService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await serviceManager.ProblemTypeService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
