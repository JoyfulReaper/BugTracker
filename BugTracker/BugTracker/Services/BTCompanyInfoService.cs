using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTCompanyInfoService : IBTCompanyInfoService
    {
        private readonly ApplicationDbContext _context;

        public BTCompanyInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BTUser>> GetAllMembersAsync(int companyId)
        {
            IList<BTUser> result = await _context.Users.Where(x => x.CompanyId == companyId).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId)
        {
            return await _context.Projects
                .Where(x => x.CompanyId == companyId)
                .Include(x => x.Members)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.Comments)
                 .Include(x => x.Tickets)
                    .ThenInclude(x => x.Attachments)
                 .Include(x => x.Tickets)
                    .ThenInclude(x => x.History)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.DeveloperUser)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.OwnerUser)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.TicketStatus)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.TicketPriority)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.TicketType)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.Notifications)
                .Include(x => x.ProjectPriority)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId)
        {
            
            IEnumerable<Project> projects = await GetAllProjectsAsync(companyId);
            var result = projects.SelectMany(x => x.Tickets);

            return result;
        }

        public async Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
            Company result = new();

            if(companyId != null)
            {
                result = await _context.Companies
                    .Include(x => x.Members)
                    .Include(x => x.Projects)
                    .Include(x => x.Invites)
                    .FirstOrDefaultAsync(x => x.Id == companyId);
            }

            return result;
        }
    }
}
