using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTRolesService : IBTRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BTUser> _userManager;

        public BTRolesService(ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<BTUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            try
            {
                return _context.Roles.AsEnumerable<IdentityRole>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        
        public async Task<bool> AddUserToRoleAsync(BTUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            IdentityRole role = _context.Roles.Find(roleId);
            string result = await _roleManager.GetRoleNameAsync(role);

            return result;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(BTUser user)
        {
            IEnumerable<string> result = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<IEnumerable<BTUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            IList<BTUser> users = (await _userManager.GetUsersInRoleAsync(roleName));
            IEnumerable<BTUser> result = users.Where(x => x.CompanyId == companyId);

            return result;
        }

        public async Task<IEnumerable<BTUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {
            IList<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(x => x.Id).ToList();
            IList<BTUser> roleUsers = _context.Users.Where(x => !userIds.Contains(x.Id)).ToList();

            IEnumerable<BTUser> result = roleUsers.Where(x => x.CompanyId == companyId); // Why pull every user from the DB then do this.... TODO: Fix it

            return result;
        }

        public async Task<bool> IsUserInRoleAsync(BTUser user, string roleName)
        {
            bool result = await _userManager.IsInRoleAsync(user, roleName);
            return result;
        }

        public async Task<bool> RemoveUserFromRoleAsync(BTUser user, string roleName)
        {
            bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        public async Task<bool> RemoveUserFromRolesAsync(BTUser user, IEnumerable<string> roles)
        {
            bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
            return result;
        }
    }
}
