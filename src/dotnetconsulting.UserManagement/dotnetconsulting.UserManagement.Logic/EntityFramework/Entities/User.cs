﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable

namespace dotnetconsulting.UserManagement.Logic.EntityFramework
{
    public partial class User
    {
        public User()
        {
            UserPlantCodes = new HashSet<UserPlantCode>();

            Roles = new HashSet<Role>();

            UserRoles = new HashSet<UserRole>();
        }

        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string ResourceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<UserPlantCode> UserPlantCodes { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        internal virtual ICollection<UserRole> UserRoles { get; set; }
    }
}