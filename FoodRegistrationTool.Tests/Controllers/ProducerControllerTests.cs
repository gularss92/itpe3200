using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using FoodRegistrationTool.Controllers;
using FoodRegistrationTool.Models;
using FoodRegistrationTool.DAL;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FoodRegistrationTool.Test.Controllers;
public class ProducerControllerTests
{
    [Fact]
    public async Task Table_ReturnsViewResult_WithListOfProducers()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);

        var producers = new List<Producer>
        {
            new Producer { ProducerId = 1, Name = "Producer 1", Address = "Test Road"},
            new Producer { ProducerId = 2, Name = "Producer 2", Address = "Test Avenue"}
        };
        mockProductRepository.Setup(repo => repo.GetAllProducers()).ReturnsAsync(producers);

        // Act
        var result = await producerController.Table();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Producer>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }
}