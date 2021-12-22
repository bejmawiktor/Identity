using Identity.Core.Domain;

namespace Identity.Core.Application
{
    public static class Permissions
    {
        private static readonly PermissionDtoConverter Converter = new PermissionDtoConverter();

        private static readonly PermissionId CreateResourceId
            = new PermissionId(
                resourceId: new ResourceId("Identity"),
                name: "CreateResource");

        internal static readonly Permission CreateResource
            = new Permission(
                id: CreateResourceId,
                description: "It allows to create new resources.");

        public static readonly PermissionDto CreateResourceDto
            = Permissions.Converter.ToDto(Permissions.CreateResource);
    }
}