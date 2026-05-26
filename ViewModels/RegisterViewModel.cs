using System.ComponentModel.DataAnnotations;

namespace CleanLife.Web.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Имя пользователя")]
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Username { get; set; }

    [Display(Name = "Электронная почта")]
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Display(Name = "Пароль")]
    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Подтверждение пароля")]
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public required string ConfirmPassword { get; set; }
}