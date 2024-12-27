using CodeChallenge.Models;
using System;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        /// <summary>
        /// Gather compensation data for a given employee id.
        /// </summary>
        /// <param name="employeeId">The employee id to target a compensation for</param>
        /// <returns>The targeted compensation data</returns>
        Compensation GetCompensationByEmployeeId(String employeeId);

        /// <summary>
        /// Generate new compensation data for a given employee.
        /// </summary>
        /// <param name="employeeId">The new compensation data to be created</param>
        /// <returns>The generated compensation data</returns>
        Compensation CreateCompensation(Compensation compensation);
    }
}
