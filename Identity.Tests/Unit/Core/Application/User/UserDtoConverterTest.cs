using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class UserDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenUserGiven_ThenUserDtoIsReturned()
        {
            User user = UserBuilder.DefaultUser;
            UserDtoConverter userDtoConverter = new();

            UserDto userDto = userDtoConverter.ToDto(user);

            Assert.Multiple(() =>
            {
                Assert.That(userDto.Id, Is.EqualTo(user.Id.ToGuid()));
                Assert.That(userDto.Email, Is.EqualTo(user.Email.ToString()));
                Assert.That(userDto.HashedPassword, Is.EqualTo(user.Password.ToString()));
            });
        }

        [Test]
        public void TestToDto_WhenNullUserGiven_ThenArgumentNullExceptionIsThrown()
        {
            UserDtoConverter userDtoConverter = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("user"),
                () => userDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenUserIdGiven_ThenDtoIdentifierIsReturned()
        {
            UserId userId = new(Guid.NewGuid());
            UserDtoConverter userDtoConverter = new();

            Guid userDtoIdentifier = userDtoConverter.ToDtoIdentifier(userId);

            Assert.That(userDtoIdentifier, Is.EqualTo(userId.ToGuid()));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            UserDtoConverter userDtoConverter = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("userId"),
                () => userDtoConverter.ToDtoIdentifier(null));
        }
    }
}