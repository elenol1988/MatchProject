using Microsoft.EntityFrameworkCore;
using System;
using Shared;

namespace Api.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }

        public DbSet<Match> Match { get; set; }

        public DbSet<MatchOdds> MatchOdds { get; set; }

        public DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>().HasData(
             new Match
             {
                 Id = 1,
                 Description = "OSFP-PAO",
                 MatchDate = DateTime.Parse("2021-03-19").Date,
                 MatchTime = new TimeSpan(12, 00, 00),
                 TeamA = "OSFP",
                 TeamB = "PAO",
                 Sport = Sport.Football
             }
          );

            modelBuilder.Entity<MatchOdds>().HasData(
               new MatchOdds
               {
                   Id = 1,
                   MatchId = 1,
                   Specifier = "X",
                   Odd = 1.5
               }
            );

            #region UserInfoSeed
            modelBuilder.Entity<UserInfo>().HasData(
                new UserInfo
                {
                    Id = 1,
                    FirstName = "Eleni",
                    LastName = "Grigori",
                    UserName = "Eleni_Admin",
                    Email = "grigorieleni@gmail.com",
                    Password = "eleni123",
                    CreatedDate = DateTime.Now
                }
            );
            #endregion
        }

    }
}
