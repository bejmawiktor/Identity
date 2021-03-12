using System;

namespace Identity.Domain
{
    public class AuthorizationService
    {
        public IUsersRepository UsersRepository { get; }
        public IRolesRepository RolesRepository { get; }

        public AuthorizationService(IUsersRepository usersRepository, IRolesRepository rolesRepository)
        {
            this.UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.RolesRepository = rolesRepository ?? throw new ArgumentNullException(nameof(rolesRepository));
        }

        public bool CheckUserAccess(UserId userId, PermissionId permissionId)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if(permissionId == null)
            {
                throw new ArgumentNullException(nameof(permissionId));
            }

            User user = this.UsersRepository.Get(userId);

            if(user == null)
            {
                throw new UserNotFoundException(userId);
            }

            if(user.IsPermittedTo(permissionId))
            {
                return true;
            }

            foreach(var roleId in user.Roles)
            {
                Role role = this.RolesRepository.Get(roleId);

                if(role.IsPermittedTo(permissionId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}