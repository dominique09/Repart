using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Repart.Common.Auth;
using Repart.Common.Exceptions;
using Repart.Services.Identity.Domain.Models;
using Repart.Services.Identity.Domain.Repositories;
using Repart.Services.Identity.Domain.Services;
using Repart.Services.Identity.Services;
using Xunit;

namespace Repart.Services.Identity.Tests.Unit
{
    public class UnitServiceTests
    {
        #region Login Tests
        
        [Fact]
        public async Task user_service_login_should_return_jwt()
        {
            var email = "test@test.com";
            var password = "secret";
            var name = "test";
            var salt = "salt";
            var hash = "hash";
            var token = "token";
            var role = new Role("TEST_ROLE");
            var userRolesGuid = new List<Guid>() {role.Id};
            var userRolesName = new List<string>() {role.Name};
            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var encrypterMock = new Mock<IEncrypter>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            roleRepositoryMock.Setup(x => x.GetAsync(role.Id)).ReturnsAsync(role);
            encrypterMock.Setup(x => x.GetSalt()).Returns(salt);
            encrypterMock.Setup(x => x.GetHash(password, salt)).Returns(hash);
            jwtHandlerMock.Setup(x => x.Create(It.IsAny<Guid>(), userRolesName)).Returns(new JsonWebToken
            {
                Token = token
            });

            var user = new User(email, name, userRolesGuid);
            user.SetPassword(password, encrypterMock.Object);
            userRepositoryMock.Setup(x => x.GetAsync(email)).ReturnsAsync(user);

            var userService = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object,
                encrypterMock.Object, jwtHandlerMock.Object);

            var jwt = await userService.LoginAsync(email, password);
            userRepositoryMock.Verify(x => x.GetAsync(email), Times.Once);
            jwtHandlerMock.Verify(x => x.Create(It.IsAny<Guid>(), userRolesName), Times.Once);
            jwt.Should().NotBeNull();
            jwt.Token.Should().BeEquivalentTo(token);
        }

        [Fact]
        public async Task user_service_login_async_when_invalid_email_should_throw_repart_exception()
        {
            var email = "test@test.com";
            var invalidEmail = "invalid@test.com";
            var password = "secret";
            var name = "test";
            var salt = "salt";
            var hash = "hash";
            var token = "token";
            var role = new Role("TEST_ROLE");
            var userRolesGuid = new List<Guid>() {role.Id};
            var userRolesName = new List<string>() {role.Name};
            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var encrypterMock = new Mock<IEncrypter>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            roleRepositoryMock.Setup(x => x.GetAsync(role.Id)).ReturnsAsync(role);
            encrypterMock.Setup(x => x.GetSalt()).Returns(salt);
            encrypterMock.Setup(x => x.GetHash(password, salt)).Returns(hash);
            jwtHandlerMock.Setup(x => x.Create(It.IsAny<Guid>(), userRolesName)).Returns(new JsonWebToken
            {
                Token = token
            });

            var user = new User(email, name, userRolesGuid);
            user.SetPassword(password, encrypterMock.Object);
            userRepositoryMock.Setup(x => x.GetAsync(email)).ReturnsAsync(user);

            var userService = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object,
                encrypterMock.Object, jwtHandlerMock.Object);

            var ex = await Assert.ThrowsAsync<RepartException>(() => userService.LoginAsync(invalidEmail, password));
            ex.Code.Should().BeEquivalentTo("invalid_credentials");
        }

        [Fact]
        public async Task user_service_login_async_when_invalid_password_should_throw_repart_exception()
        {
            var email = "test@test.com";
            var password = "secret";
            var invalidPassword = "invalid";
            var name = "test";
            var salt = "salt";
            var hash = "hash";
            var token = "token";
            var role = new Role("TEST_ROLE");
            var userRolesGuid = new List<Guid>() {role.Id};
            var userRolesName = new List<string>() {role.Name};
            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var encrypterMock = new Mock<IEncrypter>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            roleRepositoryMock.Setup(x => x.GetAsync(role.Id)).ReturnsAsync(role);
            encrypterMock.Setup(x => x.GetSalt()).Returns(salt);
            encrypterMock.Setup(x => x.GetHash(password, salt)).Returns(hash);
            jwtHandlerMock.Setup(x => x.Create(It.IsAny<Guid>(), userRolesName)).Returns(new JsonWebToken
            {
                Token = token
            });

            var user = new User(email, name, userRolesGuid);
            user.SetPassword(password, encrypterMock.Object);
            userRepositoryMock.Setup(x => x.GetAsync(email)).ReturnsAsync(user);

            var userService = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object,
                encrypterMock.Object, jwtHandlerMock.Object);

            var ex = await Assert.ThrowsAsync<RepartException>(() => userService.LoginAsync(email, invalidPassword));
            ex.Code.Should().BeEquivalentTo("invalid_credentials");
        }
        
        #endregion

        #region Register Tests
        [Fact]
        public async Task user_service_register_async_should_return_user()
        {
            var email = "test@test.com";
            var password = "secret";
            var name = "test";
            var salt = "salt";
            var hash = "hash";
            var role = new Role("TEST_ROLE");
            var userRolesGuid = new List<Guid>() {role.Id};

            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var encrypterMock = new Mock<IEncrypter>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            roleRepositoryMock.Setup(x => x.GetAsync(role.Id)).ReturnsAsync(role);
            encrypterMock.Setup(x => x.GetSalt()).Returns(salt);
            encrypterMock.Setup(x => x.GetHash(password, salt)).Returns(hash);

            var user = new User(email, name, userRolesGuid);
            user.SetPassword(password, encrypterMock.Object);
            userRepositoryMock.Setup(x => x.GetAsync(email)).ReturnsAsync((User)null);

            var userService = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object,
                encrypterMock.Object, jwtHandlerMock.Object);

            var result = await userService.RegisterAsync(email, password, name, userRolesGuid);
            result.Should().BeEquivalentTo(user, 
                x => x
                    .Excluding(u => u.Id)
                    .Excluding(u => u.Roles)
                    .Excluding(u => u.Password)
                    .Excluding(u => u.Salt)
                    .Excluding(u => u.CreatedAt));
            result.Roles.First().Should().BeEquivalentTo(role);
        }

        [Fact]
        public async Task user_service_register_async_when_email_exist_should_throw_repart_exception()
        {
            var email = "test@test.com";
            var password = "secret";
            var name = "test";
            var salt = "salt";
            var hash = "hash";
            var role = new Role("TEST_ROLE");
            var userRolesGuid = new List<Guid>() {role.Id};

            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var encrypterMock = new Mock<IEncrypter>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            roleRepositoryMock.Setup(x => x.GetAsync(role.Id)).ReturnsAsync(role);
            encrypterMock.Setup(x => x.GetSalt()).Returns(salt);
            encrypterMock.Setup(x => x.GetHash(password, salt)).Returns(hash);

            var user = new User(email, name, userRolesGuid);
            user.SetPassword(password, encrypterMock.Object);
            userRepositoryMock.Setup(x => x.GetAsync(email)).ReturnsAsync((User)null);

            var userService = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object,
                encrypterMock.Object, jwtHandlerMock.Object);
            
            var ex = await Assert.ThrowsAsync<RepartException>(() => 
                userService.RegisterAsync(email, password, name, userRolesGuid));
            ex.Code.Should().BeEquivalentTo("user_already_exist");
        }
        #endregion

        #region AddToRole Tests

        [Fact]
        public async Task add_role_async_should_return_user()
        {

        }

        #endregion
    }
}
