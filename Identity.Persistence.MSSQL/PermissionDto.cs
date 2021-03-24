using Identity.Application;

namespace Identity.Persistence.MSSQL
{
    internal record PermissionDto
    {
        public string ResourceId { get; set; }
        public ResourceDto Resource { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public PermissionDto(Identity.Application.PermissionDto permissionDto)
        {
            this.ResourceId = permissionDto.Id.ResourceId;
            this.Name = permissionDto.Id.Name;
            this.Description = permissionDto.Description;
        }

        public PermissionDto()
        {
        }

        public Identity.Application.PermissionDto ToApplicationDto()
            => new Identity.Application.PermissionDto(
                this.ResourceId,
                this.Name,
                this.Description);
    }
}