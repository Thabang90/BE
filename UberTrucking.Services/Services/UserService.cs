using Microsoft.Extensions.Configuration;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Infrastructure.Repositories.Interfaces;
using UberTrucking.Services.Enums;
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
        private readonly IDriverDetailRepository driverDetailRepository;

        public UserService(IUserRepository userRepository, IConfiguration config, IEmailService emailService, IDriverDetailRepository driverDetailRepository)
        {
            this.userRepository = userRepository;
            this.config = config;
            this.emailService = emailService;
            this.driverDetailRepository = driverDetailRepository;
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
                    Phone_Number = request.PhoneNumber,
                    Role_Id = request.RoleId
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

            if(user.Role_Id == (int)UserRole.Driver)
            {
                bool isDriverAvailable = true;
                await this.driverDetailRepository.UpdateDriverStatusAsync(user.Id, isDriverAvailable);
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
