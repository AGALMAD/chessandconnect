using chess4connect.DTOs;
using chess4connect.Mappers;
using chess4connect.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace chess4connect.Services
{
    public class AuthService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _tokenParameters;
        private readonly UserMapper _userMapper;

        public AuthService(UnitOfWork unitOfWork, IOptionsMonitor<JwtBearerOptions> jwtOptions, UserMapper userMapper)
        {
            _unitOfWork = unitOfWork;
            _userMapper = userMapper;
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme)
                    .TokenValidationParameters;
        }


        public async Task<User> GetUserFromDbByStringId(string stringId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(Int32.Parse(stringId));
        }

        public async Task<User> GetUserById(int id)
        {
            return await _unitOfWork.UserRepository.GetUserById(id);
        }

        public async Task<User> InsertUser(User user)
        {
            User newUser = await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();
            return newUser;
        }

        public string ObtainToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // EL CONTENIDO DEL JWT
                Claims = new Dictionary<string, object>
                    {
                        { "id", user.Id },
                        { "name", user.UserName },
                        { ClaimTypes.Role, user.Role }
                    },
                Expires = DateTime.UtcNow.AddYears(3),
                SigningCredentials = new SigningCredentials(
                        _tokenParameters.IssuerSigningKey,
                        SecurityAlgorithms.HmacSha256Signature
                    )
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> RegisterUser(UserSignUpDto receivedUser)
        {
            User user = _userMapper.ToEntity(receivedUser);

            PasswordService passwordService = new PasswordService();
            user.Password = passwordService.Hash(receivedUser.Password);

            user.Role = "User";
            User newUser = await InsertUser(user);
            return ObtainToken(newUser);
        }
        public async Task<User> GetUserByCredentialAndPassword(string credential, string password)
        {
            User user = await _unitOfWork.UserRepository.GetUserByCredential(credential);
            if (user == null)
            {
                return null;
            }

            PasswordService passwordService = new PasswordService();
            if (passwordService.IsPasswordCorrect(user.Password, password))
            {
                return user;
            }


            return null;
        }


    }
}
