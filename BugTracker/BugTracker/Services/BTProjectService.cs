using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using BugTracker.Models.Enums;

namespace BugTracker.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;

        public BTProjectService(ApplicationDbContext context,
            IBTRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            var project = _context.Projects.FirstOrDefault(x => x.ProjectPriorityId == projectId);
            var currentPm = await GetProjectManagerAsync(projectId);

            if(currentPm != null)
            {
                try
                {
                    await RemoveProjectManagerAsync(projectId);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"**** ERROR **** - Error Removing current PM from project -> {ex.Message}");
                    return false;
                }
            }

            try
            {
                await AddProjectManagerAsync(userId, projectId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** - Error adding new pm -> {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return false;
            }

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId);
            if(!await IsUserOnProjectAsync(userId, projectId))
            {
                try
                {
                    project.Members.Add(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public async Task ArchiveProjectAsync(Project project)
        {
            project.Archived = true;
            _context.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BTUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            IEnumerable<BTUser> developers = await GetProjectMembersByRoleAsync(projectId, Roles.Developer.ToString());
            IEnumerable<BTUser> submitters = await GetProjectMembersByRoleAsync(projectId, Roles.Submitter.ToString());
            IEnumerable<BTUser> admins = await GetProjectMembersByRoleAsync(projectId, Roles.Admin.ToString());

            IEnumerable<BTUser> teamMembers = developers.Concat(submitters.Concat(admins));
            return teamMembers;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsByCompany(int companyId)
        {
            return await (_context.Projects
                .Where(x => x.Id == companyId && x.Archived == false))
                .Include(x => x.Members)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.Comments)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.Attachments)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.History)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.Notifications)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.DeveloperUser)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.OwnerUser)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.TicketStatus)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x. TicketPriority)
                .Include(x => x.Tickets)
                    .ThenInclude(x => x.TicketType)
                .Include(x => x.ProjectPriority)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsByPriority(int companyId, string priorityName)
        {
            var projects = await GetAllProjectsByCompany(companyId);
            int priorityId = await LookupProjectPriorityId(priorityName);

            return projects.Where(x => x.ProjectPriorityId == priorityId);
        }

        public async Task<IEnumerable<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            return (await GetAllProjectsByCompany(companyId))
                .Where(x => x.Archived == true);
        }

        public Task<IEnumerable<BTUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            // PER DISCORD THIS IS NEVER IMPLEMENTED NOT MISSED!
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            return _context.Projects
                .Include(x => x.Tickets)
                .Include(x => x.Members)
                .Include(x => x.ProjectPriority)
                .FirstOrDefaultAsync(x => x.Id == projectId && x.CompanyId == companyId);
        }

        public async Task<BTUser> GetProjectManagerAsync(int projectId)
        {
            Project project = await _context.Projects
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == projectId);

            foreach (var user in project?.Members)
            {
                if(await _rolesService.IsUserInRoleAsync(user, Roles.ProjectManager.ToString()))
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<IEnumerable<BTUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            var project = await _context.Projects
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == projectId);

            IList<BTUser> memebers = new List<BTUser>();

            foreach (var u in project.Members)
            {
                if(await _rolesService.IsUserInRoleAsync(u, role))
                {
                    memebers.Add(u);
                }
            }

            return memebers;
        }

        public Task<IEnumerable<BTUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            // PER DISCORD THIS IS NEVER IMPLEMENTED NOT MISSED!
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                var userProjects = (await _context.Users
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Company)
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Members)
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Tickets)
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Tickets)
                            .ThenInclude(x => x.DeveloperUser)
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Tickets)
                            .ThenInclude(x => x.OwnerUser)
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Tickets)
                            .ThenInclude(x => x.TicketPriority)
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Tickets)
                            .ThenInclude(x => x.TicketStatus)
                    .Include(x => x.Projects)
                        .ThenInclude(x => x.Tickets)
                            .ThenInclude(x => x.TicketType)
                    .FirstOrDefaultAsync(x => x.Id == userId)).Projects;

                return userProjects;
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** - Error Getting User Projects -> {ex.Message}");
            }
            return null;
        }

        public async Task<IEnumerable<BTUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            var users = await _context.Users.Where(x => x.Projects.All(x => x.Id != projectId)).ToListAsync();
            return users.Where(x => x.CompanyId == companyId);
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            var project = await _context.Projects
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
            {
                return false;
            }

            return project.Members
                .Any(x => x.Id == userId);
        }

        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            return (await _context.ProjectPriorities.FirstOrDefaultAsync(x => x.Name == priorityName)).Id;
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == projectId);

            try
            {
                foreach(var u in project?.Members)
                {
                    if(await _rolesService.IsUserInRoleAsync(u, Roles.ProjectManager.ToString()))
                    {
                        await RemoveUserFromProjectAsync(u.Id, projectId);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

                try
                {
                    if (await IsUserOnProjectAsync(userId, projectId))
                    {
                        project.Members.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** - Error Removing User from project -> {ex.Message}");
            }
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            try
            {
                IEnumerable<BTUser> memebers = await GetProjectMembersByRoleAsync(projectId, role);
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

                foreach (var u in memebers)
                {
                    try 
                    {
                        project.Members.Remove(u);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** - Error Removing User from project -> {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
