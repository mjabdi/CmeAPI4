﻿// <auto-generated />
using System;
using CallMeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CallMeAPI.Migrations
{
    [DbContext(typeof(MyDBContext))]
    partial class MyDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CallMeAPI.Models.CallbackSchedule", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("CallDone");

                    b.Property<string>("Comment");

                    b.Property<bool>("EmailNotificationDone");

                    b.Property<string>("LeadEmail");

                    b.Property<string>("LeadMessage");

                    b.Property<string>("LeadName");

                    b.Property<string>("LeadPhoneNumber");

                    b.Property<DateTime>("ScheduledDateTime");

                    b.Property<Guid?>("widgetID");

                    b.HasKey("ID");

                    b.HasIndex("widgetID");

                    b.ToTable("CallbackSchedules");
                });

            modelBuilder.Entity("CallMeAPI.Models.CallInfo", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CallDateTime");

                    b.Property<string>("CallType");

                    b.Property<decimal>("Cost");

                    b.Property<string>("Destination");

                    b.Property<string>("Duration");

                    b.Property<string>("Extension");

                    b.Property<int>("Seconds");

                    b.Property<string>("Source");

                    b.Property<string>("Time");

                    b.HasKey("ID");

                    b.ToTable("CallInfos");
                });

            modelBuilder.Entity("CallMeAPI.Models.EventLog", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventDesc");

                    b.Property<string>("EventType");

                    b.Property<string>("IP");

                    b.Property<string>("PhoneNumber");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<Guid>("WidgetID");

                    b.HasKey("ID");

                    b.HasIndex("WidgetID");

                    b.ToTable("EventLogs");
                });

            modelBuilder.Entity("CallMeAPI.Models.SecurityLinkToken", b =>
                {
                    b.Property<string>("Token")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("Email");

                    b.Property<bool>("IsDone");

                    b.Property<string>("Type");

                    b.HasKey("Token");

                    b.ToTable("SecurityLinkTokens");
                });

            modelBuilder.Entity("CallMeAPI.Models.User", b =>
                {
                    b.Property<string>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsFirstLogon");

                    b.Property<DateTime?>("LastLogon");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Role");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CallMeAPI.Models.Widget", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthKey");

                    b.Property<int>("CallsCount");

                    b.Property<string>("ColorText");

                    b.Property<string>("ColorWidget");

                    b.Property<string>("ConnectedTo");

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("DomainURL");

                    b.Property<string>("Extension");

                    b.Property<bool>("IsAnimated");

                    b.Property<string>("Status");

                    b.Property<string>("TalkToUsText");

                    b.Property<string>("UserID");

                    b.Property<string>("WeekDays");

                    b.Property<string>("WidgetName");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Widgets");
                });

            modelBuilder.Entity("CallMeAPI.Models.CallbackSchedule", b =>
                {
                    b.HasOne("CallMeAPI.Models.Widget", "widget")
                        .WithMany()
                        .HasForeignKey("widgetID");
                });

            modelBuilder.Entity("CallMeAPI.Models.EventLog", b =>
                {
                    b.HasOne("CallMeAPI.Models.Widget", "Widget")
                        .WithMany()
                        .HasForeignKey("WidgetID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CallMeAPI.Models.Widget", b =>
                {
                    b.HasOne("CallMeAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });
#pragma warning restore 612, 618
        }
    }
}
