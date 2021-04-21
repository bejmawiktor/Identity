using Identity.Domain;

namespace Identity.Application
{
    public static class Permissions
    {
        private readonly static PermissionDtoConverter Converter = new PermissionDtoConverter();

        private readonly static PermissionId CreateResourceId
            = new PermissionId(
                resourceId: new ResourceId("Identity"),
                name: "CreateResource");

        internal readonly static Permission CreateResource
            = new Permission(
                id: CreateResourceId,
                description: "It allows to create new resources.");

        public readonly static PermissionDto CreateResourceDto
            = Permissions.Converter.ToDto(Permissions.CreateResource);
    }
}