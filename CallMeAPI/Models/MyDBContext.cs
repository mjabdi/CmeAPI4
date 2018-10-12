using System;
using Microsoft.EntityFrameworkCore;

namespace CallMeAPI.Models
{
    public class MyDBContext : DbContext
    {

        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Widget> Widgets { get; set; }
        public virtual DbSet<EventLog> EventLogs { get; set; }
        public virtual DbSet<SecurityLinkToken> SecurityLinkTokens { get; set; }
        public virtual DbSet<CallInfo> CallInfos { get; set; }


    }
}
