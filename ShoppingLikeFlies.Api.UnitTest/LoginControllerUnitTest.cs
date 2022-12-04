
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFiles.DomainServices.DTOs;
using ShoppingLikeFlies.Api.Configuration;

namespace ShoppingLikeFlies.Api.UnitTest;

public class LoginControllerUnitTest
{
    [Fact]
    public async Task Test1()
    {
        var caff = new Mock<ICaffService>();
        var data = new Mock<IDataService>();
        var mapper = getMapper();
        var logger = new LoggerConfiguration().CreateBootstrapLogger();

        var list = new List<CaffDTO>
        {
            new CaffDTO()
            {
                Id= 1,
                Year = 2020,
                Month = 1,
                Day= 1,
                Hour= 1,
                Minute= 1,
                Creator = "Feri",
                ThumbnailPath = "https://http.cat/103.jpg",
                FilePath = "",
                Comments = new(){ new CommentDTO { Text = "Bela", Id= 1}},
            },
        };

        data.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(list));

        CaffController controller = new CaffController(caff.Object, data.Object, logger, mapper);

        var result = await controller.GetAllAsync();

        result.Result.Should().NotBeNull();

        result.Result.Should().BeAssignableTo<OkObjectResult>();

        var value = result.Value;

        value.Should().NotBeNull();

        value.Should().NotHaveCount(list.Count);
        value.Should().OnlyContain(x => list.Any(l => l.Id == x.id));
    }

    [Fact]
    public async Task Test2()
    {
        var caff = new Mock<ICaffService>();
        var data = new Mock<IDataService>();
        var mapper = getMapper();
        var logger = new LoggerConfiguration().CreateBootstrapLogger();
        data.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(new List<CaffDTO>()));
        var controller = new CaffController(caff.Object, data.Object, logger, mapper);

        var response = await controller.GetAllAsync();

        response.Result.Should().NotBeNull();
        response.Result.Should().BeAssignableTo<OkObjectResult>();

        var value = response.Value;

        value.Should().NotBeNull();
        value.Should().HaveCountLessThan(1);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(15)]
    [InlineData(int.MaxValue)]
    public async Task Test3(int id)
    {
        var caff = new Mock<ICaffService>();
        var data = new Mock<IDataService>();
        var mapper = getMapper();
        var logger = new LoggerConfiguration().CreateBootstrapLogger();

        var caffDto = new CaffDTO()
        {
            Id = id,
            Creator = "ExpectedCreator",
        };

        data.Setup(x => x.GetCaffAsync(It.Is<int>(i => i == id))).Returns(Task.FromResult(caffDto));
        data.Setup(x => x.GetCaffAsync(It.Is<int>(i => i != id))).Throws<Exception>();

        var controller = new CaffController(caff.Object, data.Object, logger, mapper);

        var result = await controller.GetOne(id);

        result.Should().NotBeNull();
        result.Result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<OkObjectResult>();

        var actual = result.Value;

        actual.Should().NotBeNull();
        actual.creator.Should().Be(caffDto.Creator);
        actual.id.Should().Be(id);
    }


    private static IMapper getMapper()
    {
        var config = new MapperConfiguration(x => x.AddProfile<ApiProfile>());

        return config.CreateMapper();
    }
}
