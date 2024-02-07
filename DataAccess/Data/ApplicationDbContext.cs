using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Adminregion> Adminregions { get; set; }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserrole> Aspnetuserroles { get; set; }

    public virtual DbSet<Blockrequest> Blockrequests { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Casetag> Casetags { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<Emaillog> Emaillogs { get; set; }

    public virtual DbSet<Healthprofessional> Healthprofessionals { get; set; }

    public virtual DbSet<Healthprofessionaltype> Healthprofessionaltypes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Physicianlocation> Physicianlocations { get; set; }

    public virtual DbSet<Physiciannotification> Physiciannotifications { get; set; }

    public virtual DbSet<Physicianregion> Physicianregions { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Requestbusiness> Requestbusinesses { get; set; }

    public virtual DbSet<Requestclient> Requestclients { get; set; }

    public virtual DbSet<Requestclosed> Requestcloseds { get; set; }

    public virtual DbSet<Requestconcierge> Requestconcierges { get; set; }

    public virtual DbSet<Requestnote> Requestnotes { get; set; }

    public virtual DbSet<Requeststatuslog> Requeststatuslogs { get; set; }

    public virtual DbSet<Requesttype> Requesttypes { get; set; }

    public virtual DbSet<Requestwisefile> Requestwisefiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolemenu> Rolemenus { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftdetail> Shiftdetails { get; set; }

    public virtual DbSet<Shiftdetailregion> Shiftdetailregions { get; set; }

    public virtual DbSet<Smslog> Smslogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=9434;Server=localhost;Port=5432;Database=HalloDoc_Db;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("pk_admin");

            entity.ToTable("admin");

            entity.Property(e => e.Adminid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("adminid");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.Altphone)
                .HasMaxLength(20)
                .HasColumnName("altphone");
            entity.Property(e => e.Aspnetuserid)
                .HasMaxLength(128)
                .HasColumnName("aspnetuserid");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .HasColumnName("zip");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.AdminAspnetusers)
                .HasForeignKey(d => d.Aspnetuserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_admin");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.AdminCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_admin1");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.AdminModifiedbyNavigations)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("fk_admin2");

            entity.HasOne(d => d.Region).WithMany(p => p.Admins)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("fk_admin3");

            entity.HasOne(d => d.Role).WithMany(p => p.Admins)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("fk_admin4");
        });

        modelBuilder.Entity<Adminregion>(entity =>
        {
            entity.HasKey(e => e.Adminregionid).HasName("pk_adminregion");

            entity.ToTable("adminregion");

            entity.Property(e => e.Adminregionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("adminregionid");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Regionid).HasColumnName("regionid");

            entity.HasOne(d => d.Admin).WithMany(p => p.Adminregions)
                .HasForeignKey(d => d.Adminid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_adminregion");

            entity.HasOne(d => d.Region).WithMany(p => p.Adminregions)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_adminregion1");
        });

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_aspnetrole");

            entity.ToTable("aspnetroles");

            entity.Property(e => e.Id)
                .HasMaxLength(128)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_aspnetusers");

            entity.ToTable("aspnetusers");

            entity.Property(e => e.Id)
                .HasMaxLength(128)
                .HasColumnName("id");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Passwordhash)
                .HasColumnType("character varying")
                .HasColumnName("passwordhash");
            entity.Property(e => e.Phonenumber)
                .HasColumnType("character varying")
                .HasColumnName("phonenumber");
            entity.Property(e => e.Username)
                .HasMaxLength(256)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Aspnetuserrole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("pk_aspnetuserrole");

            entity.ToTable("aspnetuserroles");

            entity.Property(e => e.Roleid)
                .ValueGeneratedNever()
                .HasColumnName("roleid");
            entity.Property(e => e.Userid)
                .HasMaxLength(128)
                .HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserroles)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aspnetuserrole");
        });

        modelBuilder.Entity<Blockrequest>(entity =>
        {
            entity.HasKey(e => e.Blockrequestid).HasName("pk_blockrequests");

            entity.ToTable("blockrequests");

            entity.Property(e => e.Blockrequestid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("blockrequestid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isactive)
                .HasColumnType("bit(1)")
                .HasColumnName("isactive");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(50)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Reason)
                .HasColumnType("character varying")
                .HasColumnName("reason");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Request).WithMany(p => p.Blockrequests)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_blockrequests");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Businessid).HasName("pk_business");

            entity.ToTable("business");

            entity.Property(e => e.Businessid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("businessid");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Faxnumber)
                .HasMaxLength(20)
                .HasColumnName("faxnumber");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isregistered)
                .HasColumnType("bit(1)")
                .HasColumnName("isregistered");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(10)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.BusinessCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_business1");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.BusinessModifiedbyNavigations)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("fk_business2");

            entity.HasOne(d => d.Region).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("fk_business");
        });

        modelBuilder.Entity<Casetag>(entity =>
        {
            entity.HasKey(e => e.Casetagid).HasName("pk_casetag");

            entity.ToTable("casetag");

            entity.Property(e => e.Casetagid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("casetagid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Concierge>(entity =>
        {
            entity.HasKey(e => e.Conciergeid).HasName("pk_concierge");

            entity.ToTable("concierge");

            entity.Property(e => e.Conciergeid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("conciergeid");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Conciergename)
                .HasMaxLength(100)
                .HasColumnName("conciergename");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .HasColumnName("street");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(50)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.Region).WithMany(p => p.Concierges)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_concierge");
        });

        modelBuilder.Entity<Emaillog>(entity =>
        {
            entity.HasKey(e => e.Emaillogid).HasName("pk_emaillog");

            entity.ToTable("emaillog");

            entity.Property(e => e.Emaillogid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("emaillogid");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Confirmationnumber)
                .HasMaxLength(200)
                .HasColumnName("confirmationnumber");
            entity.Property(e => e.Createdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdate");
            entity.Property(e => e.Emailid)
                .HasMaxLength(200)
                .HasColumnName("emailid");
            entity.Property(e => e.Emailtemplate)
                .HasColumnType("character varying")
                .HasColumnName("emailtemplate");
            entity.Property(e => e.Filepath)
                .HasColumnType("character varying")
                .HasColumnName("filepath");
            entity.Property(e => e.Isemailsent)
                .HasColumnType("bit(1)")
                .HasColumnName("isemailsent");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Sentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sentdate");
            entity.Property(e => e.Senttries).HasColumnName("senttries");
            entity.Property(e => e.Subjectname)
                .HasMaxLength(200)
                .HasColumnName("subjectname");

            entity.HasOne(d => d.Admin).WithMany(p => p.Emaillogs)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("fk_emaillog2");

            entity.HasOne(d => d.Physician).WithMany(p => p.Emaillogs)
                .HasForeignKey(d => d.Physicianid)
                .HasConstraintName("fk_emaillog3");

            entity.HasOne(d => d.Request).WithMany(p => p.Emaillogs)
                .HasForeignKey(d => d.Requestid)
                .HasConstraintName("fk_emaillog1");
        });

        modelBuilder.Entity<Healthprofessional>(entity =>
        {
            entity.HasKey(e => e.Vendorid).HasName("pk_healthprofessionals");

            entity.ToTable("healthprofessionals");

            entity.Property(e => e.Vendorid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("vendorid");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.Businesscontact)
                .HasMaxLength(100)
                .HasColumnName("businesscontact");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Faxnumber)
                .HasMaxLength(50)
                .HasColumnName("faxnumber");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(100)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Profession).HasColumnName("profession");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Vendorname)
                .HasMaxLength(100)
                .HasColumnName("vendorname");
            entity.Property(e => e.Zip)
                .HasMaxLength(50)
                .HasColumnName("zip");

            entity.HasOne(d => d.ProfessionNavigation).WithMany(p => p.Healthprofessionals)
                .HasForeignKey(d => d.Profession)
                .HasConstraintName("fk_healthprofessional");

            entity.HasOne(d => d.Region).WithMany(p => p.Healthprofessionals)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("fk_healthprofessionals");
        });

        modelBuilder.Entity<Healthprofessionaltype>(entity =>
        {
            entity.HasKey(e => e.Healthprofessionalid).HasName("pk_healthprofessionaltype");

            entity.ToTable("healthprofessionaltype");

            entity.Property(e => e.Healthprofessionalid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("healthprofessionalid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .HasColumnType("bit(1)")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Professionname)
                .HasMaxLength(50)
                .HasColumnName("professionname");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Menuid).HasName("pk_menu");

            entity.ToTable("menu");

            entity.Property(e => e.Menuid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("menuid");
            entity.Property(e => e.Accounttype).HasColumnName("accounttype");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Sortorder).HasColumnName("sortorder");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_orderdetails");

            entity.ToTable("orderdetails");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Businesscontact)
                .HasMaxLength(100)
                .HasColumnName("businesscontact");
            entity.Property(e => e.Createdby)
                .HasMaxLength(100)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Faxnumber)
                .HasMaxLength(50)
                .HasColumnName("faxnumber");
            entity.Property(e => e.Noofrefill).HasColumnName("noofrefill");
            entity.Property(e => e.Prescription)
                .HasColumnType("character varying")
                .HasColumnName("prescription");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Vendorid).HasColumnName("vendorid");

            entity.HasOne(d => d.Request).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Requestid)
                .HasConstraintName("fk_orderdetails1");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Vendorid)
                .HasConstraintName("fk_orderdetails");
        });

        modelBuilder.Entity<Physician>(entity =>
        {
            entity.HasKey(e => e.Physicianid).HasName("pk_physician");

            entity.ToTable("physician");

            entity.Property(e => e.Physicianid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("physicianid");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.Adminnotes)
                .HasMaxLength(500)
                .HasColumnName("adminnotes");
            entity.Property(e => e.Altphone)
                .HasMaxLength(20)
                .HasColumnName("altphone");
            entity.Property(e => e.Aspnetuserid)
                .HasMaxLength(128)
                .HasColumnName("aspnetuserid");
            entity.Property(e => e.Businessname)
                .HasMaxLength(100)
                .HasColumnName("businessname");
            entity.Property(e => e.Businesswebsite)
                .HasMaxLength(200)
                .HasColumnName("businesswebsite");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Isagreementdoc)
                .HasColumnType("bit(1)")
                .HasColumnName("isagreementdoc");
            entity.Property(e => e.Isbackgrounddoc)
                .HasColumnType("bit(1)")
                .HasColumnName("isbackgrounddoc");
            entity.Property(e => e.Iscredentialdoc)
                .HasColumnType("bit(1)")
                .HasColumnName("iscredentialdoc");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Islicensedoc)
                .HasColumnType("bit(1)")
                .HasColumnName("islicensedoc");
            entity.Property(e => e.Isnondisclosuredoc)
                .HasColumnType("bit(1)")
                .HasColumnName("isnondisclosuredoc");
            entity.Property(e => e.Istokengenerate)
                .HasColumnType("bit(1)")
                .HasColumnName("istokengenerate");
            entity.Property(e => e.Istrainingdoc)
                .HasColumnType("bit(1)")
                .HasColumnName("istrainingdoc");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Medicallicense)
                .HasMaxLength(500)
                .HasColumnName("medicallicense");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Npinumber)
                .HasMaxLength(500)
                .HasColumnName("npinumber");
            entity.Property(e => e.Photo)
                .HasMaxLength(100)
                .HasColumnName("photo");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Signature)
                .HasMaxLength(500)
                .HasColumnName("signature");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Syncemailaddress)
                .HasMaxLength(50)
                .HasColumnName("syncemailaddress");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .HasColumnName("zip");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.PhysicianAspnetusers)
                .HasForeignKey(d => d.Aspnetuserid)
                .HasConstraintName("fk_physician");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.PhysicianCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physician3");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.PhysicianModifiedbyNavigations)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("fk_physician2");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicians)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("fk_physician1");
        });

        modelBuilder.Entity<Physicianlocation>(entity =>
        {
            entity.HasKey(e => e.Locationid).HasName("pk_physicianlocation");

            entity.ToTable("physicianlocation");

            entity.Property(e => e.Locationid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("locationid");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Latitude)
                .HasPrecision(9, 3)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(9, 3)
                .HasColumnName("longitude");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Physicianname)
                .HasMaxLength(50)
                .HasColumnName("physicianname");

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianlocations)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physicianlocation");
        });

        modelBuilder.Entity<Physiciannotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_physiciannotification");

            entity.ToTable("physiciannotification");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Isnotificationstopped)
                .HasColumnType("bit(1)")
                .HasColumnName("isnotificationstopped");
            entity.Property(e => e.Pysicianid).HasColumnName("pysicianid");

            entity.HasOne(d => d.Pysician).WithMany(p => p.Physiciannotifications)
                .HasForeignKey(d => d.Pysicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physiciannotification");
        });

        modelBuilder.Entity<Physicianregion>(entity =>
        {
            entity.HasKey(e => e.Physicianregionid).HasName("pk_physicianregion");

            entity.ToTable("physicianregion");

            entity.Property(e => e.Physicianregionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("physicianregionid");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Regionid).HasColumnName("regionid");

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianregions)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physicianregion");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicianregions)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physicianregion1");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Regionid).HasName("pk_region");

            entity.ToTable("region");

            entity.Property(e => e.Regionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("regionid");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(50)
                .HasColumnName("abbreviation");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("pk_request");

            entity.ToTable("request");

            entity.Property(e => e.Requestid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestid");
            entity.Property(e => e.Accepteddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("accepteddate");
            entity.Property(e => e.Calltype).HasColumnName("calltype");
            entity.Property(e => e.Casenumber)
                .HasMaxLength(50)
                .HasColumnName("casenumber");
            entity.Property(e => e.Casetag)
                .HasMaxLength(50)
                .HasColumnName("casetag");
            entity.Property(e => e.Casetagphysician)
                .HasMaxLength(50)
                .HasColumnName("casetagphysician");
            entity.Property(e => e.Completedbyphysician)
                .HasColumnType("bit(1)")
                .HasColumnName("completedbyphysician");
            entity.Property(e => e.Confirmationnumber)
                .HasMaxLength(20)
                .HasColumnName("confirmationnumber");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Createduserid).HasColumnName("createduserid");
            entity.Property(e => e.Declinedby)
                .HasMaxLength(250)
                .HasColumnName("declinedby");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Ismobile)
                .HasColumnType("bit(1)")
                .HasColumnName("ismobile");
            entity.Property(e => e.Isurgentemailsent)
                .HasColumnType("bit(1)")
                .HasColumnName("isurgentemailsent");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Lastreservationdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastreservationdate");
            entity.Property(e => e.Lastwellnessdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastwellnessdate");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Patientaccountid)
                .HasMaxLength(128)
                .HasColumnName("patientaccountid");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(23)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Relationname)
                .HasMaxLength(100)
                .HasColumnName("relationname");
            entity.Property(e => e.Requesttypeid).HasColumnName("requesttypeid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Physicianid)
                .HasConstraintName("fk_request2");

            entity.HasOne(d => d.Requesttype).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Requesttypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_request");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_request1");
        });

        modelBuilder.Entity<Requestbusiness>(entity =>
        {
            entity.HasKey(e => e.Requestbusinessid).HasName("pk_requestbusiness");

            entity.ToTable("requestbusiness");

            entity.Property(e => e.Requestbusinessid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestbusinessid");
            entity.Property(e => e.Businessid).HasColumnName("businessid");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Business).WithMany(p => p.Requestbusinesses)
                .HasForeignKey(d => d.Businessid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestbusiness1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestbusinesses)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestbusiness");
        });

        modelBuilder.Entity<Requestclient>(entity =>
        {
            entity.HasKey(e => e.Requestclientid).HasName("pk_requestclient");

            entity.ToTable("requestclient");

            entity.Property(e => e.Requestclientid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestclientid");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Communicationtype).HasColumnName("communicationtype");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Intdate).HasColumnName("intdate");
            entity.Property(e => e.Intyear).HasColumnName("intyear");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Ismobile)
                .HasColumnType("bit(1)")
                .HasColumnName("ismobile");
            entity.Property(e => e.Isreservationremindersent).HasColumnName("isreservationremindersent");
            entity.Property(e => e.Issetfollowupsent).HasColumnName("issetfollowupsent");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 3)
                .HasColumnName("latitude");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Longitude)
                .HasPrecision(10, 3)
                .HasColumnName("longitude");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.Notiemail)
                .HasMaxLength(50)
                .HasColumnName("notiemail");
            entity.Property(e => e.Notimobile)
                .HasMaxLength(20)
                .HasColumnName("notimobile");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(23)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Remindhousecallcount).HasColumnName("remindhousecallcount");
            entity.Property(e => e.Remindreservationcount).HasColumnName("remindreservationcount");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
            entity.Property(e => e.Strmonth)
                .HasMaxLength(20)
                .HasColumnName("strmonth");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(10)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.Region).WithMany(p => p.Requestclients)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclient1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestclients)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclient");
        });

        modelBuilder.Entity<Requestclosed>(entity =>
        {
            entity.HasKey(e => e.Requestclosedid).HasName("pk_requestclosed");

            entity.ToTable("requestclosed");

            entity.Property(e => e.Requestclosedid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestclosedid");
            entity.Property(e => e.Clientnotes)
                .HasMaxLength(500)
                .HasColumnName("clientnotes");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Phynotes)
                .HasMaxLength(500)
                .HasColumnName("phynotes");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Requeststatuslogid).HasColumnName("requeststatuslogid");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestcloseds)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclosed");

            entity.HasOne(d => d.Requeststatuslog).WithMany(p => p.Requestcloseds)
                .HasForeignKey(d => d.Requeststatuslogid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclosed1");
        });

        modelBuilder.Entity<Requestconcierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_requestconcierge");

            entity.ToTable("requestconcierge");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Conciergeid).HasColumnName("conciergeid");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Concierge).WithMany(p => p.Requestconcierges)
                .HasForeignKey(d => d.Conciergeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestconcierge1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestconcierges)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestconcierge");
        });

        modelBuilder.Entity<Requestnote>(entity =>
        {
            entity.HasKey(e => e.Requestnotesid).HasName("pk_requestnotes");

            entity.ToTable("requestnotes");

            entity.Property(e => e.Requestnotesid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestnotesid");
            entity.Property(e => e.Administrativenotes)
                .HasMaxLength(500)
                .HasColumnName("administrativenotes");
            entity.Property(e => e.Adminnotes)
                .HasMaxLength(500)
                .HasColumnName("adminnotes");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Intdate).HasColumnName("intdate");
            entity.Property(e => e.Intyear).HasColumnName("intyear");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Physiciannotes)
                .HasMaxLength(500)
                .HasColumnName("physiciannotes");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Strmonth)
                .HasMaxLength(20)
                .HasColumnName("strmonth");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestnotes)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestnotes");
        });

        modelBuilder.Entity<Requeststatuslog>(entity =>
        {
            entity.HasKey(e => e.Requeststatuslogid).HasName("pk_requeststatuslog");

            entity.ToTable("requeststatuslog");

            entity.Property(e => e.Requeststatuslogid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requeststatuslogid");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Transtoadmin)
                .HasColumnType("bit(1)")
                .HasColumnName("transtoadmin");
            entity.Property(e => e.Transtophysicianid).HasColumnName("transtophysicianid");

            entity.HasOne(d => d.Admin).WithMany(p => p.Requeststatuslogs)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("fk_requeststatuslog2");

            entity.HasOne(d => d.Physician).WithMany(p => p.RequeststatuslogPhysicians)
                .HasForeignKey(d => d.Physicianid)
                .HasConstraintName("fk_requeststatuslog1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requeststatuslogs)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requeststatuslog");

            entity.HasOne(d => d.Transtophysician).WithMany(p => p.RequeststatuslogTranstophysicians)
                .HasForeignKey(d => d.Transtophysicianid)
                .HasConstraintName("fk_requeststatuslog4");
        });

        modelBuilder.Entity<Requesttype>(entity =>
        {
            entity.HasKey(e => e.Requesttypeid).HasName("pk_requesttype");

            entity.ToTable("requesttype");

            entity.Property(e => e.Requesttypeid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requesttypeid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Requestwisefile>(entity =>
        {
            entity.HasKey(e => e.Requestwisefileid).HasName("pk_requestwisefile");

            entity.ToTable("requestwisefile");

            entity.Property(e => e.Requestwisefileid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("requestwisefileid");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Doctype).HasColumnName("doctype");
            entity.Property(e => e.Filename)
                .HasMaxLength(500)
                .HasColumnName("filename");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Iscompensation)
                .HasColumnType("bit(1)")
                .HasColumnName("iscompensation");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isfinalize)
                .HasColumnType("bit(1)")
                .HasColumnName("isfinalize");
            entity.Property(e => e.Isfrontside)
                .HasColumnType("bit(1)")
                .HasColumnName("isfrontside");
            entity.Property(e => e.Ispatientrecords)
                .HasColumnType("bit(1)")
                .HasColumnName("ispatientrecords");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");

            entity.HasOne(d => d.Admin).WithMany(p => p.Requestwisefiles)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("fk_requestwisefile2");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requestwisefiles)
                .HasForeignKey(d => d.Physicianid)
                .HasConstraintName("fk_requestwisefile1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestwisefiles)
                .HasForeignKey(d => d.Requestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestwisefile");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("pk_role");

            entity.ToTable("role");

            entity.Property(e => e.Roleid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("roleid");
            entity.Property(e => e.Accounttype).HasColumnName("accounttype");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Rolemenu>(entity =>
        {
            entity.HasKey(e => e.Rolemenuid).HasName("pk_rolemenu");

            entity.ToTable("rolemenu");

            entity.Property(e => e.Rolemenuid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("rolemenuid");
            entity.Property(e => e.Menuid).HasColumnName("menuid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Menu).WithMany(p => p.Rolemenus)
                .HasForeignKey(d => d.Menuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rolemenu2");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Shiftid).HasName("pk_shift");

            entity.ToTable("shift");

            entity.Property(e => e.Shiftid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("shiftid");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isrepeat)
                .HasColumnType("bit(1)")
                .HasColumnName("isrepeat");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Repeatupto).HasColumnName("repeatupto");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Weekdays)
                .HasMaxLength(7)
                .IsFixedLength()
                .HasColumnName("weekdays");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shift1");

            entity.HasOne(d => d.Physician).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Physicianid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shift");
        });

        modelBuilder.Entity<Shiftdetail>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailid).HasName("pk_shiftdetail");

            entity.ToTable("shiftdetail");

            entity.Property(e => e.Shiftdetailid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("shiftdetailid");
            entity.Property(e => e.Endtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("endtime");
            entity.Property(e => e.Eventid)
                .HasMaxLength(100)
                .HasColumnName("eventid");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Issync)
                .HasColumnType("bit(1)")
                .HasColumnName("issync");
            entity.Property(e => e.Lastrunningdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastrunningdate");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Shiftdate).HasColumnName("shiftdate");
            entity.Property(e => e.Shiftid).HasColumnName("shiftid");
            entity.Property(e => e.Starttime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttime");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.Shiftdetails)
                .HasForeignKey(d => d.Modifiedby)
                .HasConstraintName("fk_shiftdetails1");

            entity.HasOne(d => d.Shift).WithMany(p => p.Shiftdetails)
                .HasForeignKey(d => d.Shiftid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftdetails");
        });

        modelBuilder.Entity<Shiftdetailregion>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailregionid).HasName("pk_shiftdetailregion");

            entity.ToTable("shiftdetailregion");

            entity.Property(e => e.Shiftdetailregionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("shiftdetailregionid");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.Shiftdetailid).HasColumnName("shiftdetailid");

            entity.HasOne(d => d.Region).WithMany(p => p.Shiftdetailregions)
                .HasForeignKey(d => d.Regionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftdetailregion1");

            entity.HasOne(d => d.Shiftdetail).WithMany(p => p.Shiftdetailregions)
                .HasForeignKey(d => d.Shiftdetailid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftdetailregion");
        });

        modelBuilder.Entity<Smslog>(entity =>
        {
            entity.HasKey(e => e.Smslogid).HasName("pk_user");

            entity.ToTable("smslog");

            entity.Property(e => e.Smslogid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("smslogid");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.Adminid).HasColumnName("adminid");
            entity.Property(e => e.Confirmationnumber)
                .HasMaxLength(50)
                .HasColumnName("confirmationnumber");
            entity.Property(e => e.Createdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdate");
            entity.Property(e => e.Issmssent)
                .HasColumnType("bit(1)")
                .HasColumnName("issmssent");
            entity.Property(e => e.Mobilenumber)
                .HasMaxLength(50)
                .HasColumnName("mobilenumber");
            entity.Property(e => e.Physicianid).HasColumnName("physicianid");
            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Sentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sentdate");
            entity.Property(e => e.Senttries).HasColumnName("senttries");
            entity.Property(e => e.Smstemplate)
                .HasColumnType("character varying")
                .HasColumnName("smstemplate");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("pk_users");

            entity.ToTable("users");

            entity.Property(e => e.Userid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("userid");
            entity.Property(e => e.Aspnetuserid)
                .HasMaxLength(128)
                .HasColumnName("aspnetuserid");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Createdby)
                .HasMaxLength(128)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Intdate).HasColumnName("intdate");
            entity.Property(e => e.Intyear).HasColumnName("intyear");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Ismobile)
                .HasColumnType("bit(1)")
                .HasColumnName("ismobile");
            entity.Property(e => e.Isrequestwithemail)
                .HasColumnType("bit(1)")
                .HasColumnName("isrequestwithemail");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(128)
                .HasColumnName("modifiedby");
            entity.Property(e => e.Modifieddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Regionid).HasColumnName("regionid");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
            entity.Property(e => e.Strmonth)
                .HasMaxLength(20)
                .HasColumnName("strmonth");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .HasColumnName("zip");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.Users)
                .HasForeignKey(d => d.Aspnetuserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users1");

            entity.HasOne(d => d.Region).WithMany(p => p.Users)
                .HasForeignKey(d => d.Regionid)
                .HasConstraintName("fk_users2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
