using AuthenticationAp.Application.DTOs;
using ecommrece.sharedliberary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAp.Application.Interfaces
{
    public interface IUser
    {
       public Task<Response> Register(AppUserDTO appUserDTO);
       public Task<Response> Login(LoginDTO loginDTO);
     public  Task<GetUserDTO> GetUser(int UserId);
    }
}
