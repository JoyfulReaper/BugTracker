using BugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<BTUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Invite> Invites { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }

        public DbSet<TicketHistory> TicketHistories { get; set; }

        public DbSet<TicketPriority> TicketPriorities { get; set; }

        public DbSet<TicketStatus> TicketStatuses { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Invite>()
                .HasOne(x => x.Project)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Notification>()
                .HasOne(x => x.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<BugTracker.Models.ProjectPriority> ProjectPriority { get; set; }
    }
}
