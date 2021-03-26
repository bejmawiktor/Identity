using Identity.Application;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record Permission
    {
        public string ResourceId { get; set; }
        public Resource Resource { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Permission(PermissionDto permissionDto)
        {
            this.ResourceId = permissionDto.Id.ResourceId;
            this.Name = permissionDto.Id.Name;
            this.Description = permissionDto.Description;
        }

        public Permission()
        {
        }

        public PermissionDto ToDto()
            => new PermissionDto(
                this.ResourceId,
                this.Name,
                this.Description);
    }
}