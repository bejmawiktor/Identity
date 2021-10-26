using DDD.Application.Persistence.Adapters;
using System;

namespace Identity.Application
{
    using IAsyncApplicationsRepositoryAdapter = IAsyncRepositoryAdapter<ApplicationDto, Guid, IApplicationsRepository, ApplicationDtoConverter, Identity.Domain.Application, Identity.Domain.ApplicationId>;

    internal class ApplicationsRepositoryAdapter : IAsyncApplicationsRepositoryAdapter, Domain.IApplicationsRepository
    {
        public IApplicationsRepository ApplicationsRepository { get; }

        IApplicationsRepository IAsyncApplicationsRepositoryAdapter.DtoRepository
            => this.ApplicationsRepository;

        public ApplicationsRepositoryAdapter(IApplicationsRepository applicationsRepository)
        {
            this.ApplicationsRepository = applicationsRepository
                ?? throw new ArgumentNullException(nameof(applicationsRepository));
        }
    }
}