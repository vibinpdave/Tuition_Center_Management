namespace TCM.Persistence.DBContext
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<GradeSubjects> GradeSubjects { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<TeacherGradeSubjects> TeacherGradeSubjects { get; set; }
        public DbSet<Parent> Parent { get; set; }
        public DbSet<Student> Student { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
