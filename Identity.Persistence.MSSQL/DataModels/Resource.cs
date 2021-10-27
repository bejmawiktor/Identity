using Identity.Application;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal class Resource
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public Resource(ResourceDto resourceDto)
        {
            this.SetFields(resourceDto);
        }

        public Resource()
        {
        }

        public void SetFields(ResourceDto resourceDto)
        {
            this.Id = resourceDto.Id;
            this.Description = resourceDto.Description;
        }

        public ResourceDto ToDto()
            => new ResourceDto(this.Id, this.Description);
    }
}