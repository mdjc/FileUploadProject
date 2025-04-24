using Microsoft.EntityFrameworkCore;
using FileUploadApi.Entities;

namespace FileUploadApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FileEntity> Files => Set<FileEntity>();
}
