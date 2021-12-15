using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class AuthenticationService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public AuthenticationService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork 
                ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<User> Authenticate(EmailAddress emailAddress, Password password)
        {
            User user = await this.UnitOfWork.UsersRepository.GetAsync(emailAddress);

            if(user == null)
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