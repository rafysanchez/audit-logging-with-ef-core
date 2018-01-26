# Audit logging with EF Core

This is a demonstration on how to implement a generic audit log with Entity Framework Core.

## Usage

    using (var connection = new SqliteConnection("DataSource=:memory:"))
    {
        connection.Open();

        using (var students = new StudentRepository(connection))
        {
            var student = new Student
            {
                Id = 1,
                Name = "name",
                Classes = new List<Class> { new Class { Id = 1, Name = "Math" } }
            };
            students.Add(student);
            students.SaveChanges(student);
        }
    }

Will produce reports similar to:

*For the student (with `id == 1`)*

| User     | Changed  | Property | From     | To       | At                  |
|----------|----------|----------|----------|----------|---------------------|
| jdoe     | Student  | Name     |          | name     | 26.01.2018 10.30.11 |
| jdoe     | Class    | Name     |          | Math     | 26.01.2018 10.30.11 |

*For all classes (only one in this case)*
| User     | Changed  | Property | From     | To       | At                  |
|----------|----------|----------|----------|----------|---------------------|
| jdoe     | Class    | Name     |          | Math     | 26.01.2018 10.30.11 |

## Implementation

Add EF models and mark them as auditable:

    public class Student : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Class> Classes { get; set; }
    }

    public class Class : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
     
        public StudentDbContext(DbContextOptions<DbContext> options) : base(options) { }
    }

Configure the db context to accept audits:

    public class StudentDbContext : DbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Map(new AuditablePropertiesConfig());
            base.OnModelCreating(modelBuilder);
        }
    }

Calling `SaveChangesWithAudit()` instead of `SaveChanges()` to audit all changes:

    public class StudentRepository : IDisposable
    {
        private readonly StudentDbContext context;

        public StudentRepository(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseSqlite(connection)
                .Options;

            context = new StudentDbContext(options);
            context.Database.EnsureCreated();
        }

        public void Add(Student student)
        {
            context.Users.Add(student);
        }

        public Student Get(int id)
        {
            return context.Users.Include(s => s.Classes).Single(s => s.Id == id);
        }

        public void SaveChanges(Student student)
        {
            context.SaveChangesWithAudit(student);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
