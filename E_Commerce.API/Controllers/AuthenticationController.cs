using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Authentications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

public class AuthenticationController(IAuthenticationService authenticationService) : ApiBaseController
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        return ToActionResult(await _authenticationService.LoginAsync(loginDto, cancellationToken));
    }


    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        return ToActionResult(await _authenticationService.RegisterAsync(registerDto, cancellationToken));
    }


    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email, CancellationToken cancellationToken)
    {
        return ToActionResult(await _authenticationService.CheckEmailAsync(email, cancellationToken));
    }


    [Authorize]
    [HttpGet("currentuser")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        return ToActionResult(await _authenticationService.GetCurrentUserAsync(GetEmailFromToken(), cancellationToken));
    }


    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress(CancellationToken cancellationToken)
    {
        return ToActionResult(await _authenticationService.GetUserAddressAsync(GetEmailFromToken(), cancellationToken));
    }


    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto, CancellationToken cancellationToken)
    {
        return ToActionResult(await _authenticationService.UpdateUserAddressAsync(addressDto, GetEmailFromToken(), cancellationToken));
    }

}
