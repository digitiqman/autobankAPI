﻿using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using AutoBank.Models;

namespace Autobank.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<AccountModel> Account { get; set; }     

        //public ApplicationDbContext()
        //    : base("DefaultConnection", throwIfV1Schema: false)
        public ApplicationDbContext()
            : base("DBConnectionString", throwIfV1Schema: false)
        {
        }

        public ApplicationDbContext(string conStringName)
            : base("name=" + conStringName)
        {            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }

}