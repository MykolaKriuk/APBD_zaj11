using APBD_zaj11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_zaj11.Contexts;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}