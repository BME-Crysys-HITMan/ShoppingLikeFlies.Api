using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFiles.DomainServices.DTOs;
using ShoppingLikeFlies.Api.Configuration;
using ShoppingLikeFlies.Api.Contracts.Response;
using ShoppingLikeFlies.Api.Security.DAL;
using System.Diagnostics.Contracts;

namespace ShoppingLikeFlies.Api.UnitTest;
public class UsersControllerUnitTest
{
    [Fact]
    public async Task Test1()
    {
        var um = TestHelper.initUserManager();
        var logger = new LoggerConfiguration().CreateBootstrapLogger();

        var data = new List<ApplicationUser>
        {
            new ApplicationUser()
            {
                Id = "123",
                Email = "test_name",
                EmailConfirmed = true,
                UserName = "test_name",
                LastName = "test_name",
                FirstName = "test_name",
            },
        };

        var resultData = data.ConvertAll(x =>
            new UserResponse(Guid.Parse(x.Id), x.UserName, x.FirstName, x.LastName, false)
            );

        um.Setup(e => e.Users.ToList()).Returns(data);

        var controller = new UsersController();
        var result = (await controller.OnGetAsync()).Value;
        result.Should().NotBeNull();

        for (int i = 0; i < data.Count; i++)
        {
            result[i].Should().BeEquivalentTo(resultData[i]);
        }
    }


}

