using Liebra_Permana.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Liebra_Permana.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Tr_QRCode> Tr_QRCode => Set<Tr_QRCode>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("dbo");
        builder.Entity<IdentityUser>(entity => { entity.ToTable(name: "Tb_Users"); });
        builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Tb_Roles"); });
        builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("Tb_UserRoles"); });
        builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("Tb_UserClaims"); });
        builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("Tb_UserLogins"); });
        builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("Tb_RoleClaims"); });
        builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("Tb_UserTokens"); });
    }
}