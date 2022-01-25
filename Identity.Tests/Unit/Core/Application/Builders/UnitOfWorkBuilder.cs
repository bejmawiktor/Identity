using Identity.Core.Application;
using Moq;

namespace Identity.Tests.Unit.Core.Application.Builders
{
    internal class UnitOfWorkBuilder
    {
        public static readonly IUnitOfWork DefaultUnitOfWork = new UnitOfWorkBuilder().Build();

        public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
            = new Mock<IUnitOfWork>();
        public IApplicationsRepository ApplicationsRepository { get; private set; }
            = new Mock<IApplicationsRepository>().Object;
        public IAuthorizationCodesRepository AuthorizationCodesRepository { get; private set; }
            = new Mock<IAuthorizationCodesRepository>().Object;
        public IUsersRepository UsersRepository { get; private set; }
            = new Mock<IUsersRepository>().Object;
        public IRefreshTokensRepository RefreshTokensRepository { get; private set; }
            = new Mock<IRefreshTokensRepository>().Object;
        public IPermissionsRepository PermissionsRepository { get; private set; }
            = new Mock<IPermissionsRepository>().Object;
        public IResourcesRepository ResourcesRepository { get; private set; }
            = new Mock<IResourcesRepository>().Object;
        public IRolesRepository RolesRepository { get; private set; }
            = new Mock<IRolesRepository>().Object;

        public UnitOfWorkBuilder WithUnitOfWorkMock(Mock<IUnitOfWork> unitOfWorkMock)
        {
            this.UnitOfWorkMock = unitOfWorkMock;

            return this;
        }

        public UnitOfWorkBuilder WithApplicationsRepository(
            IApplicationsRepository applicationsRepository)
        {
            this.ApplicationsRepository = applicationsRepository;

            return this;
        }

        public UnitOfWorkBuilder WithAuthorizationCodesRepository(
            IAuthorizationCodesRepository authorizationCodesRepository)
        {
            this.AuthorizationCodesRepository = authorizationCodesRepository;

            return this;
        }

        public UnitOfWorkBuilder WithUsersRepository(
            IUsersRepository usersRepository)
        {
            this.UsersRepository = usersRepository;

            return this;
        }

        public UnitOfWorkBuilder WithRefreshTokensRepository(
            IRefreshTokensRepository refreshTokensRepository)
        {
            this.RefreshTokensRepository = refreshTokensRepository;

            return this;
        }

        public UnitOfWorkBuilder WithPermissionsRepository(
            IPermissionsRepository permissionsRepository)
        {
            this.PermissionsRepository = permissionsRepository;

            return this;
        }

        public UnitOfWorkBuilder WithResourcesRepository(
            IResourcesRepository resourcesRepository)
        {
            this.ResourcesRepository = resourcesRepository;

            return this;
        }

        public UnitOfWorkBuilder WithRolesRepository(
            IRolesRepository rolesRepository)
        {
            this.RolesRepository = rolesRepository;

            return this;
        }

        public IUnitOfWork Build()
        {
            this.UnitOfWorkMock
                .Setup(u => u.ApplicationsRepository)
                .Returns(this.ApplicationsRepository);
            this.UnitOfWorkMock
                .Setup(u => u.AuthorizationCodesRepository)
                .Returns(this.AuthorizationCodesRepository);
            this.UnitOfWorkMock
                .Setup(u => u.PermissionsRepository)
                .Returns(this.PermissionsRepository);
            this.UnitOfWorkMock
                .Setup(u => u.RefreshTokensRepository)
                .Returns(this.RefreshTokensRepository);
            this.UnitOfWorkMock
                .Setup(u => u.ResourcesRepository)
                .Returns(this.ResourcesRepository);
            this.UnitOfWorkMock
                .Setup(u => u.RolesRepository)
                .Returns(this.RolesRepository);
            this.UnitOfWorkMock
                .Setup(u => u.UsersRepository)
                .Returns(this.UsersRepository);

            return this.UnitOfWorkMock.Object;
        }
    }
}