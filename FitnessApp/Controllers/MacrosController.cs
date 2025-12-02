using FitnessApp.DAL.Repositories;
using FitnessApp.Domain.Interfaces;
using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MacrosController : ControllerBase
    {
        private readonly IMacrosApiClient _macrosServive;
        public MacrosController(IMacrosApiClient macrosApi )
        {
            _macrosServive = macrosApi;
        }
       
        [HttpGet("{query}")]
        public async Task<IActionResult> Get(string query,CancellationToken ct)
        {
            try
            {
                var result = await _macrosServive.GetMacrosAsync(query, ct);
                if(result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (TaskCanceledException tce)
            {
                
                return StatusCode(500, ApiResponse<object>.Fail("External service timeout."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Cannot reach external service."));
            }
        }


        
        
    }
}
