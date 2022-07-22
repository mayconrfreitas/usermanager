using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Users.API.Controllers;
using Users.API.Data;
using Users.API.Entities;
using Users.API.Repositories;
using Xunit;

namespace UsersAPI.Test
{
    public class UsersTest
    {
        private readonly IConfiguration _configuration;
        private UsersController _controller;
        private IUserRepository _repository;
        private IUserContext _context;

        public UsersTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .AddEnvironmentVariables()
                .Build();

            _context = new UserContext(_configuration);
            _repository = new UserRepository(_context);
            _controller = new UsersController(_repository);
        }


        [Fact]
        public async void GetUsersTest()
        {
            //Arrange


            //Act
            var result = await _controller.GetUsers();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

            OkObjectResult list = result.Result as OkObjectResult;
            Assert.IsType<List<User>>(list.Value);

            //List<User> listUsers = list.Value as List<User>;
            //Assert.Equal(3, listUsers.Count);
        }



        [Fact]
        public async void GetActiveUsersTest()
        {
            //Arrange


            //Act
            var result = await _controller.GetActiveUsers();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

            OkObjectResult list = result.Result as OkObjectResult;
            Assert.IsType<List<User>>(list.Value);

            //List<User> listUsers = list.Value as List<User>;
            //Assert.Equal(3, listUsers.Count);
        }



        [Theory]
        [InlineData("62d9f68de51a517ab820abab", "ffffffffffffffffffffffff")]
        public async void GetUserByIdTest(string validId, string invalidId)
        {
            //Arrange

            //Act
            var notFoundResult = await _controller.GetUserById(invalidId);
            var okResult = await _controller.GetUserById(validId);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
            Assert.IsType<OkObjectResult>(okResult.Result);

            OkObjectResult item = okResult.Result as OkObjectResult;
            Assert.IsType<User>(item.Value);

            User user = item.Value as User;
            Assert.Equal(validId, user.Id);
            Assert.Equal("Maycon Freitas", user.Name);
        }



        [Fact]
        public async void CreateUserTest()
        {
            //Arrange
            User completeUser = new User()
            {
                Name = "Albert Einstein",
                BirthDate = "1879-03-14"
            };

            //Act
            var createdResponse = await _controller.CreateUser(completeUser);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse.Result);

            CreatedAtRouteResult item = createdResponse.Result as CreatedAtRouteResult;
            Assert.IsType<User>(item.Value);

            User user = item.Value as User;
            Assert.Equal(completeUser.Name, user.Name);
            Assert.Equal(completeUser.BirthDate, user.BirthDate);



            //Arrange
            User noNameUser = new User()
            {
                BirthDate = "1879-03-14"
            };

            //Act
            _controller.ModelState.AddModelError("Name", "Name is a required field");
            var noNameBadResponse = await _controller.CreateUser(noNameUser);

            //Assert
            Assert.IsType<BadRequestObjectResult>(noNameBadResponse.Result);



            //Arrange
            User noBirthDateUser = new User()
            {
                Name = "Albert Einstein"
            };

            //Act
            _controller.ModelState.AddModelError("BirthDate", "BirthDate is a required field");
            var noBirthBadResponse = await _controller.CreateUser(noBirthDateUser);

            //Assert
            Assert.IsType<BadRequestObjectResult>(noBirthBadResponse.Result);



            //Arrange
            User wrongBirthDateFormatUser = new User()
            {
                Name = "Albert Einstein",
                BirthDate = "14/03/1879"
            };

            //Act
            _controller.ModelState.AddModelError("BirthDate", "Date is in the wrong format. Please use the format: yyyy-MM-dd");
            var wrongBirthBadResponse = await _controller.CreateUser(wrongBirthDateFormatUser);

            //Assert
            Assert.IsType<BadRequestObjectResult>(wrongBirthBadResponse.Result);
        }




        [Theory]
        [InlineData("62d9f68de51a517ab820abab", "ffffffffffffffffffffffff")]
        public async void UpdateUserStateTest(string validId, string invalidId)
        {
            //Arrange
            var userResult = await _controller.GetUserById(validId);
            OkObjectResult userItem = userResult.Result as OkObjectResult;
            User userFound = userItem.Value as User;

            bool previousState = userFound.Active;

            //Act
            var notFoundResult = await _controller.UpdateUserState(invalidId);
            var okResult = await _controller.UpdateUserState(validId);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<OkObjectResult>(okResult);

            OkObjectResult item = okResult as OkObjectResult;
            Assert.IsType<User>(item.Value);

            User user = item.Value as User;
            Assert.Equal(validId, user.Id);
            Assert.NotEqual(previousState, user.Active);
        }



        [Theory]
        [InlineData("62d9f68de51a517ab820abad", "ffffffffffffffffffffffff")]
        public async void DeleteUserTest(string validId, string invalidId)
        {
            //Arrange
            var usersResult = await _controller.GetUsers();
            OkObjectResult usersItems = usersResult.Result as OkObjectResult;
            List<User> usersFound = usersItems.Value as List<User>;

            int qntUsers = usersFound.Count;

            //Act
            var notFoundResult = await _controller.DeleteUserById(invalidId);
            var okResult = await _controller.DeleteUserById(validId);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<OkObjectResult>(okResult);

            var usersNewResult = await _controller.GetUsers();
            OkObjectResult usersNewItems = usersNewResult.Result as OkObjectResult;
            List<User> usersNewFound = usersNewItems.Value as List<User>;

            int qntNewUsers = usersNewFound.Count;

            Assert.Equal(qntUsers - 1, qntNewUsers);
        }


    }
}
