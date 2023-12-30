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

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleLevel.Vendor)}")]
        public IActionResult Create(VendorCreateDto vendorCreateDto)
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var vendorResult = _vendorService.Create(vendorCreateDto, userId);
                if (vendorResult == null)
                {
                    return BadRequest(new ResponseHandlers<string>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Data not invalid"
                    });
                }
                {
                    return Ok(new ResponseHandlers<VendorDetailDto>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Data added successfully",
                        Data = vendorResult
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
    }
}
