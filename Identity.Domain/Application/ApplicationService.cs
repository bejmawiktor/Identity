using DDD.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class ApplicationService
    {
        public IUnitOfWork UnitOfWork { get; }

        public ApplicationService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork 
                ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task CreateApplicationAsync(UserId userId, string name, Url homepageUrl, Url callbackUrl)
        {
            using(var eventsScope = new EventsScope())
            {
                User user = await this.UnitOfWork.UsersRepository.GetAsync(userId);

                if(user == null)
                {
                    throw new UserNotFoundException(userId);
                }

                Application application = user.CreateApplication(name, homepageUrl, callbackUrl);

                await this.UnitOfWork.ApplicationsRepository.AddAsync(application);

                eventsScope.Publish();
            }
        }
    }
}