using BugTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTCompanyInfoService
    {
        Task<Company> GetCompanyInfoByIdAsync(int? companyId);

        Task<IEnumerable<BTUser>> GetAllMembersAsync(int companyId);

        public Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId);

        public Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId);
    }
}
