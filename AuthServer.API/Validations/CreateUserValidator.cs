using AuthServer.Core.DTOs;
using FluentValidation;

namespace AuthServer.API.Validations
{
    public class CreateUserValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.MailAddress).NotEmpty().WithMessage("Mail Adresi Girilmesi Zorunludur.").EmailAddress().WithMessage("Girdiğiniz Mail Adresi 'Email' Tipine Uygun Değildir!");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı Adı Girilmesi Zorunludur");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Girilmesi Zorunludur");
        }
    }
}
