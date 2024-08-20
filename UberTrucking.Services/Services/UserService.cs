using UberTrucking.Infrastructure.Entities;
using UberTrucking.Infrastructure.Repositories.Interfaces;
using UberTrucking.Services.Helpers;
using UberTrucking.Services.Models;
using UberTrucking.Services.Services.Interfaces;

namespace UberTrucking.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task CreateUserAsync(UserRequest request)
        {
            var user = new User()
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Password = PasswordHasher.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                RoleId = request.RoleId
            };

            await this.userRepository.CreateUserAsync(user);
        }

        public async Task<UserResponse> GetUserByEmailAsync(LoginRequest request)
        {
            var user = await this.userRepository.RetrieveUserByEmailAsync(request.Email);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.Password))
            {
                return new UserResponse
                {
                    ErrorMessage = "Invalid username or password"
                };
            }

            return new UserResponse
            {
                User = user
            };
        }
    }
}
