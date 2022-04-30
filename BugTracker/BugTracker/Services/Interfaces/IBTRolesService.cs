using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTRolesService
    {
        public Task<bool> IsUserInRoleAsync (BTUser user, string roleName);

        public IEnumerable<IdentityRole> GetRoles();

        public Task<IEnumerable<string>> GetUserRolesAsync(BTUser user);

        public Task<bool> AddUserToRoleAsync(BTUser user, string roleName);

        public Task<bool> RemoveUserFromRoleAsync(BTUser user, string roleName);

        public Task<bool> RemoveUserFromRolesAsync(BTUser user, IEnumerable<string> roles);

        public Task<IEnumerable<BTUser>> GetUsersInRoleAsync(string roleName, int companyId);

        public Task<IEnumerable<BTUser>> GetUsersNotInRoleAsync(string roleName, int companyId);

        public Task<string> GetRoleNameByIdAsync(string roleId);
    }
}
