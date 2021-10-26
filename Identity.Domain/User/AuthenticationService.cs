using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class AuthenticationService
    {
        private IUsersRepository UsersRepository { get; set; }

        public AuthenticationService(IUsersRepository usersRepository)
        {
            this.UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public async Task<User> Authenticate(EmailAddress emailAddress, Password password)
        {
            User user = await this.UsersRepository.GetAsync(emailAddress);

            if (user == null)
            {
                return null;
            }

            PasswordVerificationResult passwordVerificationResult = user.Password.Verify(password);

            if(passwordVerificationResult == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }
    }
}
