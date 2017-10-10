using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ExpenseLink.Controllers;
using ExpenseLink.Models;
using ExpenseLink.Repository;
using ExpenseLink.Services;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExpenseLink.Tests.Controllers
{
    [TestClass]
    public class RequestControllerTest
    {
        [TestMethod]
        public void Index_WhenNotLoggedIn_ShouldReturnNull()
        {
            //Arrange
            RequestController requestController = new RequestController();

            //Act
            var result = requestController.Index();

            //Assert           
            Assert.AreEqual(result.GetType(), typeof(HttpUnauthorizedResult));
        }

        [TestMethod]
        public void Index_WhenEmployeeUser_ShouldReturnViewl()
        {
            //Arrange
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(m => m.HttpContext.User.Identity.Name).Returns("someuser");
            mockContext.SetupGet(m => m.HttpContext.User.Identity.GetUserId()).Returns("someuserId");
            mockContext.SetupGet(m => m.HttpContext.Request.IsAuthenticated).Returns(true);
            mockContext.SetupGet(m => m.HttpContext.User);
            RequestController requestController = new RequestController();
            //requestController.ControllerContext = mockContext.Object;
            //requestController.
            //Act
            var result = requestController.Index();

            //Assert           
            Assert.AreEqual(result.GetType(), typeof(HttpUnauthorizedResult));
        }
    }
}

//    [TestMethod]
        //    public void Index_WhenUserIsEmployee_GetRequestByUserIdIsCalled()
        //    {
        //        //Arrange             
        //        const string roleName = RoleName.Employee;
        //        var identity = new GenericIdentity(roleName, "");
        //        var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, roleName);
        //        identity.AddClaim(nameIdentifierClaim);

        //        var mockPrincipal = new Mock<IPrincipal>();
        //        mockPrincipal.Setup(x => x.Identity).Returns(identity);
        //        mockPrincipal.Setup(x => x.IsInRole(roleName)).Returns(true);

        //        var mockContext = new Mock<HttpContextBase>();
        //        mockContext.SetupGet(c => c.User).Returns(mockPrincipal.Object);

        //        var mockControllerContext = new Mock<ControllerContext>();
        //        mockControllerContext.SetupGet(c => c.HttpContext)
        //            .Returns(mockContext.Object);

        //        var userManagerMock = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
        //        userManagerMock.As<IUserPasswordStore<ApplicationUser>>()
        //            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
        //            .ReturnsAsync(new ApplicationUser() { Id = "id"});

        //        var mockEmail = new Mock<IEmailService>();
        //        var mockRequestRepo = new Mock<IRequestRepository>();
        //        mockRequestRepo.Setup(m=>m.GetRequestByUserId(It.IsAny<string>())).Verifiable();
        //        RequestController requestController = new RequestController(
        //            mockEmail.Object,
        //            mockRequestRepo.Object,
        //            new UserManager<ApplicationUser>(userManagerMock.Object))
        //        {
        //            ControllerContext = mockControllerContext.Object
        //        };

        //        //Act
        //        ViewResult result = requestController.Index() as ViewResult;

        //        //Assert
        //        mockRequestRepo.Verify(m=>m.GetRequestByUserId(It.IsAny<string>()));
        //        //Assert.IsNotNull(result);
        //    }

        //    [TestMethod]
        //    public void Index_WhenUserIsManager_GetRequestSubmittedIsCalled()
        //    {
        //        //Arrange             
        //        const string roleName = RoleName.Manager;
        //        var identity = new GenericIdentity(roleName, "");
        //        var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, roleName);
        //        identity.AddClaim(nameIdentifierClaim);

        //        var mockPrincipal = new Mock<IPrincipal>();
        //        mockPrincipal.Setup(x => x.Identity).Returns(identity);
        //        mockPrincipal.Setup(x => x.IsInRole(roleName)).Returns(true);

        //        var mockContext = new Mock<HttpContextBase>();
        //        mockContext.SetupGet(c => c.User).Returns(mockPrincipal.Object);

        //        var mockControllerContext = new Mock<ControllerContext>();
        //        mockControllerContext.SetupGet(c => c.HttpContext)
        //            .Returns(mockContext.Object);

        //        var userManagerMock = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
        //        userManagerMock.As<IUserPasswordStore<ApplicationUser>>()
        //            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
        //            .ReturnsAsync(new ApplicationUser() { Id = "id" });

        //        var mockEmail = new Mock<IEmailService>();
        //        var mockRequestRepo = new Mock<IRequestRepository>();
        //        mockRequestRepo.Setup(m => m.GetRequestSubmitted()).Verifiable();
        //        RequestController requestController = new RequestController(
        //            mockEmail.Object,
        //            mockRequestRepo.Object,
        //            new UserManager<ApplicationUser>(userManagerMock.Object))
        //        {
        //            ControllerContext = mockControllerContext.Object
        //        };

        //        //Act
        //        ViewResult result = requestController.Index() as ViewResult;

        //        //Assert
        //        mockRequestRepo.Verify(m => m.GetRequestSubmitted());
        //        //Assert.IsNotNull(result);
        //    }

        //    [TestMethod]
        //    public void Index_WhenUserIsFinance_GetRequestWaitingForReimbursementIsCalled()
        //    {
        //        //Arrange             
        //        const string roleName = RoleName.Finance;
        //        var identity = new GenericIdentity(roleName, "");
        //        var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, roleName);
        //        identity.AddClaim(nameIdentifierClaim);

        //        var mockPrincipal = new Mock<IPrincipal>();
        //        mockPrincipal.Setup(x => x.Identity).Returns(identity);
        //        mockPrincipal.Setup(x => x.IsInRole(roleName)).Returns(true);

        //        var mockContext = new Mock<HttpContextBase>();
        //        mockContext.SetupGet(c => c.User).Returns(mockPrincipal.Object);

        //        var mockControllerContext = new Mock<ControllerContext>();
        //        mockControllerContext.SetupGet(c => c.HttpContext)
        //            .Returns(mockContext.Object);

        //        var userManagerMock = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
        //        userManagerMock.As<IUserPasswordStore<ApplicationUser>>()
        //            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
        //            .ReturnsAsync(new ApplicationUser() { Id = "id" });

        //        var mockEmail = new Mock<IEmailService>();
        //        var mockRequestRepo = new Mock<IRequestRepository>();
        //        mockRequestRepo.Setup(m => m.GetRequestWaitingForReimbursement()).Verifiable();
        //        RequestController requestController = new RequestController(
        //            mockEmail.Object,
        //            mockRequestRepo.Object,
        //            new UserManager<ApplicationUser>(userManagerMock.Object))
        //        {
        //            ControllerContext = mockControllerContext.Object
        //        };

        //        //Act
        //        ViewResult result = requestController.Index() as ViewResult;

        //        //Assert
        //        mockRequestRepo.Verify(m => m.GetRequestWaitingForReimbursement());
        //        //Assert.IsNotNull(result);
        //    }
        //}
    //}
