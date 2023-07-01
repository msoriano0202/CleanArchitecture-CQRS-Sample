using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //private readonly UserManager<IdentityUser> _userManager; //--refresh Token
        //private readonly SignInManager<IdentityUser> _signInManager; //--refresh Token
        private readonly JwtSettings _jwtSettings;

        //// --- refresh token ---
        //private readonly CleanArchitectureIdentityDbContext _context;
        //private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IOptions<JwtSettings> jwtSettings//,
            //CleanArchitectureIdentityDbContext context,
            //TokenValidationParameters tokenValidationParameters
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            //_context = context;
            //_tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception($"El usuario con email {request.Email} no existe");
            }

            var resultado = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!resultado.Succeeded)
            {
                throw new Exception($"Las credenciales son incorrectas");
            }

            var token = await GenerateToken(user);
            var authResponse = new AuthResponse
            {
                Id = user.Id,
                //Token = token.Item1, // --refresh token
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email,
                Username = user.UserName//,
                //RefreshToken = token.Item2 // --refresh token
            };

            return authResponse;
        }

        //public async Task<AuthResponse> RefreshToken(TokenRequest request)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var tokenValidationParamsClone = _tokenValidationParameters.Clone();
        //    tokenValidationParamsClone.ValidateLifetime = false;

        //    var authResponse = new AuthResponse();

        //    try
        //    {
        //        // validation: el formato del token es correcto
        //        var tokenVerification = jwtTokenHandler.ValidateToken(
        //            request.Token, 
        //            tokenValidationParamsClone, 
        //            out var validatedToken);

        //        // validation: verifica encriptacion
        //        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        //        {
        //            var result = jwtSecurityToken.Header.Alg.Equals(
        //                SecurityAlgorithms.HmacSha256, 
        //                StringComparison.InvariantCultureIgnoreCase);

        //            if (!result)
        //            {
        //                authResponse.Errors!.Add("El Token tiene errores de encriptacion");
        //                return authResponse;
        //            }

        //        }

        //        // validation: verificar fecha de expiracion
        //        var utcExpireDate = long.Parse(
        //            tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value
        //            );

        //        var expireDate = UnixTimeStampToDateTime(utcExpireDate);

        //        if (expireDate > DateTime.UtcNow)
        //        {
        //            authResponse.Errors!.Add("El token ha expirado");
        //            return authResponse;
        //        }

        //        // validation: el refrhes token exits en la dase de datos
        //        var storedToken = await _context.RefreshToken!.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);
        //        if (storedToken is null)
        //        {
        //            authResponse.Errors!.Add("El token no existe");
        //            return authResponse;
        //        }

        //        // validation: is el token ya fue usado
        //        if (storedToken.IsUsed)
        //        {
        //            authResponse.Errors!.Add("El token ya fue usado");
        //            return authResponse;
        //        }

        //        // validation: si el token fue revocado
        //        if (storedToken.IsRevoked)
        //        {
        //            authResponse.Errors!.Add("El token ha sido revocado");
        //            return authResponse;
        //        }

        //        // validation: validar el id dle token
        //        var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        //        if (storedToken.JwtId != jti)
        //        {
        //            authResponse.Errors!.Add("El token no concuerda con el valor inicial");
        //            return authResponse;
        //        }

        //        // validation: segunda validacion para fecha de expiracion
        //        if (storedToken.ExpiredDate < DateTime.UtcNow)
        //        {
        //            authResponse.Errors!.Add("El refresh token ha expirado");
        //            return authResponse;
        //        }

        //        storedToken.IsUsed = false;
        //        _context.RefreshToken!.Update(storedToken);
        //        await _context.SaveChangesAsync();

        //        var user = await _userManager.FindByIdAsync(storedToken.UserId);
        //        var token = await GenerateToken(user);

        //        authResponse.Id = user.Id;
        //        authResponse.Token = token.Item1;
        //        authResponse.Email = user.Email;
        //        authResponse.Username = user.UserName;
        //        authResponse.RefreshToken = token.Item2;
        //        authResponse.Success = true;

        //        return authResponse;
        //    }
        //    catch (Exception ex) 
        //    {
        //        if (ex.Message.Contains("Lifetime validation failed. The token is expired"))
        //        {
        //            authResponse.Errors!.Add("El token ha expirado, porfavor tienes que volver a realizar el login");
        //            return authResponse;
        //        }
        //        else 
        //        {
        //            authResponse.Errors!.Add("EL token tiene errores, tienes que volver a hacer el login");
        //            return authResponse;
        //        }
        //    }
        //}

        //private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        //{
        //    var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
        //    return dateTimeVal;
        //}

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                throw new Exception($"El username ya fue tomado por otra cuenta");
            }

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                throw new Exception($"El email ya fue tomado por otra cuenta");
            }

            //var user = new IdentityUser // -- refresh token
            var user = new ApplicationUser
            {
                Email = request.Email,
                Nombre = request.Nombre,
                Apellidos = request.Apellidos//,
                // -- refresh token
                //UserName = request.Username,
                //EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                // -- refresh token
                //var applicationUser = new ApplicationUser 
                //{
                //    IdentityId = new Guid(user.Id),
                //    Nombre = request.Nombre,
                //    Apellidos = request.Apellidos,
                //    Country = request.Country,
                //    Email = request.Email,
                //    Phone = request.Phone
                //};
                //_context.ApplicationUsers!.Add(applicationUser);
                //await _context.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, "Operator");
                var token = await GenerateToken(user);
                return new RegistrationResponse
                {
                    Email = user.Email,
                    //Token = token.Item1, // -- refresh token
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = user.Id,
                    Username = user.UserName//,
                    //RefreshToken = token.Item2 // -- refresh token
                };
            }

            throw new Exception($"{result.Errors}");

        }


        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid, user.Id)
            }.Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        //// -- refresh token
        //private async Task<Tuple<string, string>> GenerateToken(IdentityUser user) 
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    var roles = await _userManager.GetRolesAsync(user);

        //    var roleClaims = new List<Claim>();
        //    foreach (var role in roles)
        //    {
        //        roleClaims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim("Id", user.Id),
        //            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //        }.Union(userClaims).Union(roleClaims)),
        //        Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
        //        SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        //    var jwtToken = jwtTokenHandler.WriteToken(token);

        //    var refreshToken = new RefreshToken
        //    {
        //        JwtId = token.Id,
        //        IsUsed = false,
        //        IsRevoked = false,
        //        UserId = user.Id,
        //        CreatedDate = DateTime.UtcNow,
        //        ExpiredDate = DateTime.UtcNow.AddMonths(6),
        //        Token = $"{GenerateRandomTokenCharacters(35)}{Guid.NewGuid()}"
        //    };

        //    await _context.RefreshToken!.AddAsync(refreshToken);
        //    await _context.SaveChangesAsync();

        //    return new Tuple<string, string>(jwtToken, refreshToken.Token);
        //}

        //private string GenerateRandomTokenCharacters(int length)
        //{
        //    var random = new Random();
        //    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        //    var str = Enumerable.Repeat(chars, length).Select(x => x[random.Next(length)]);
        //    return new string(str.ToArray());
        //}
    }
}
