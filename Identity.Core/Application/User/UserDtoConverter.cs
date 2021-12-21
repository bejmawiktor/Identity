using DDD.Application.Model.Converters;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Application
{
    internal class UserDtoConverter : IAggregateRootDtoConverter<User, UserId, UserDto, Guid>
    {
        public UserDto ToDto(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return new UserDto(
                id: user.Id.ToGuid(),
                email: user.Email.ToString(),
                hashedPassword: user.Password.ToString());
        }

        public Guid ToDtoIdentifier(UserId userId)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return userId.ToGuid();
        }
    }
}