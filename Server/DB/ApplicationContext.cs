using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using ManagingSalesApp.Shared;
using System.Data;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
namespace ManagingSalesApp.Server.DB
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Window> Windows { get; set; }
        public DbSet<SubElement> SubElements { get; set; }
        //private readonly IWebAssemblyHostEnvironment _environment;
        public ApplicationContext()
        {
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            if (Database.CanConnect())
            {
              //  Database.EnsureDeleted();
              //  Database.EnsureCreated();
            }
            else
            { // first call
                
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);
            modelBuilder.Entity<Window>()
                .HasKey(w => w.Id);
            modelBuilder.Entity<SubElement>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Windows)
                .WithOne(w => w.Order)
                .HasForeignKey(w => w.OrderId);
           modelBuilder.Entity<Window>()
           .HasOne(w => w.Order)
           .WithMany(o => o.Windows)
           .HasForeignKey(w => w.OrderId);
            // Seed the data from the provided XML
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    Name = "New York Building 1",
                    State = "NY"
                },
                new Order
                {
                    Id = 2,
                    Name = "California Hotel AJK",
                    State = "CA"
                }
            );
            modelBuilder.Entity<Window>().HasData(
                new Window
                {
                    Id = 1,
                    Name = "A51",
                    QuantityOfWindows = 4,
                    TotalSubElements = 3,
                    OrderId = 1
                },
                new Window
                {
                    Id = 2,
                    Name = "C Zone 5",
                    QuantityOfWindows = 2,
                    TotalSubElements = 1,
                    OrderId = 1
                },
                new Window
                {
                    Id = 3,
                    Name = "GLB",
                    QuantityOfWindows = 3,
                    TotalSubElements = 2,
                    OrderId = 2
                },
                new Window
                {
                    Id = 4,
                    Name = "OHF",
                    QuantityOfWindows = 10,
                    TotalSubElements = 2,
                    OrderId = 2
                }
            );
            modelBuilder.Entity<SubElement>().HasData(
                new SubElement
                {
                    Id = 1,
                    Type = "Doors",
                    Width = 1200,
                    Height = 1850,
                    WindowId = 1
                },
                new SubElement
                {
                    Id = 2,
                    Type = "Window",
                    Width = 800,
                    Height = 1850,
                    WindowId = 1
                },
                new SubElement
                {
                    Id = 3,
                    Type = "Window",
                    Width = 700,
                    Height = 1850,
                    WindowId = 1
                },
                new SubElement
                {
                    Id = 4,
                    Type = "Window",
                    Width = 1500,
                    Height = 2000,
                    WindowId = 2
                },
                new SubElement
                {
                    Id = 5,
                    Type = "Doors",
                    Width = 1400,
                    Height = 2200,
                    WindowId = 3
                },
                new SubElement
                {
                    Id = 6,
                    Type = "Window",
                    Width = 600,
                    Height = 2200,
                    WindowId = 3
                },
                new SubElement
                {
                    Id = 7,
                    Type = "Window",
                    Width = 1500,
                    Height = 2000,
                    WindowId = 4
                },
                new SubElement
                {
                    Id = 8,
                    Type = "Window",
                    Width = 1500,
                    Height = 2000,
                    WindowId = 4
                }
            );
        }
    }
}
