using chess4connect.Mappers;
using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
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
            //Retorna nulo si el usuario es nulo, si introduce un email en el nombre de usuario o si no introduce un email correcto
            if (receivedUser == null || !IsEmail(receivedUser.Email) || IsEmail(receivedUser.UserName))
                return null;

            User user = _userMapper.ToEntity(receivedUser);

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


        public bool IsEmail(string email)
        {
            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            return validateEmailRegex.IsMatch(email);
        }


    }
}
