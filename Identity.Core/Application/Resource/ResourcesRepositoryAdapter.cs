using DDD.Application.Persistence.Adapters;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Application
{
    using IAsyncResourcesRepositoryAdapter = IAsyncRepositoryAdapter<ResourceDto, string, IResourcesRepository, ResourceDtoConverter, Resource, ResourceId>;

    internal class ResourcesRepositoryAdapter
    : IAsyncResourcesRepositoryAdapter, Domain.IResourcesRepository
    {
        public IResourcesRepository ResourcesRepository { get; }

        IResourcesRepository IAsyncResourcesRepositoryAdapter.DtoRepository
            => this.ResourcesRepository;

        public ResourcesRepositoryAdapter(IResourcesRepository resourcesRepository)
        {
            this.ResourcesRepository = resourcesRepository
                ?? throw new ArgumentNullException(nameof(resourcesRepository));
        }
    }
}