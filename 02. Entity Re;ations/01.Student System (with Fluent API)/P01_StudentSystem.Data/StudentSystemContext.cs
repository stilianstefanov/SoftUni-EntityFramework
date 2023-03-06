namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;

    using P01_StudentSystem.Common;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
            
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<Homework> Homeworks { get; set; } = null!;

        public DbSet<Resource> Resources { get; set; } = null!;

        public DbSet<Student> Students { get; set; } = null!;

        public DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);

                entity.Property(s => s.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(ValidationConstansts.StudentNameMaxLength);                                

                entity.Property(s => s.PhoneNumber)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(ValidationConstansts.StudentPhoneNumberMaxLength);

                entity.Property(s => s.RegisteredOn)
                .IsRequired(true);

                entity.Property(s => s.Birthday)
                .IsRequired(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);

                entity.Property(c => c.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength (ValidationConstansts.CourseNameMaxLength);

                entity.Property(e => e.Description)
                .IsRequired(false)
                .IsUnicode(true)
                .HasMaxLength(ValidationConstansts.CourseDescrMaxLength);

                entity.Property(c => c.StartDate)
                .IsRequired(true);

                entity.Property(c => c.EndDate)
                .IsRequired(true);

                entity.Property(c => c.Price)
                .IsRequired(true);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(r => r.ResourceId);

                entity.Property(r => r.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(ValidationConstansts.ResourceNameMaxLength);

                entity.Property(r => r.Url)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(ValidationConstansts.ResourceUrlMaxLength);

                entity.Property(r => r.ResourceType)
                .IsRequired(true);

                entity.Property(r => r.CourseId)
                .IsRequired(true);

                entity.HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(c => c.CourseId);
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(h => h.HomeworkId);

                entity.Property(h => h.Content)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(ValidationConstansts.HomeWorkContentMaxLength);

                entity.Property(h => h.ContentType)
                .IsRequired(true);

                entity.Property(h => h.SubmissionTime)
                .IsRequired(true);

                entity.Property(h => h.StudentId)
                .IsRequired(true);

                entity.Property(h => h.CourseId)
                .IsRequired(true);

                entity.HasOne(h => h.Student)
                .WithMany(s => s.Homeworks)
                .HasForeignKey(h => h.StudentId);

                entity.HasOne(h => h.Course)
                .WithMany(c => c.Homeworks)
                .HasForeignKey(h => h.CourseId);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new {sc.StudentId, sc.CourseId});

                entity.HasOne(sc => sc.Student)
                .WithMany(s => s.StudentsCourses)
                .HasForeignKey(sc => sc.StudentId);

                entity.HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(sc => sc.CourseId);
            });
        }
    }
}