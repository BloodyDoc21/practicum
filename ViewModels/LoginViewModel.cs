using System.ComponentModel.DataAnnotations;

namespace CleanLife.Web.ViewModels;

public class LoginViewModel
{
    [Display(Name = "Имя пользователя")]
    [Required]
	public required string Username { get; set; }

    [Display(Name = "Пароль")]
    [Required]
	[DataType(DataType.Password)]
	public required string Password { get; set; }

    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; }
}