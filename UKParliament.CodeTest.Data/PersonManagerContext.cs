using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data;

public class PersonManagerContext(DbContextOptions<PersonManagerContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });


#if DEBUG
        modelBuilder.Entity<Person>().HasData(            
            new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = DateTime.Parse("1991-12-1"), DepartmentId = 1 },
            new Person { Id = 2, FirstName = "Tommy", LastName = "Townsend", DateOfBirth = DateTime.Parse("1996-03-27"), DepartmentId = 2 },
            new Person { Id = 3, FirstName = "Shawn", LastName = "Oliver", DateOfBirth = DateTime.Parse("2004-12-09"), DepartmentId = 2 },
            new Person { Id = 4, FirstName = "Stephen", LastName = "O'Niel", DateOfBirth = DateTime.Parse("1987-4-20"), DepartmentId = 2 },
            new Person { Id = 5, FirstName = "Mohammed", LastName = "Hassan", DateOfBirth = DateTime.Parse("1990-09-12"), DepartmentId = 1 },
            new Person { Id = 6, FirstName = "Jannet", LastName = "Lee", DateOfBirth = DateTime.Parse("2002-12-09"), DepartmentId = 3 },
            new Person { Id = 7, FirstName = "Sioban", LastName = "Knight", DateOfBirth = DateTime.Parse("2005-11-15"), DepartmentId = 1 },
            new Person { Id = 8, FirstName = "Edward", LastName = "Hover", DateOfBirth = DateTime.Parse("1983-03-19"), DepartmentId = 3 },
            new Person { Id = 9, FirstName = "Anwar", LastName = "Suleman", DateOfBirth = DateTime.Parse("1989-05-21"), DepartmentId = 2 },
            new Person { Id = 10, FirstName = "Yasmin", LastName = "Al Talib", DateOfBirth = DateTime.Parse("2001-09-30"), DepartmentId = 4 },
            new Person { Id = 11, FirstName = "Misha", LastName = "Nakisj", DateOfBirth = DateTime.Parse("1979-08-11"), DepartmentId = 4 },
            new Person { Id = 12, FirstName = "Vladimir", LastName = "Crakatan", DateOfBirth = DateTime.Parse("2001-01-02"), DepartmentId = 2 },             
            new Person { Id = 13, FirstName = "Peter", LastName = "File", DateOfBirth = DateTime.Parse("2000-11-19"), DepartmentId = 4 }
        );
#endif
    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }
}