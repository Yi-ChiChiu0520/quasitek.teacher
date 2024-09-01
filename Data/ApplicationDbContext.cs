using quasitekWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace quasitekWeb.Data;

// Creates a class called ApplicationDbContext that inherits from DbContext
// DbContext is a class from Entity Framework Core that allows you to interact with a database.
public class ApplicationDbContext: DbContext{
    // Constructor that takes in an instance of DbContextOptions and passes it to the base class constructor (DbContext)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 

    }

    // DbSet properties that represent tables in the database (Student, Teacher, Course, Classes, Device, Record) 
    public DbSet<Student> Student { get; set; }
    public DbSet<Teacher> Teacher { get; set; } 
    public DbSet<Course> Course { get; set; }
    public DbSet<Classes> Classes { get; set; }
    public DbSet<Device> Device { get; set; } 
    public DbSet<RecordLog> RecordLog { get; set; }
    public DbSet<PaperType> PaperType { get; set; }
    public DbSet<Mode> Mode { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define primary keys
        modelBuilder.Entity<PaperType>()
            .HasKey(p => p.TypeId); // Ensure primary key is defined

        modelBuilder.Entity<RecordLog>()
            .HasKey(r => r.LogId); // Ensure primary key is defined

        // Set up one-to-many relationship for AcademicType
        modelBuilder.Entity<RecordLog>()
        .HasOne(rl => rl.AcedemicType)
        .WithMany(pt => pt.AcademicRecordLogs) // Use separate navigation collection
        .HasForeignKey(rl => rl.AcedemicTypeId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

    // Relationship for technical type
    modelBuilder.Entity<RecordLog>()
        .HasOne(rl => rl.TechType)
        .WithMany(pt => pt.TechnicalRecordLogs) // Use separate navigation collection
        .HasForeignKey(rl => rl.TechTypeId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
    }

        
}