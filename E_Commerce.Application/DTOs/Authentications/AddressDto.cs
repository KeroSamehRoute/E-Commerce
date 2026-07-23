using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Application.DTOs.Authentications;

public class AddressDto
{

    [Required]
    public string FirstName { get; set; } = default!;


    [Required]
    public string LastName { get; set; } = default!;


    [Required]
    public string City { get; set; } = default!;


    [Required]
    public string Street { get; set; } = default!;


    [Required]
    public string Country { get; set; } = default!;

}
