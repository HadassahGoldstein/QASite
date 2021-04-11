using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace QASite.Data
{
    public class QADbContext : DbContext
    {
        private readonly string _connectionString;

        public QADbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuestionsTags> QuestionsTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Entity<QuestionsTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            //set up foreign key from QuestionsTags to Questions
            modelBuilder.Entity<QuestionsTags>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionsTags)
                .HasForeignKey(q => q.QuestionId);

            //set up foreign key from QuestionsTags to Tags
            modelBuilder.Entity<QuestionsTags>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionsTags)
                .HasForeignKey(q => q.TagId);
        }
    }
}
