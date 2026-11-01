using AuthenticationProvider.Application.Dtos.AppUserDtos;
using AuthenticationProvider.Application.Dtos.TokenDtos;
using AuthenticationProvider.Application.Errors;
using AuthenticationProvider.Domain.Entities;
using AuthenticationProvider.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationProvider.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRequestClock _requestClock;

    public AuthenticationService(IAppUserRepository appUserRepository, IPasswordHasher<AppUser> passwordHasher, ITokenService tokenService, IRequestClock requestClock)
    {
        _appUserRepository = appUserRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _requestClock = requestClock;
    }

    public async Task<Result<AccessTokenDto>> RegisterAsync(RegisterAppUserForm form, CancellationToken cancellationToken = default)
    {
        if(form.Password != form.VerifyPassword)
        {
            return Result.Failure<AccessTokenDto>(
                AppError.Validation(nameof(AppUser), 
                    [ValidationErrorEntry.PasswordAreNotTheSame(nameof(form.Password), nameof(form.VerifyPassword))]));
        }
        var overlapUsers = await _appUserRepository.GetUsersByUsernameOrEmailAsync(form.Username, form.Email, cancellationToken);

        if (overlapUsers.Count != 0)
        {
            var validationErrorEntries = new List<ValidationErrorEntry>();
            overlapUsers.ForEach(user =>
            {
                if(user.Email == form.Email)
                    validationErrorEntries.Add(ValidationErrorEntry.AlreadyExistWith(nameof(AppUser), nameof(AppUser.Email)));
                if(user.Username == form.Username)
                    validationErrorEntries.Add(ValidationErrorEntry.AlreadyExistWith(nameof(AppUser), nameof(AppUser.Username)));
            });
            return Result.Failure<AccessTokenDto>(AppError.Validation(nameof(AppUser), validationErrorEntries));;
        }

        var newUser = new AppUser(Guid.CreateVersion7(), 
        form.Username, form.Email, _passwordHasher.HashPassword(null!, form.Password),
        _requestClock.UtcNow, _requestClock.UtcNow, AppUserRole.User);

        await _appUserRepository.SaveUserAsync(newUser, cancellationToken);

        return Result.Success(_tokenService.GenerateToken(newUser.Id, newUser.Username, newUser.Role));
    }

    public async Task<Result<AccessTokenDto>> LoginAsync(LoginAppUserForm form, CancellationToken cancellationToken = default)
    {
        var user = await _appUserRepository.GetUserByUsernameAsync(form.Username, cancellationToken);

        if(user == null || user.PasswordHash == null)
        {
            _ = _passwordHasher.HashPassword(null!, form.Password);
            return Result.Failure<AccessTokenDto>(UnauthorizedError.InvalidUsernameOrPassword());
        }

        var hashingResult = 
            _passwordHasher.VerifyHashedPassword(null!, user.PasswordHash, form.Password);

        if(hashingResult == PasswordVerificationResult.Failed)
        {
            return Result.Failure<AccessTokenDto>(UnauthorizedError.InvalidUsernameOrPassword());
        }

        return Result.Success(_tokenService.GenerateToken(user.Id, user.Username, user.Role));
    }
}