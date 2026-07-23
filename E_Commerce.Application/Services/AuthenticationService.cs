using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Authentications;

namespace E_Commerce.Application.Services;

internal class AuthenticationService(IIdentityService identityService, ITokenService tokenService) : IAuthenticationService
{
    private readonly IIdentityService _identityService = identityService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<bool>> CheckEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _identityService.EmailExistsAsync(email, cancellationToken);
    }

    public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.FindByEmailAsync(email, cancellationToken);

        if (!result.IsSuccess)
        {
            return Result<UserDto>.Fail(result.Errors);
        }

        var user = result.Data;

        var rolesResult = await _identityService.GetRolesAsync(email, cancellationToken);

        if (!rolesResult.IsSuccess)
        {
            return Result<UserDto>.Fail(rolesResult.Errors);
        }

        var roles = rolesResult.Data;

        if (user.Email is null)
        {
            return Result<UserDto>.Fail(Error.Unauthorized("User email is missing"));
        }

        if (user.UserName is null)
        {
            return Result<UserDto>.Fail(Error.Unauthorized("User username is missing"));
        }

        var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles);

        return new UserDto { DisplayName = user.DisplayName, Email = user.Email, Token = token };
    }

    public async Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.GetAddressByEmailAsync(email, cancellationToken);

        if (!result.IsSuccess)
        {
            return Result<AddressDto>.Fail(result.Errors);
        }

        return result.Data;
    }

    public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var userResult = await _identityService.FindByEmailAsync(loginDto.Email, cancellationToken);

        if (!userResult.IsSuccess)
        {
            return Result<UserDto>.Fail(userResult.Errors);
        }

        var passwordResult = await _identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, cancellationToken);

        if (!passwordResult.IsSuccess)
        {
            return Result<UserDto>.Fail(Error.Unauthorized("Invalid Email or Password"));
        }

        var rolesResult = await _identityService.GetRolesAsync(loginDto.Email, cancellationToken);

        if (!rolesResult.IsSuccess)
        {
            return Result<UserDto>.Fail(rolesResult.Errors);
        }

        var roles = rolesResult.Data;

        var user = userResult.Data;

        if (user.Email is null)
        {
            return Result<UserDto>.Fail(Error.Unauthorized("User email is missing"));
        }

        if (user.UserName is null)
        {
            return Result<UserDto>.Fail(Error.Unauthorized("User username is missing"));
        }

        var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles);

        return new UserDto
        {
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = token
        };
    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.CreateUserAsync(registerDto, cancellationToken);

        if (!result.IsSuccess || result.Data is null)
        {
            return Result<UserDto>.Fail(result.Errors);
        }

        if (result.Data.Email is null)
        {
            return Result<UserDto>.Fail(Error.Unauthorized("User email is missing"));
        }

        return new UserDto { Email = result.Data.Email, DisplayName = result.Data.DisplayName, Token = "Token" };
    }

    public async Task<Result<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto, string email, CancellationToken cancellationToken = default)
    {
        return await _identityService.UpSertAddressAsync(email, addressDto, cancellationToken);
    }

}
