using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        #region Fields
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        #endregion

        #region Constructor
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }
        #endregion

        #region Employee endpoints
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            // I come from healthcare field. Learned to never expose anything remotely considered PHI. Depends on project, I suppose.
            //_logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");
            _logger.LogDebug($"Received employee create request using id of {employee.EmployeeId}");

            _employeeService.CreateEmployee(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.ReplaceEmployee(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
        #endregion

        #region ReportingStructure endpoints
        [HttpGet("reportingStructure/{employeeId}", Name = "getReportingStructure")]
        public IActionResult GetReportingStructure(String employeeId)
        {
            _logger.LogDebug($"Received GetReportingStructure get request for employee with id {employeeId}");

            Employee employee = _employeeService.GetEmployeeById(employeeId);

            if (employeeId == null || employee == null)
            {
                _logger.LogDebug($"No employee exists with id of {employeeId}");
                return NotFound();
            }

            int numOfReports = CountReportsUnderEmployee(employee);
            _logger.LogDebug($"Found {numOfReports} reports for employee with id {employeeId}");

            return Ok(
                new ReportingStructure
                {
                    Employee = employee,
                    NumberOfReports = numOfReports
                }
            );
        }
        #endregion

        #region Helper methods
        private int CountReportsUnderEmployee(Employee employee)
        {
            int reportCount = 0;

            if (employee.DirectReports == null || employee.DirectReports.Count == 0)
            {
                return reportCount;
            }

            reportCount = employee.DirectReports.Count;

            foreach(var directReport in employee.DirectReports)
            {
                Employee subEmployee = _employeeService.GetEmployeeById(directReport.EmployeeId);
                reportCount += CountReportsUnderEmployee(subEmployee);
            }

            return reportCount;
        }
        #endregion
    }
}
