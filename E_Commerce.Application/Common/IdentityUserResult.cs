namespace E_Commerce.Application.Common;

public sealed class IdentityUserResult(string id, string? email, string? userName, string displayName)
{
    public string Id { get; set; } = id;
    public string? Email { get; set; } = email;
    public string? UserName { get; set; } = userName;
    public string DisplayName { get; set; } = displayName;
}
