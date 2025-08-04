using AuthenticationAp.Application.DTOs;
using AuthenticationAp.Application.Interfaces;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAp.Representation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IUser userinterface)  : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
        {
            if (!ModelState.IsValid) BadRequest(ModelState);
            var User = await userinterface.Register(appUserDTO);
            return User.flag ? Ok(User) : BadRequest(User);
      
        }
        [HttpPost("Login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) BadRequest(ModelState);
            var User = await userinterface.Login(loginDTO);
            return User.flag ? Ok(User) : BadRequest(User);

        }
        [HttpGet("{userid:int}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetUser(int userid)
        {

            if (userid <= 0) return BadRequest("User Not Found");
            var User = await userinterface.GetUser(userid);
            return User.Id > 0 ? Ok(User) : NotFound(User);
        }

    }
}
