using Microsoft.EntityFrameworkCore;
using DotnetWebAPI.Models;

namespace DotnetWebAPI.Repository {

    public class NotesDbContext : DbContext {
        public DbSet<Note> Notes => Set<Note>();

        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.UseSerialColumns();
        }
    }
}
