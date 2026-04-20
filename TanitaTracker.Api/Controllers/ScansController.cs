using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TanitaTracker.Core.Entities;
using TanitaTracker.Core.Interfaces;

namespace TanitaTracker.Api.Controllers
{
    /// <summary>
    /// Implementation of the MVC based Scan Controller endpoint (/api/ScanController). 
    /// </summary>
    /// [Authorize]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")] // Endpoint definition
    public class ScansController : ControllerBase
    {
        private readonly IScanRepository _repository; // Via Dependency Injection a ScanRepository object is created
        private readonly IOcrService _ocrService; // // Via Dependency Injection a OcrService object is created

        public ScansController(IScanRepository repository, IOcrService ocrService)
        {
            _repository = repository;
            _ocrService = ocrService;
        }

        // Helper to securely get the logged-in user's ID. See Claims for the definition
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpPost("analyze")]
        public async Task<ActionResult<BodyCompositionScan>> AnalyzeDocument(IFormFile file, CancellationToken ct)
        {
            // TODO: Write a implementation
            throw new NotImplementedException("Not yet impemented");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BodyCompositionScan>>> GetMyScans(CancellationToken ct)
        {
            // TODO: Write implementation
            throw new NotImplementedException("Not yet implemented");
        }

        [HttpPost]
        public async Task<IActionResult> SaveScan([FromBody] BodyCompositionScan scan, CancellationToken ct)
        {
            // TODO: Write implementation
            throw new NotImplementedException("Not yet implemented");
        }

    }
}
