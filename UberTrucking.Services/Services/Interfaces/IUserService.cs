using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Services.Models;

namespace UberTrucking.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserRequest request);

        Task<UserResponse> GetUserByEmailAsync(LoginRequest request);
    }
}
