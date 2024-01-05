using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Register_Supply_Management.Dtos.Project;
using Register_Supply_Management.Services;
using Register_Supply_Management.Utilities.Enum;
using Register_Supply_Management.Utilities.Handlers;
using System.Net;

namespace Register_Supply_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpPost("Create")]
        [Authorize(Roles = $"{nameof(RoleLevel.Vendor)}")]
        public IActionResult Create(ProjectCreateDto projectCreateDto)
        {
            var createResult = _projectService.Create(projectCreateDto);
            if (createResult == null)
            {
                return NotFound(new ResponseHandlers<ProjectDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Create data Failed"
                });
            }
            return Ok(new ResponseHandlers<ProjectDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Create data Successfully",
                Data = createResult
            });
        }



        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            var vendorResult = _projectService.GetAll();
            if (vendorResult == null)
            {
                return NotFound(new ResponseHandlers<ProjectDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new ResponseHandlers<IEnumerable<ProjectDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = vendorResult
            });
        }

        [HttpGet("Get/{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            var vendorResult = _projectService.Get(id);
            if (vendorResult == null)
            {
                return NotFound(new ResponseHandlers<ProjectDetailDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new ResponseHandlers<ProjectDetailDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = vendorResult
            });
        }
    }
}
