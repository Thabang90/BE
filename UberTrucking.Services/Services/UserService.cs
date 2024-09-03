using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration config;
        private readonly IEmailService emailService;

        public UserService(IUserRepository userRepository, IConfiguration config, IEmailService emailService)
        {
            this.userRepository = userRepository;
            this.config = config;
            this.emailService = emailService;
        }

        public async Task<UserResponse> CreateUserAsync(UserRequest request)
        {
            var userResponse = new UserResponse();
            var user = await this.userRepository.RetrieveUserByEmailAsync(request.Email);
            if(user != null)
            {
                userResponse.ErrorMessage = "User already exists, please use a different email!";
            }
            else
            {
                user = new User()
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Email = request.Email,
                    Password = PasswordHasher.HashPassword(request.Password),
                    PhoneNumber = request.PhoneNumber,
                    RoleId = request.RoleId
                };

                await this.userRepository.CreateUserAsync(user);

                userResponse.Message = "User has been successfully created!";
            }
            
            return userResponse;
        }

        public async Task<UserResponse> LoginAsync(LoginRequest request)
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

        public async Task<UserResponse> SendResetLinkAsync(string email)
        {
            var user = await this.userRepository.RetrieveUserByEmailAsync(email);
            if(user is null)
                return new UserResponse { ErrorMessage = "Email not found. Please Verify!" };

            var fromEmail = this.config["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBuilder.BuildForgotPasswordEmailBody(email));
            this.emailService.SendEmail(emailModel);

            return new UserResponse { Message = "Email has been sent with a link to reset your password!" };
        }
    }
}
