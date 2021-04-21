using Identity.Application;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal class Resource
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public Resource(ResourceDto roleDto)
        {
            this.Id = roleDto.Id;
            this.Description = roleDto.Description;
        }

        public Resource()
        {
        }

        public ResourceDto ToDto()
            => new ResourceDto(this.Id, this.Description);
    }
}