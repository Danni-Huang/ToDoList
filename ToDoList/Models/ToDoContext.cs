using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ToDoList.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options) { }

        public DbSet<ToDo> ToDos { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = "work", Name = "Work" },
                new Category { CategoryId = "home", Name = "Home" },
                new Category { CategoryId = "ex", Name = "Exercise" },
                new Category { CategoryId = "shop", Name = "Shopping" },
                new Category { CategoryId = "call", Name = "Contact" }
            );
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "open", Name = "Open" },
                new Status { StatusId = "closed", Name = "Completed" }
            );

            modelBuilder.Entity<ToDo>().HasData(
                new ToDo() { Id = 1, Description = "Get Milk", DueDate = new DateTime(2023, 10, 5), CategoryId = "home", StatusId = "open" },
                new ToDo() { Id = 2, Description = "Do Laundry", DueDate = new DateTime(2023, 10, 7), CategoryId = "home", StatusId = "open" }
                );
        }
    }
}
