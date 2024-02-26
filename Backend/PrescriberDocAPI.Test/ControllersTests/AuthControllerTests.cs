using Microsoft.AspNetCore.Mvc.Formatters;
using Moq;
using PrescriberDocAPI.Patients.Domain;
using PrescriberDocAPI.Patients.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrescriberDocAPI.Test.ControllersTests
{
    public class AuthControllerTests
    {
        CrudBase _controller;
        public AuthControllerTests()
        {
            var mockRepository = new Mock<Repository<Medicine>>(); // Assuming UserService depends on IUserRepository
            mockRepository.Setup(repo => repo.Get()).ReturnsAsync(EntityMocks.Medicines);

        }
        //[Fact]
        //public async Task GetUserAsync_ReturnsUser()
        //{
        //    // Arrange
        //    var mockRepository = new Mock<Repository<Medicine>>(); // Assuming UserService depends on IUserRepository
        //    mockRepository.Setup(repo => repo.Get()).ReturnsAsync(EntityMocks.Medicines);

        //    var userService = new CrudBase() (mockRepository.Object);

        //    // Act
        //    var result = await userService.GetUserAsync(1);

        //    // Assert
        //    Assert.NotNull(result);
        //}
    }
}
