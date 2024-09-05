using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Data.Mappings.Identity
{
    public class IdentityUserRoleMapping : IEntityTypeConfiguration<IdentityUserRole<long>>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IdentityUserRole<long>> builder)
        {
            builder.ToTable("IdentityUserRole");
            builder.HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}
