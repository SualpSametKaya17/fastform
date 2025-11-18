using FastForm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FastForm.Data
{
    public class FastFormDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<FormTemplate> FormTemplates { get; set; } = null!;
        public DbSet<FieldDefinition> FieldDefinitions { get; set; } = null!;
        public DbSet<FormInstance> FormInstances { get; set; } = null!;
        public DbSet<FieldValue> FieldValues { get; set; } = null!;
        public DbSet<ConfigurationHistory> ConfigurationHistories { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Setting> Settings { get; set; } = null!;
        public DbSet<FormTemplatePermission> FormTemplatePermissions { get; set; } = null!;
        public DbSet<BatchJob> BatchJobs { get; set; } = null!;
        public DbSet<Translation> Translations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Read connection string from appsettings.json or use default
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? "Server=(localdb)\\mssqllocaldb;Database=FastFormDb;Trusted_Connection=True;MultipleActiveResultSets=true";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.IsActive);
                entity.Property(e => e.Role).HasMaxLength(50);
            });

            // FormTemplate
            modelBuilder.Entity<FormTemplate>(entity =>
            {
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => new { e.Name, e.Version }).IsUnique();

                entity.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.ModifiedBy)
                    .WithMany()
                    .HasForeignKey(e => e.ModifiedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // FieldDefinition
            modelBuilder.Entity<FieldDefinition>(entity =>
            {
                entity.HasIndex(e => e.FormTemplateId);
                entity.HasIndex(e => new { e.FormTemplateId, e.FieldName }).IsUnique();

                entity.HasOne(e => e.FormTemplate)
                    .WithMany(t => t.FieldDefinitions)
                    .HasForeignKey(e => e.FormTemplateId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // FormInstance
            modelBuilder.Entity<FormInstance>(entity =>
            {
                entity.HasIndex(e => e.FormTemplateId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.InstanceNumber).IsUnique();
                entity.HasIndex(e => e.AssignedToId);
                entity.HasIndex(e => e.DueDate);

                entity.HasOne(e => e.FormTemplate)
                    .WithMany()
                    .HasForeignKey(e => e.FormTemplateId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.AssignedTo)
                    .WithMany()
                    .HasForeignKey(e => e.AssignedToId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.ApprovedBy)
                    .WithMany()
                    .HasForeignKey(e => e.ApprovedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // FieldValue
            modelBuilder.Entity<FieldValue>(entity =>
            {
                entity.HasIndex(e => e.FormInstanceId);
                entity.HasIndex(e => e.FieldDefinitionId);
                entity.HasIndex(e => new { e.FormInstanceId, e.FieldDefinitionId }).IsUnique();

                entity.HasOne(e => e.FormInstance)
                    .WithMany(i => i.FieldValues)
                    .HasForeignKey(e => e.FormInstanceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.FieldDefinition)
                    .WithMany()
                    .HasForeignKey(e => e.FieldDefinitionId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.ModifiedBy)
                    .WithMany()
                    .HasForeignKey(e => e.ModifiedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // ConfigurationHistory
            modelBuilder.Entity<ConfigurationHistory>(entity =>
            {
                entity.HasOne(e => e.FormTemplate)
                    .WithMany()
                    .HasForeignKey(e => e.FormTemplateId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ChangedBy)
                    .WithMany()
                    .HasForeignKey(e => e.ChangedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // AuditLog
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => new { e.EntityType, e.EntityId });
                entity.HasIndex(e => e.CreatedDate);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Setting
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasIndex(e => e.SettingKey).IsUnique();

                entity.HasOne(e => e.ModifiedBy)
                    .WithMany()
                    .HasForeignKey(e => e.ModifiedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // FormTemplatePermission
            modelBuilder.Entity<FormTemplatePermission>(entity =>
            {
                entity.HasOne(e => e.FormTemplate)
                    .WithMany()
                    .HasForeignKey(e => e.FormTemplateId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // BatchJob
            modelBuilder.Entity<BatchJob>(entity =>
            {
                entity.HasOne(e => e.FormTemplate)
                    .WithMany()
                    .HasForeignKey(e => e.FormTemplateId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Translation
            modelBuilder.Entity<Translation>(entity =>
            {
                entity.HasIndex(e => new { e.LanguageCode, e.ResourceKey }).IsUnique();
            });
        }
    }
}
