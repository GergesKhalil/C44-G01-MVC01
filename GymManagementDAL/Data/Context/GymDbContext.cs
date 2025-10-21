using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Contexts
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {

        }
        //public GymDbContext() : base()
        //{
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =DESKTOP-VLOLT3L; Database = GymManagementSystemG001; Trusted_Connection = True; TrustServerCertificate = true");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
        #region DbSets
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<MemberShip> MembersShips { get; set; }
        public DbSet<MemberSession> MembersSessions { get; set; }
        #endregion
    }
}
