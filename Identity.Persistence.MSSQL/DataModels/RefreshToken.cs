using Identity.Application;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal class RefreshToken
    {
        public string Id { get; set; }
        public bool Used { get; set; }

        public RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            this.SetFields(refreshTokenDto);
        }

        public RefreshToken()
        {
        }

        public void SetFields(RefreshTokenDto refreshTokenDto)
        {
            this.Id = refreshTokenDto.Id;
            this.Used = refreshTokenDto.Used;
        }

        public RefreshTokenDto ToDto()
            => new RefreshTokenDto(this.Id, this.Used);
    }
}
