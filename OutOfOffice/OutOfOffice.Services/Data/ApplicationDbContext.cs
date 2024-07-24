using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models.Models;

namespace OutOfOffice.Services.Data;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<ProjectsAndEmployee> ProjectsAndEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseSqlServer("Data Source=LAPTOP-O46231OI\\SQLEXPRESS;Initial Catalog=OutOfOffice;Integrated Security=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder.Entity<ApprovalRequest>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC275C72B963");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.Comment).HasColumnType("text");
            _ = entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("New");

            _ = entity.HasOne(d => d.ApproverNavigation).WithMany(p => p.ApprovalRequests)
                .HasForeignKey(d => d.Approver)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalR__Appro__31EC6D26");

            _ = entity.HasOne(d => d.LeaveRequestNavigation).WithMany(p => p.ApprovalRequests)
                .HasForeignKey(d => d.LeaveRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalR__Leave__32E0915F");
        });

        _ = modelBuilder.Entity<Employee>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC2739898A28");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);
            _ = entity.Property(e => e.Photo).HasColumnType("image");
            _ = entity.Property(e => e.Position)
                .HasMaxLength(255)
                .IsUnicode(false);
            _ = entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            _ = entity.Property(e => e.Subdivision)
                .HasMaxLength(255)
                .IsUnicode(false);

            _ = entity.HasOne(d => d.PeoplePartnerNavigation).WithMany(p => p.InversePeoplePartnerNavigation)
                .HasForeignKey(d => d.PeoplePartner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Peopl__276EDEB3");
        });

        _ = modelBuilder.Entity<LeaveRequest>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__LeaveReq__3214EC27DF90C764");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.AbscenceReason)
                .HasMaxLength(255)
                .IsUnicode(false);
            _ = entity.Property(e => e.Comment).HasColumnType("text");
            _ = entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("New");

            _ = entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveRequ__Emplo__2C3393D0");
        });

        _ = modelBuilder.Entity<Project>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC27D765D917");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.Comment).HasColumnType("text");
            _ = entity.Property(e => e.ProjectType)
                .HasMaxLength(255)
                .IsUnicode(false);
            _ = entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);

            _ = entity.HasOne(d => d.ProjectManagerNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ProjectManager)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Projects__Projec__38996AB5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
