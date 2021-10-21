using System;

namespace Identity.Domain
{
    public class AuthenticationService
    {
        private IUsersRepository UsersRepository { get; set; }

        public AuthenticationService(IUsersRepository usersRepository)
        {
            this.UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public User Authenticate(EmailAddress emailAddress, Password password)
        {
            User user = this.UsersRepository.Get(emailAddress);

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
