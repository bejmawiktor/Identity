using DDD.Model;
using System;

namespace Identity.Domain
{
    public class UserId : Identifier<Guid, UserId>
    {
        public UserId(Guid value) : base(value)
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

        public static UserId Generate()
            => new UserId(Guid.NewGuid());
    }
}