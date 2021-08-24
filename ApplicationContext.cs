using System;
using System.Collections.Generic;
using ContactApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace ContactApp
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Properties of User Entity
            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(r => r.Roles)
                .WithMany(u => u.Users);

            var adminRoleGuid = Guid.NewGuid();
            var adminRole = new UserRole()
            {
                Guid = adminRoleGuid,
                Name = "Admin",
                Permissions = new []
                {
                    RolePermissions.ManageRoles,
                    RolePermissions.CreateContact,
                    RolePermissions.ReadContact,
                    RolePermissions.UpdateContact,
                    RolePermissions.DeleteContact,
                    RolePermissions.CreateUser,
                    RolePermissions.ReadUser,
                    RolePermissions.UpdateUser,
                    RolePermissions.DeleteUser
                }

            };
            var editorRoleGuid = Guid.NewGuid();
            var editorRole = new UserRole()
            {
                Guid = editorRoleGuid,
                Name = "Editor",
                Permissions = new []
                {
                    RolePermissions.CreateContact,
                    RolePermissions.ReadContact,
                    RolePermissions.UpdateContact,
                    RolePermissions.DeleteContact,
                    RolePermissions.CreateUser,
                    RolePermissions.ReadUser,
                    RolePermissions.UpdateUser,
                    RolePermissions.DeleteUser
                }
            };
            var moderatorRoleGuid = Guid.NewGuid();
            var moderatorRole = new UserRole()
            {
                Guid = moderatorRoleGuid, 
                Name = "Moderator",
                Permissions = new []
                {
                    RolePermissions.ReadContact,
                    RolePermissions.UpdateContact,
                    RolePermissions.ReadUser,
                    RolePermissions.UpdateUser
                }

            };


            var adminUserGuid = Guid.NewGuid();
            var adminUser = new User()
            {
                Email = "admin@admin.com",
                FullName = "Admin Account",
                Guid = adminUserGuid,
                Password = "admin123",
                UserName = "admin",
            };

            var editorUserGuid = Guid.NewGuid();
            var editorUser = new User()
            {
                Email = "editor@editor.com",
                FullName = "Editor Account",
                Guid = editorUserGuid,
                Password = "editor123",
                UserName = "editor",
            };

            var moderatorUserGuid = Guid.NewGuid();
            var moderatorUser = new User()
            {
                Email = "moderator@moderator.com",
                FullName = "Moderator Account",
                Guid = moderatorUserGuid,
                Password = "moderator123",
                UserName = "moderator",
            };
            
            modelBuilder.Entity<UserRole>().HasData(adminRole, editorRole, moderatorRole);
            modelBuilder.Entity<User>().HasData(adminUser, editorUser, moderatorUser);
            modelBuilder.Entity("UserUserRole").HasData(
                new {UsersGuid=adminUserGuid, RolesGuid = adminRoleGuid},
                new {UsersGuid=moderatorUserGuid, RolesGuid = moderatorRoleGuid},
                new {UsersGuid=editorUserGuid, RolesGuid = editorRoleGuid}
            );
            

        }
        
        
    }
}