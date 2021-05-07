using DDD.Domain.Model;
using System;

namespace Identity.Domain
{
    public class ApplicationId : Identifier<Guid, ApplicationId>
    {
        public ApplicationId(Guid value) : base(value)
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

        public static ApplicationId Generate()
            => new ApplicationId(Guid.NewGuid());

        public override string ToString()
            => this.Value.ToString();
    }
}