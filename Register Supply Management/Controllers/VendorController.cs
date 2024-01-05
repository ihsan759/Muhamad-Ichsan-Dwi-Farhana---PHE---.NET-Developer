using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Register_Supply_Management.Dtos.Vendor;
using Register_Supply_Management.Services;
using Register_Supply_Management.Utilities.Enum;
using Register_Supply_Management.Utilities.Handlers;
using System.Net;

namespace Register_Supply_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly VendorService _vendorService;

        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpPut("Update")]
        [Authorize(Roles = $"{nameof(RoleLevel.Vendor)}")]
        public async Task<IActionResult> Update([FromForm] VendorUpdateDto vendorUpdateDto)
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var vendorResult = await _vendorService.Update(vendorUpdateDto, userId);
                if (vendorResult == 2)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<string>
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = "Error retrieving data from the database"
                    });
                }
                else if (vendorResult == 1)
                {
                    return Ok(new ResponseHandlers<string>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Data updated Successfully"
                    });
                }
                else
                {
                    return NotFound(new ResponseHandlers<string>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data not Found"
                    });
                }
            }
            else
            {
                return BadRequest(new ResponseHandlers<string>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not invalid"
                });
            }
        }

        [HttpPost("Approval")]
        [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
        public IActionResult Approval(int Id)
        {
            var approvalResult = _vendorService.Approval(Id);
            if (approvalResult == 2)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });
            }
            else if (approvalResult == 1)
            {
                return Ok(new ResponseHandlers<string>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Approve Successfully"
                });
            }
            else
            {
                return NotFound(new ResponseHandlers<string>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not Found"
                });
            }
        }

        [HttpGet("Admin")]
        [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
        public IActionResult GetAdmin()
        {
            var vendorResult = _vendorService.GetAdmin();
            if (vendorResult == null)
            {
                return NotFound(new ResponseHandlers<VendorDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new ResponseHandlers<IEnumerable<VendorDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = vendorResult
            });
        }

        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            var vendorResult = _vendorService.GetAll();
            if (vendorResult == null)
            {
                return NotFound(new ResponseHandlers<VendorDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new ResponseHandlers<IEnumerable<VendorDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = vendorResult
            });
        }

        [HttpGet("Get")]
        [Authorize]
        public IActionResult Get()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var vendorResult = _vendorService.Get(userId);
                if (vendorResult == null)
                {
                    return NotFound(new ResponseHandlers<VendorDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data Not Found"
                    });
                }
                return Ok(new ResponseHandlers<VendorDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Data Found",
                    Data = vendorResult
                });
            }
            else
            {
                return BadRequest(new ResponseHandlers<string>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not invalid"
                });
            }
        }

        [HttpGet("Photo/{id}")]
        public IActionResult GetPhoto(int id)
        {
            var fileResult = _vendorService.Photo(id);
            if (fileResult == null)
            {
                return NotFound(new ResponseHandlers<string>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return fileResult;
        }
    }
}
