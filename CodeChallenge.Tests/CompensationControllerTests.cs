
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;
        private static Compensation compensationToInsert = new Compensation()
        {
            Salary = 100000,
            EffectiveDate = new DateTime()
        };

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();

            // because we don't currently have separately seeded "compensation" entries to start from, do some compensation setup here instead
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var employee = getRequestTask.Result.DeserializeContent<Employee>();

            compensationToInsert.Employee = employee;
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        #region Compensation endpoints
        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var requestContent = new JsonSerialization().ToJson(compensationToInsert);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.AreEqual(compensationToInsert.Employee.EmployeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensationToInsert.Salary, newCompensation.Salary);
            Assert.AreEqual(compensationToInsert.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void CreateCompensation_Returns_NotFound()
        {
            // Arrange
            var compensationWithEmployeeThatDoesNotExist = new Compensation
            {
                Employee = new Employee
                {
                    EmployeeId = "123"
                },
                Salary = 250000,
                EffectiveDate = new DateTime()
            };

            var requestContent = new JsonSerialization().ToJson(compensationWithEmployeeThatDoesNotExist);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetCompensation_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensationToInsert.Employee.EmployeeId, compensation.Employee.EmployeeId);
            Assert.AreEqual(compensationToInsert.Salary, compensation.Salary);
            Assert.AreEqual(compensationToInsert.EffectiveDate, compensation.EffectiveDate);
        }

        [TestMethod]
        public void GetCompensation_Returns_NotFound()
        {
            // Arrange
            var employeeId = "123";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}
