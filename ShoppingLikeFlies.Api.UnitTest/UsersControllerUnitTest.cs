using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFiles.DomainServices.DTOs;
using ShoppingLikeFlies.Api.Configuration;
using ShoppingLikeFlies.Api.Contracts.Incoming;
using ShoppingLikeFlies.Api.Contracts.Response;
using ShoppingLikeFlies.Api.Security.DAL;
using ShoppingLikeFlies.Api.Security.Validators;
using System.Diagnostics.Contracts;

namespace ShoppingLikeFlies.Api.UnitTest;
public class UsersControllerUnitTest
{
    [Fact]
    public async Task Test1()
    {
        var um = TestHelper.initUserManager();
        var logger = new LoggerConfiguration().CreateBootstrapLogger();
        var validator = new  RegistrationValidator(); ;
        var data = new List<ApplicationUser>
        {
            new ApplicationUser()
            {
                Id = "9245fe4a-d402-451c-b9ed-9c1a04247482",
                Email = "test_name",
                EmailConfirmed = true,
                UserName = "test_name",
                LastName = "test_name",
                FirstName = "test_name",
            },
        };

        var resultData = data.Select(x =>
            new UserResponse(Guid.Parse(x.Id), x.UserName, x.FirstName, x.LastName, false)
            ).ToList();

        um.Setup(e => e.Users.ToList()).Returns(data);

        var controller = new UsersController(logger, um.Object, validator);
        var result = (await controller.OnGetAsync()).Value;
        result.Should().NotBeNull();

        for (int i = 0; i < data.Count; i++)
        {
            result[i].Should().BeEquivalentTo(resultData[i]);
        }
    }


}

