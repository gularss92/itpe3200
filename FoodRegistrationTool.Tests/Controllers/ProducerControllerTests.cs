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
    // Positive test - Table returns view
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
        Assert.Equal(2, model.Count()); // Asserts the number of objects
        Assert.Equal(producers, model); // Asserts that the result objects match the input
    }

    // Negative test - Table returns NotFound
    [Fact]
    public async Task Table_ReturnsNotFound_WhenNoProducersExist()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        mockProductRepository.Setup(repo => repo.GetAllProducers()).ReturnsAsync((List<Producer>)null);

        // Act
        var result = await producerController.Table();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Producer list not found", notFoundResult.Value);
    }

    // Positive test - [Get]Create returns view
    [Fact]
    public void Create_Get_ReturnsViewResult()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);

        // Act
        var result = producerController.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    // Positive test - [Post]Create returns RedirectToAction
    [Fact]
    public async Task Create_Post_ReturnsRedirectToAction_WhenModelIsValid()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);

        var producer = new Producer { ProducerId = 1, Name = "New Producer", Address = "New Address" };
        mockProductRepository.Setup(repo => repo.CreateProducer(producer)).ReturnsAsync(true);

        // Act
        var result = await producerController.Create(producer);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Table", redirectToActionResult.ActionName);
    }

    // Negative test - [Post]Create returns view
    [Fact]
    public async Task Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        producerController.ModelState.AddModelError("error", "error");
        var producer = new Producer { ProducerId = 1, Name = "Producer 1", Address = "Test Road" };

        // Act
        var result = await producerController.Create(producer);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(producer, viewResult.Model);
    }

    // Negative test - [Get]Update returns BadRequest
    [Fact]
    public async Task Update_Get_ReturnsBadRequest_WhenProducerNotFound()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        mockProductRepository.Setup(repo => repo.GetProducerById(1)).ReturnsAsync((Producer)null);

        // Act
        var result = await producerController.Update(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Producer not found for the ProducerId", badRequestResult.Value);
    }

    // Positive test - [Post]Update returns RedirectToAction
    [Fact]
    public async Task Update_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        var producer = new Producer { ProducerId = 1, Name = "Producer 1", Address = "Test Road" };
        mockProductRepository.Setup(repo => repo.UpdateProducer(producer)).ReturnsAsync(true);

        // Act
        var result = await producerController.Update(producer);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Table", redirectToActionResult.ActionName);
    }

    // Negative test - [Post]Update returns view
    [Fact]
    public async Task Update_Post_ReturnsViewResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        producerController.ModelState.AddModelError("error", "error");
        var producer = new Producer { ProducerId = 1, Name = "Producer 1", Address = "Test Road" };

        // Act
        var result = await producerController.Update(producer);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(producer, viewResult.Model);
    }

    // Positive test - [Get]Delete returns view
    [Fact]
    public async Task Delete_Get_ReturnsViewResult_WithProducer()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        var producer = new Producer { ProducerId = 1, Name = "Producer 1", Address = "Test Road" };
        mockProductRepository.Setup(repo => repo.GetProducerById(1)).ReturnsAsync(producer);

        // Act
        var result = await producerController.Delete(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(producer, viewResult.Model);
    }

    // Negative test - [Get]Delete returns BadRequest
    [Fact]
    public async Task Delete_Get_ReturnsBadRequest_WhenProducerNotFound()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        mockProductRepository.Setup(repo => repo.GetProducerById(1)).ReturnsAsync((Producer)null);

        // Act
        var result = await producerController.Delete(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Producer not found for the ProducerId", badRequestResult.Value);
    }

    // Positive test - [Post]DeleteConfirmed returns RedirectToAction
    [Fact]
    public async Task DeleteConfirmed_Post_ReturnsRedirectToActionResult_WhenDeletionIsSuccessful()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        mockProductRepository.Setup(repo => repo.DeleteProducer(1)).ReturnsAsync(true);

        // Act
        var result = await producerController.DeleteConfirmed(1);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Table", redirectToActionResult.ActionName);
    }

    // Negative test - [Post]DeleteConfirmed returns BadRequest
    [Fact]
    public async Task DeleteConfirmed_Post_ReturnsBadRequest_WhenDeletionFails()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProducerController>>();
        var producerController = new ProducerController(mockProductRepository.Object, mockLogger.Object);
        mockProductRepository.Setup(repo => repo.DeleteProducer(1)).ReturnsAsync(false);

        // Act
        var result = await producerController.DeleteConfirmed(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Producer deletion failed", badRequestResult.Value);
    }
}