using BugTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTProjectService
    {
        public Task AddNewProjectAsync(Project project);

        public Task<bool> AddProjectManagerAsync(string userId, int projectId);

        public Task<bool> AddUserToProjectAsync(string userId, int projectId);

        public Task ArchiveProjectAsync(Project project);

        public Task<IEnumerable<Project>> GetAllProjectsByCompany(int companyId);

        public Task<IEnumerable<Project>> GetAllProjectsByPriority(int companyId, string priorityName);

        public Task<IEnumerable<BTUser>> GetAllProjectMembersExceptPMAsync(int projectId);

        public Task<IEnumerable<Project>> GetArchivedProjectsByCompany(int companyId);

        public Task<IEnumerable<BTUser>> GetDevelopersOnProjectAsync(int projectId);

        public Task<BTUser> GetProjectManagerAsync(int projectId);

        public Task<IEnumerable<BTUser>> GetProjectMembersByRoleAsync(int projectId, string role);

        public Task<Project> GetProjectByIdAsync(int projectId, int companyId);

        public Task<IEnumerable<BTUser>> GetSubmittersOnProjectAsync(int projectId);

        public Task<IEnumerable<BTUser>> GetUsersNotOnProjectAsync(int projectId, int companyId);

        public Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);

        public Task<bool> IsUserOnProjectAsync(string userId, int projectId);

        public Task<int> LookupProjectPriorityId(string priorityName);

        public Task RemoveProjectManagerAsync(int projectId);

        public Task RemoveUsersFromProjectByRoleAsync(string role, int projectId);

        public Task RemoveUserFromProjectAsync(string userId, int projectId);

        public Task UpdateProjectAsync(Project project);
    }
}
