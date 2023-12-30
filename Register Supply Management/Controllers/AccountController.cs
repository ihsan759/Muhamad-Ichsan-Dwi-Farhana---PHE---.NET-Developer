using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Register_Supply_Management.Dtos.Account;
using Register_Supply_Management.Model.Data;
using Register_Supply_Management.Services;
using Register_Supply_Management.Utilities.Enum;
using Register_Supply_Management.Utilities.Handlers;
using System.Net;

namespace Register_Supply_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountSevices;

        public AccountController(AccountService accountSevices)
        {
            _accountSevices = accountSevices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            var registerResult = await _accountSevices.Register(registerDto);
            if (registerResult == null)
            {
                return NotFound(new ResponseHandlers<RegisterDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Register Failed"
                });
            }
            return Ok(new ResponseHandlers<Account>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Register Successfully",
                Data = registerResult
            });
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var loginResult = _accountSevices.Login(loginDto);
            if (loginResult == "0")
            {
                return Unauthorized(new ResponseHandlers<String>
                {
                    Code = StatusCodes.Status401Unauthorized,
                    Status = HttpStatusCode.Unauthorized.ToString(),
                    Message = "Incorrect email or password"
                });
            }
            else if (loginResult == "1")
            {
                return new ObjectResult(new ResponseHandlers<String>
                {
                    Code = StatusCodes.Status403Forbidden,
                    Status = HttpStatusCode.Forbidden.ToString(),
                    Message = "Account is inactive"
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else
            {
                return Ok(new ResponseHandlers<String>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Login Successfully",
                    Data = loginResult
                });
            }
        }

        [HttpPut("Delete")]
        [Authorize]
        public IActionResult Delete()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var approvalResult = _accountSevices.Delete(userId);
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
                        Message = "Delete Account Successfully"
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

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateAccountDto updateAccountDto)
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var updateResult = await _accountSevices.Update(updateAccountDto, userId);
                if (updateResult == 2)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<string>
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = "Error retrieving data from the database"
                    });
                }
                else if (updateResult == 1)
                {
                    return Ok(new ResponseHandlers<string>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Update Account Successfully"
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
            var approvalResult = _accountSevices.Approval(Id);
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

        [HttpGet("User")]
        [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
        public IActionResult GetAccountUser()
        {
            var userResult = _accountSevices.GetAccountUser();
            if (userResult == null)
            {
                return NotFound(new ResponseHandlers<AccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new ResponseHandlers<IEnumerable<AccountDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = userResult
            });
        }

        [HttpGet("Vendor")]
        [Authorize(Roles = $"{nameof(RoleLevel.Admin)}, {nameof(RoleLevel.Manager)}")]
        public IActionResult GetAccountVendor()
        {
            var vendorResult = _accountSevices.GetAccountVendor();
            if (vendorResult == null)
            {
                return NotFound(new ResponseHandlers<AccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new ResponseHandlers<IEnumerable<AccountDto>>
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
                var getAccount = _accountSevices.Get(userId);
                if (getAccount == null)
                {
                    return NotFound(new ResponseHandlers<AccountDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data Not Found"
                    });
                }
                return Ok(new ResponseHandlers<AccountDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Data Found",
                    Data = getAccount
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
    }
}
