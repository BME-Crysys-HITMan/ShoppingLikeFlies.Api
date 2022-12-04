using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFlies.Api.Contracts.Incoming;
using ShoppingLikeFlies.Api.Security.DAL;
using ShoppingLikeFlies.Api.Security.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingLikeFlies.Api.UnitTest;

public class RegisterControllerUnitTest
{
    [Fact]
    public async Task Test1()
    {
        var mgr = TestHelper.initUserManager();

        var logger = new LoggerConfiguration().CreateBootstrapLogger();
        var validator = new RegistrationValidator();


        mgr.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

        var controller = new RegisterController(logger, mgr.Object, validator);
        var contract = new RegisterRequest("test_name", "test_name", "test_name" ,"Test1234!", "Test1234!");

        var result = await controller.OnPostAsync(contract);
        result.Should().NotBeOfType<BadRequestResult>();


       
        contract = new RegisterRequest("test_name", "test_name", "test_name", "Test134!", "test1234!");

        result = await controller.OnPostAsync(contract);
        result.Should().NotBeOfType<CreatedResult>();

    }
}

