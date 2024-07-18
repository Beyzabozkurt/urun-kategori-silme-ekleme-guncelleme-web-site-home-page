using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StajTest.DBManager;
using StajTest.Services.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace StajTest.Services
{
    [Route("/User", Name = nameof(LoginAppService))]
    [IgnoreAntiforgeryToken]
    public class LoginAppService : ApplicationService 
    {
        private readonly IdentityUserManager _userManager;
        private readonly ICurrentUser _currentUser;
        private readonly JwtOptions _jwtOptions;
        private readonly Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> _signInManager;

        private readonly IDapperManager _dapperManager;

        public LoginAppService(IOptionsSnapshot<JwtOptions> jwtOptions, IDapperManager dapperManager, IdentityUserManager userManager, ICurrentUser currentUser, Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager)
        {
            _jwtOptions = jwtOptions.Value;
            _dapperManager = dapperManager;
            _userManager = userManager;
            _currentUser = currentUser;
            _signInManager = signInManager;
        }

        #region LoginAsync
        [HttpPost]
        [Route("/User/Login", Name = nameof(LoginAsync))]
        public async Task<object> LoginAsync(LoginInput input)
        {
            //var r = input.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(input));

            // _logger.LogInformation("test");
            try
            {
                var result = await _dapperManager.Login(input);

                
                if (result == null) throw new UserFriendlyException("Kullanıcı adı veya şifre yanlış");

                 

                var jwt = await BuildResult(result);

                
                return jwt;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        #endregion

        #region PutAsync ChangePasswordAsync
        [HttpPut]
        [Authorize]
        [Route("/User", Name = nameof(PutAsync))]
        public async Task<object> PutAsync(UserPassword input)
        {
            // _userManager.ErrorDescriber = new TurkishIdentityErrorDescriber();
            var user = await _userManager.FindByNameAsync(_currentUser.UserName);

            //var user = await _userManager.FindByIdAsync(id);

            //var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //var result = await _userManager.ResetPasswordAsync(user, token, input.n);

            var result = await _userManager.ChangePasswordAsync(user, input.o, input.n);

            return result;
            //var newPasswordHash = new Microsoft.AspNetCore.Identity.PasswordHasher<IdentityUser>().HashPassword(user, input.n);

            //var r = await _genericDapper.Execute("update  AbpUsers set PasswordHash = @newPasswordHash where Id = @Id", new { user.Id, newPasswordHash });
            //return r;
        }
        #endregion


        #region BuildResult GenerateJwt

        private async Task<LoginOutput> BuildResult(LoginDto user)
        {
           

            var token = GenerateJwt(user);
            //var loginOutput = ObjectMapper.Map<IdentityUser, LoginOutput>(user);

            
            

            LoginOutput loginOutput = new();
            loginOutput.Id = user.id;
            loginOutput.Name = $"{user.username}";
            loginOutput.UserName = user.username;
            loginOutput.isAuth = !string.IsNullOrWhiteSpace(user.username);
            loginOutput.Token = token;  
            return loginOutput;
        }

        /// <summary>
        /// jwt token
        /// </summary>
        /// <returns></returns>
        private string GenerateJwt(LoginDto user)
        {
            var dateNow = DateTime.Now;
            //var expirationTime = dateNow + TimeSpan.FromHours(_jwtOptions.ExpirationTime);

            var expirationTime = dateNow + TimeSpan.FromDays(360);

            var key = Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Audience, _jwtOptions.Audience),
                new Claim(JwtClaimTypes.Issuer, _jwtOptions.Issuer),
                new Claim(AbpClaimTypes.UserId, user.id.ToString()),
                new Claim(AbpClaimTypes.Name, user.username), 
                new Claim(AbpClaimTypes.UserName, user.username),
                new Claim(AbpClaimTypes.Email, user.email)
            };

           

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        #endregion
    }
}
