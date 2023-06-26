﻿// <auto-generated />
using System;
using F88.Digital.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    [DbContext(typeof(AppPartnerDbContext))]
    [Migration("20210309072308_AddNewColumn_NotiSummary")]
    partial class AddNewColumn_NotiSummary
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("AspNetCoreHero.EntityFrameworkCore.Auditing.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AffectedColumns")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<decimal?>("BalanceValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NotiDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NotiDetail")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("NotiIconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NotiPeriod")
                        .HasColumnType("int");

                    b.Property<int>("NotiSummary")
                        .HasColumnType("int");

                    b.Property<string>("NotiTitle")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("NotiType")
                        .HasColumnType("int");

                    b.Property<string>("NotiTypeCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int?>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotiTypeCode")
                        .IsClustered(false);

                    b.HasIndex("UserProfileId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("OtherAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PaidValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<decimal?>("TaxValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("TransferDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserBankId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TransferDate")
                        .IsClustered(false);

                    b.HasIndex("UserBankId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.PaymentUserLoanReferral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<int>("UserLoanReferralId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserLoanReferralId");

                    b.ToTable("PaymentUserLoanReferral");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.TransactionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransFinalStatus")
                        .HasColumnType("int");

                    b.Property<decimal?>("TransSubTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TransFinalStatus")
                        .IsClustered(false);

                    b.HasIndex("TransactionDate")
                        .IsClustered(false);

                    b.HasIndex("TransactionId")
                        .IsUnique()
                        .IsClustered(false);

                    b.HasIndex("TransactionType")
                        .IsClustered(false);

                    b.HasIndex("UserProfileId");

                    b.ToTable("TransactionHistorys");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserAuthToken", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LoginToken")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime?>("LoginTokenCreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OTPHash")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ResetPasswordToken")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("UserAuthTokens");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AccName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AccNumber")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<string>("Branch")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("UserBankStatus")
                        .HasColumnType("bit");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccName")
                        .IsClustered(false);

                    b.HasIndex("AccNumber")
                        .IsUnique()
                        .IsClustered(false);

                    b.HasIndex("BankId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserBanks");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserDevice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeviceBrowserType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeviceLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DeviceOS")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId")
                        .IsClustered(false);

                    b.HasIndex("DeviceName")
                        .IsClustered(false);

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserDevices");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserLoanReferral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("District")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsF88Cus")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("LoanAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("LoanStatus")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Province")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RefAsset")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RefContractGroupId")
                        .HasColumnType("int");

                    b.Property<int>("RefFinalGroupId")
                        .HasColumnType("int");

                    b.Property<int>("RefRealGroupId")
                        .HasColumnType("int");

                    b.Property<int>("RefTempGroupId")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FullName")
                        .IsClustered(false);

                    b.HasIndex("PhoneNumber")
                        .IsClustered(false);

                    b.HasIndex("UserProfileId")
                        .IsClustered(false);

                    b.ToTable("UserLoanReferrals");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AppNotificationId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NotiEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NotiStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PaymentId")
                        .HasColumnType("int");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppNotificationId");

                    b.HasIndex("NotiEndDate")
                        .IsClustered(false);

                    b.HasIndex("NotiStartDate")
                        .IsClustered(false);

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserNotification");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserOTP", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("OTPHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPhone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("UserOTP");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AvatarURL")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActiveUpdate")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAgreementConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Passport")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("PassportBackURL")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("PassportDate")
                        .HasColumnType("Date");

                    b.Property<string>("PassportFrontURL")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PassportPlace")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("UserPhone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("WarningCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FirstName")
                        .IsClustered(false);

                    b.HasIndex("LastName")
                        .IsClustered(false);

                    b.HasIndex("Passport")
                        .IsUnique()
                        .HasFilter("[Passport] IS NOT NULL")
                        .IsClustered(false);

                    b.HasIndex("UserPhone")
                        .IsUnique()
                        .IsClustered(false);

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Deposit", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserLoanReferral", "UserLoanReferral")
                        .WithOne("Deposit")
                        .HasForeignKey("F88.Digital.Domain.Entities.AppPartner.Deposit", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserLoanReferral");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Notification", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserProfile", null)
                        .WithMany("AppNotifications")
                        .HasForeignKey("UserProfileId");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Payment", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserBank", "UserBank")
                        .WithMany()
                        .HasForeignKey("UserBankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserBank");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.PaymentUserLoanReferral", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.Payment", "Payment")
                        .WithMany("PaymentUserLoanReferrals")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserLoanReferral", "UserLoanReferral")
                        .WithMany()
                        .HasForeignKey("UserLoanReferralId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("UserLoanReferral");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.TransactionHistory", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserProfile", "UserProfile")
                        .WithMany("TransactionHistorys")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserAuthToken", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserProfile", "UserProfile")
                        .WithOne("UserAuthToken")
                        .HasForeignKey("F88.Digital.Domain.Entities.AppPartner.UserAuthToken", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserBank", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserProfile", "UserProfile")
                        .WithMany("UserBanks")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserDevice", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserProfile", "UserProfile")
                        .WithMany()
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserLoanReferral", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserProfile", "UserProfile")
                        .WithMany("UserLoanReferrals")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserNotification", b =>
                {
                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.Notification", "AppNotification")
                        .WithMany()
                        .HasForeignKey("AppNotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId");

                    b.HasOne("F88.Digital.Domain.Entities.AppPartner.UserProfile", "UserProfile")
                        .WithMany()
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppNotification");

                    b.Navigation("Payment");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.Payment", b =>
                {
                    b.Navigation("PaymentUserLoanReferrals");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserLoanReferral", b =>
                {
                    b.Navigation("Deposit");
                });

            modelBuilder.Entity("F88.Digital.Domain.Entities.AppPartner.UserProfile", b =>
                {
                    b.Navigation("AppNotifications");

                    b.Navigation("TransactionHistorys");

                    b.Navigation("UserAuthToken");

                    b.Navigation("UserBanks");

                    b.Navigation("UserLoanReferrals");
                });
#pragma warning restore 612, 618
        }
    }
}
