using CodeChallenge.Models;
using System;

namespace CodeChallenge.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Gather employee data for a given employee id.
        /// </summary>
        /// <param name="employeeId">The employee id retrieve data for</param>
        /// <returns>The targeted employee data</returns>
        Employee GetEmployeeById(String employeeId);

        /// <summary>
        /// Generate new employee data.
        /// </summary>
        /// <param name="employee">The new employee data</param>
        /// <returns>The generated employee data</returns>
        Employee CreateEmployee(Employee employee);

        /// <summary>
        /// Replace/update existing employee data.
        /// </summary>
        /// <param name="originalEmployee">The original employee data</param>
        /// <param name="newEmployee">The new employee data</param>
        /// <returns>The updated employee data</returns>
        Employee ReplaceEmployee(Employee originalEmployee, Employee newEmployee);
    }
}
