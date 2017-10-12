using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ExpenseLink.Controllers;
using ExpenseLink.Models;
using ExpenseLink.Repository;
using ExpenseLink.Services;
using ExpenseLink.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExpenseLink.Tests.Controllers
{
    [TestClass]
    public class RequestControllerTest
    {

        #region Privatge method
        private Mock<IUserStore<ApplicationUser>> CreateMockUserManager(string testUserFullName)
        {
            var userManagerMock = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
            userManagerMock.As<IUserPasswordStore<ApplicationUser>>()
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser() { Id = "id", Name = testUserFullName });
            return userManagerMock;
        }

        private Mock<ControllerContext> CreateMockControlContext(string roleName, string testUserFullName)
        {
            var identity = new GenericIdentity(roleName, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, roleName);
            identity.AddClaim(nameIdentifierClaim);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.Identity.Name).Returns(testUserFullName);
            mockPrincipal.Setup(x => x.IsInRole(roleName)).Returns(true);

            var mockContext = new Mock<HttpContextBase>();
            mockContext.SetupGet(c => c.User).Returns(mockPrincipal.Object);

            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.SetupGet(c => c.HttpContext)
                .Returns(mockContext.Object);
            return mockControllerContext;
        }
        #endregion


        [TestMethod]
        public void New_WhenNewRequest_ShouldReturnNewRequestPage()
        {
            //Arrange
            const string roleName = RoleName.Employee;
            const string testUserFullName = "FirstName LastName";

            var mockUserManager = CreateMockUserManager(testUserFullName);
            var mockControllerContext = CreateMockControlContext(roleName, testUserFullName);

            //Act
            RequestController requestController = new RequestController(new RequestRepository(new Mock<ApplicationDbContext>().Object),
                new UserManager<ApplicationUser>(mockUserManager.Object),
                new Mock<IEmailService>().Object)
            {
                ControllerContext = mockControllerContext.Object
            };

            NewRequestViewModel newRequestViewModel = new NewRequestViewModel();//{Requester = "Test Requester"};

            ViewResult result = (ViewResult) requestController.New(newRequestViewModel);
            var model = result.Model as NewRequestViewModel;

            //Assert
            Assert.IsTrue(model.Requester == testUserFullName);
        }

        [TestMethod]
        public void Create_WhenCreate_SendEmailIsCalled()
        {
            //Arrange
            //Arrange
            const string roleName = RoleName.Employee;
            const string testUserFullName = "FirstName LastName";

            var mockUserManager = CreateMockUserManager(testUserFullName);
            var mockControllerContext = CreateMockControlContext(roleName, testUserFullName);
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(m=>m.Send(It.IsAny<MailMessage>())).Verifiable();

            var mockRepository = new Mock<IRequestRepository>();
            //mockRepository.VerifySet(m=>m.);
            //Act
            RequestController requestController = new RequestController(mockRepository.Object,
                new UserManager<ApplicationUser>(mockUserManager.Object),
                mockEmailService.Object)
            {
                ControllerContext = mockControllerContext.Object
            };
            NewRequestViewModel viewModel = new NewRequestViewModel();
            requestController.Create(viewModel);

            //Assert
            mockEmailService.Verify(m=>m.Send(It.IsAny<MailMessage>()), Times.Once);
        }

        [TestMethod]
        public void Create_WhenCreate_AddRequestIsCalled()
        {
            //Arrange
            //Arrange
            const string roleName = RoleName.Employee;
            const string testUserFullName = "FirstName LastName";

            var mockUserManager = CreateMockUserManager(testUserFullName);
            var mockControllerContext = CreateMockControlContext(roleName, testUserFullName);
            var mockEmailService = new Mock<IEmailService>();
            
            var mockRepository = new Mock<IRequestRepository>();
            mockRepository.Setup(m => m.AddRequest(It.IsAny<Request>())).Verifiable();

            //Act
            RequestController requestController = new RequestController(mockRepository.Object,
                new UserManager<ApplicationUser>(mockUserManager.Object),
                mockEmailService.Object)
            {
                ControllerContext = mockControllerContext.Object
            };
            NewRequestViewModel viewModel = new NewRequestViewModel();
            requestController.Create(viewModel);

            //Assert
            mockRepository.Verify(m => m.AddRequest(It.IsAny<Request>()), Times.Once);
        }

        [TestMethod]
        public void Index_WhenNotLoggedIn_ShouldReturnTypeHttpUnauthorizedResult()
        {
            //Arrange
            var userManagerMock = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
            RequestController requestController = new RequestController(new RequestRepository(new Mock<ApplicationDbContext>().Object),
                new UserManager<ApplicationUser>(userManagerMock.Object),
                new Mock<IEmailService>().Object);

            //Act
            var result = requestController.Index();

            //Assert           
            Assert.AreEqual(result.GetType(), typeof(HttpUnauthorizedResult));
        }

    }
}