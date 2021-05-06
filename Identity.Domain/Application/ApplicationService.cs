﻿using DDD.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class ApplicationService
    {
        public IApplicationsRepository ApplicationsRepository { get; }
        public IUsersRepository UsersRepository { get; }

        public ApplicationService(
            IApplicationsRepository applicationsRepository,
            IUsersRepository usersRepository)
        {
            this.ApplicationsRepository = applicationsRepository
                ?? throw new ArgumentNullException(nameof(applicationsRepository));
            this.UsersRepository = usersRepository
                ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public void CreateApplication(UserId userId, string name, Url homepageUrl, Url callbackUrl)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                User user = this.UsersRepository.Get(userId);

                if(user == null)
                {
                    throw new UserNotFoundException(userId);
                }

                Application application = user.CreateApplication(name, homepageUrl, callbackUrl);

                this.ApplicationsRepository.Add(application);

                eventsScope.Publish();
            }
        }

        public async Task CreateApplicationAsync(UserId userId, string name, Url homepageUrl, Url callbackUrl)
        {
            using(EventsScope eventsScope = EventManager.Instance.CreateScope())
            {
                User user = await this.UsersRepository.GetAsync(userId);

                if(user == null)
                {
                    throw new UserNotFoundException(userId);
                }

                Application application = user.CreateApplication(name, homepageUrl, callbackUrl);

                await this.ApplicationsRepository.AddAsync(application);

                eventsScope.Publish();
            }
        }
    }
}