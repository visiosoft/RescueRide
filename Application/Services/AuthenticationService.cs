﻿using RescueRide.Application.DTOs.Authentication;
using RescueRide.Infrastructure.Repositories;


namespace RescueRide.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IUserRepository userRepository, IOtpService otpService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _otpService = otpService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<TokenResponse> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new TokenResponse { ErrorMessage = "Invalid Email" };
            }
            var isValidPassword = await _userRepository.ValidateUserAsync(email, password);
            if (!isValidPassword)
            {
                return new TokenResponse { ErrorMessage = "Invalid Password" };
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new TokenResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1)
            };
        }

        public async void GenerateOtp(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                throw new Exception("User not found");

            _otpService.SendOtp(username);
        }

        public Task<bool> ValidateOtp(string idToken)
        {
            return _otpService.ValidateOtp(idToken);
        }
    }

}
