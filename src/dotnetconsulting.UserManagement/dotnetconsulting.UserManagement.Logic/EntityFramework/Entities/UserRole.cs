﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable

namespace dotnetconsulting.UserManagement.Logic.EntityFramework
{
    internal partial class UserRole
    {
        public int ObjectId { get; set; }
        public int UserObjectId { get; set; }
        public int RoleObjectId { get; set; }

        public virtual Role RoleObject { get; set; }
        internal virtual User UserObject { get; set; }
    }
}