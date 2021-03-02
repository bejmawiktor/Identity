using DDD.Domain.Model;
using System;

namespace Identity.Domain
{
    public class RoleId : Identifier<Guid, RoleId>
    {
        public RoleId(Guid value) : base(value)
        {
        }

        protected override void ValidateValue(Guid value)
        {
            if(value == Guid.Empty)
            {
                throw new ArgumentException("Guid can't be empty.");
            }
        }

        public Guid ToGuid()
            => this.Value;

        public static RoleId Generate()
            => new RoleId(Guid.NewGuid());
    }
}