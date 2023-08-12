using System.ComponentModel.DataAnnotations;
using MassTransit;


public class EmployeeModel
{
    [Required]
    [StringLength(maximumLength: 25, MinimumLength = 2)]
    public string FirstName {get; set;}

    [Required]
    [StringLength(maximumLength: 25, MinimumLength = 2)]
    public string LastName {get; set;}
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber {get; set;}
    public string Address {get; set;}

    [Required]
    public string Gender{get; set;}

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email {get; set;}

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }
}


