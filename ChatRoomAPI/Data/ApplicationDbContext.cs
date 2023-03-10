using Microsoft.EntityFrameworkCore;
using ChatRoomAPI.Models;
using System.Collections.Generic;
using ChatRoomAPI.Data;

namespace ChatRoomWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
