using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;

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

            modelBuilder.HasDbFunction(typeof(DbUtility)
                .GetMethod(nameof(DbUtility.DateDiff)))
                .HasTranslation(args =>
                {
                    var newArgs = args.ToArray();
                    newArgs[0] = new SqlFragmentExpression((string)((ConstantExpression)newArgs[0]).Value);
                    return new SqlFunctionExpression(
                        "DATEDIFF",
                        typeof(int),
                        newArgs);
                });
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Widget> Widgets { get; set; }
        public virtual DbSet<EventLog> EventLogs { get; set; }
        public virtual DbSet<SecurityLinkToken> SecurityLinkTokens { get; set; }
        public virtual DbSet<CallInfo> CallInfos { get; set; }
        public virtual DbSet<CallbackSchedule> CallbackSchedules { get; set; }
        public virtual DbSet<CallLog> CallLogs { get; set; }
        public virtual DbSet<StripeEventLog> StripeEventLogs { get; set; }
        public virtual DbSet<CallMeAPI.Models.Subscription> Subscriptions { get; set; }
        public virtual DbSet<CallMeAPI.Models.Invoice> Invoices { get; set; }
        public virtual DbSet<CallMeAPI.Models.AppException> AppExceptions { get; set; }
        public virtual DbSet<CallMeAPI.Models.CallReport> CallReports { get; set; }

    }


    public static class DbUtility
    {
        public static int DateDiff(string diffType, DateTime startDate, DateTime endDate)
        {
            throw new InvalidOperationException($"{nameof(DateDiff)} should be performed on database");
        }
    }
}
