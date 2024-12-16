using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Contracs;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password);