using DataExporter.Dtos;
using DataExporter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DataExporter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private IPolicyService _policyService;

        public PoliciesController(IPolicyService policyService) 
        { 
            _policyService = policyService;
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ReadPolicyDto))]
        public async Task<IActionResult> PostPolicies([FromBody]CreatePolicyDto createPolicyDto, CancellationToken cancellationToken)
        {
            var response = await _policyService.CreatePolicyAsync(createPolicyDto, cancellationToken);
            return CreatedAtRoute(nameof(GetPolicy), new { id = response!.Id }, response);
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadPolicyDto>))]
        public async Task<IActionResult> GetPolicies(CancellationToken cancellationToken)
        {
            return Ok(await _policyService.ReadPoliciesAsync(null, cancellationToken));
        }

        [HttpGet("{id}", Name = nameof(GetPolicy))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReadPolicyDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPolicy(int id, CancellationToken cancellationToken)
        {
            var policy = await _policyService.ReadPolicyAsync(id, cancellationToken);

            return policy is not null ? Ok(policy) : NotFound();
        }


        [HttpPost("export")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadPolicyDto>))]
        public async Task<IActionResult> ExportData([FromQuery]DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            return Ok(await _policyService.ReadPoliciesAsync(new Models.ReadPoliciesFilterRequest { StartDate = startDate, EndDate = endDate}, cancellationToken));
        }
    }
}
