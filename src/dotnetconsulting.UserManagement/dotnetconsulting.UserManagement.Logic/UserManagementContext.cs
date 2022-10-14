// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.EntityFrameworkCore;

namespace dotnetconsulting.UserManagement.Logic.EntityFramework;

public partial class UserManagementContext
{
#pragma warning disable CA1822 // Mark members as static
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
#pragma warning restore CA1822 // Mark members as static
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "dbo");

            entity.HasKey(e => e.ObjectId);

            entity.HasAlternateKey(k => k.Name);

            entity.Property(p => p.Name).HasConversion(c => c.ToLower(), c => c.ToLower());
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles", "dbo");

            entity.HasKey(e => e.ObjectId);

            entity.Property(p => p.Name).HasColumnName("Rolename");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles", "dbo");

            entity.HasKey(e => e.ObjectId);
        });

        modelBuilder.Entity<UserPlantCode>(entity =>
        {
            entity.ToTable("UserPlantCodes", "dbo");

            entity.Property(p => p.Code).HasColumnName("PlantCode");

            entity.HasKey(e => e.ObjectId);
        });

        // m:n User/ Roles
        modelBuilder.Entity<User>()
            .HasMany(m => m.Roles)
            .WithMany(m => m.Users)
            .UsingEntity<UserRole>(
                    left => left.HasOne(o => o.RoleObject).WithMany(o => o.UserRoles).HasForeignKey(f => f.RoleObjectId),
                    right => right.HasOne(o => o.UserObject).WithMany(o => o.UserRoles).HasForeignKey(f => f.UserObjectId)
            );

        // 1:n User/ Plants
        modelBuilder.Entity<User>()
            .HasMany(m => m.UserPlantCodes)
            .WithOne(m => m.UserObject)
            .HasForeignKey(d => d.UserObjectId);
    }
}