using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    // "Compensation" is a broad and distinct term/resource from "Employee,"
    // so create a new controller along with a service, DTO, and any necessary interfaces in the style that "Employee" does.
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        #region Fields
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;
        #endregion

        #region Constructor
        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }
        #endregion

        #region Compensation endpoints
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            try
            {
                _logger.LogDebug($"Received compensation create request for employee with id {compensation.Employee.EmployeeId}");

                // verify employee linked with this compensation even exists before creating it
                if (_employeeService.GetEmployeeById(compensation.Employee.EmployeeId) == null)
                {
                    return NotFound();
                }

                _compensationService.CreateCompensation(compensation);

                return CreatedAtRoute("getCompensationByEmployeeId", new { employeeId = compensation.Employee.EmployeeId }, compensation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error" });
            }
        }

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for employee with id {employeeId}");

            var compensation = _compensationService.GetCompensationByEmployeeId(employeeId);

            if (compensation == null)
            {
                return NotFound();
            }

            return Ok(compensation);
        }
        #endregion
    }
}
