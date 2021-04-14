using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public DbSet<Likes> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

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


            modelBuilder.Entity<Likes>()
               .HasKey(l => new { l.QuestionId, l.UserId });

            
            modelBuilder.Entity<Likes>()
                .HasOne(l => l.Question)
                .WithMany(q => q.Likes)
                .HasForeignKey(q => q.QuestionId);

            
            modelBuilder.Entity<Likes>()
                .HasOne(qt => qt.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(u => u.UserId);

        }
    }
}
