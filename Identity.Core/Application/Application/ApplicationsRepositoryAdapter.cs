using DDD.Application.Persistence.Adapters;
using System;

namespace Identity.Core.Application
{
    using IAsyncApplicationsRepositoryAdapter = IAsyncRepositoryAdapter<ApplicationDto, Guid, IApplicationsRepository, ApplicationDtoConverter, Identity.Core.Domain.Application, Identity.Core.Domain.ApplicationId>;

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