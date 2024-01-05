using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Dtos.Project;
using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IVendorRepository _vendorRepository;

        public ProjectService(IProjectRepository projectRepository, IVendorRepository vendorRepository)
        {
            _projectRepository = projectRepository;
            _vendorRepository = vendorRepository;
        }

        public ProjectDto Create(ProjectCreateDto projectCreateDto)
        {
            var userId = int.Parse(projectCreateDto.Id);
            var getVendor = _vendorRepository.GetByAccount(userId);
            if (getVendor == null) return null;
            var project = new Project
            {
                Name = projectCreateDto.Name,
                Status = false,
                VendorId = getVendor.Id,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };

            var newProject = _projectRepository.Create(project);

            var dto = new ProjectDto
            {
                Id = newProject.Id,
                Name = newProject.Name,
                Status = newProject.Status,
                VendorId = newProject.VendorId,
                CreatedAt = newProject.CreatedAt,
                ModifiedAt = newProject.ModifiedAt
            };

            return dto;
        }

        public IEnumerable<ProjectDto> GetAll()
        {
            var projects = _projectRepository.GetAll();
            if (!projects.Any()) return null;

            var dto = (from project in projects
                       select new ProjectDto
                       {
                           Id = project.Id,
                           Name = project.Name,
                           Status = project.Status,
                           VendorId = project.VendorId,
                           CreatedAt = project.CreatedAt,
                           ModifiedAt = project.ModifiedAt
                       }).ToList();

            return dto;
        }

        public ProjectDetailDto Get(int Id)
        {
            var getProject = _projectRepository.GetById(Id);
            if (getProject == null) return null;
            var getVendor = _vendorRepository.GetById(getProject.VendorId);
            if (getVendor == null) return null;

            var dto = new ProjectDetailDto
            {
                Id = getProject.Id,
                Name = getProject.Name,
                Status = getProject.Status,
                VendorId = getVendor.Id,
                NameVendor = getVendor.Name,
                Email = getVendor.Email,
                PhoneNumber = getVendor.PhoneNumber,
                BusinessFields = getVendor.BusinessFields,
                CompanyType = getVendor.CompanyType,
                CreatedAt = getProject.CreatedAt,
                ModifiedAt = getProject.ModifiedAt
            };

            return dto;
        }
    }
}
