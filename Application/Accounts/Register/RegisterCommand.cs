using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.Register
{
    public class RegisterCommand
    {
        public record RegisterCommandRequest(
            RegisterRequest RegisterRequest) : IRequest<Result<Profile>>;

        internal class RegisterCommandHandler :
            IRequestHandler<RegisterCommandRequest, Result<Profile>>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly ITokenService _tokenService;

            public RegisterCommandHandler(ITokenService tokenService,
                                          UserManager<AppUser> userManager)
            {
                _tokenService = tokenService;
                _userManager = userManager;
            }

            public async Task<Result<Profile>> Handle(RegisterCommandRequest request,
                                                      CancellationToken cancellationToken)
            {
                if (await _userManager.Users.AnyAsync(x => x.Email ==
                                                           request.RegisterRequest.Email))
                {
                    Result<Profile>.Failure("El email ya fue registrado por otro usuario.");
                }

                if (await _userManager.Users.AnyAsync(x => x.UserName ==
                                                           request.RegisterRequest.Username))
                {
                    Result<Profile>.Failure("El username ya fue registrado");
                }

                var user = new AppUser()
                {
                    NombreCompleto = request.RegisterRequest.NombreCompleto,
                    Id = Guid.NewGuid().ToString(),
                    Carrera = request.RegisterRequest.Carrera,
                    UserName = request.RegisterRequest.Username,
                    Email = request.RegisterRequest.Email,
                };

                var resultado =
                    await _userManager.CreateAsync(user, request.RegisterRequest.Password!);

                if (resultado.Succeeded)
                {
                    var profile = new Profile
                    {
                        Email = user.Email,
                        NombreCompleto = user.NombreCompleto,
                        Token = await _tokenService.CreateToken(user),
                        Username = user.UserName,
                    };

                    return Result<Profile>.Success(profile);
                }
                
                return Result<Profile>.Failure("Ocurrió un error al registrar al usuario.");
            }
        }
    }
}