using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using TasksAuthWebApi.Models;

namespace TasksAuthWebApi.DAL
{
    public class TasksContext : DbContext
    {
        public TasksContext() : base("TasksContext") { }

        public DbSet<TTask> TTasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}