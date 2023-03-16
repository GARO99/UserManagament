using FluentValidation;
using UserManagement.Models.ViewModels;

namespace UserManagement.UsesCases.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(u => u.FirstName).Cascade(CascadeMode.Stop).NotNull().WithMessage("El nombre es requerido")
                .NotEmpty().WithMessage("El nombre es requerido");
            RuleFor(u => u.LastName).Cascade(CascadeMode.Stop).NotNull().WithMessage("El nombre es requerido")
                .NotEmpty().WithMessage("El apellido es requerido");
            RuleFor(u => u.Email).Cascade(CascadeMode.Stop).NotNull().WithMessage("El nombre es requerido").
                NotEmpty().WithMessage("El correo electronico es requerido")
                .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").WithMessage("El correo electronico no es valido");
            RuleFor(u => u.Phone).Cascade(CascadeMode.Stop).Must(x => x != 0).WithMessage("El teléfono es requerido")
                .Must(ValidatePhoneNumber).WithMessage("El número de teléfono es inválido");
            RuleFor(u => u.Birthdate).Must(DateNotDeafult).WithMessage("La fecha de nacimiento es requerida");
        }

        private bool ValidatePhoneNumber(long phone)
        {
            var phoneString = phone.ToString();

            return !string.IsNullOrEmpty(phoneString) && (phoneString.Length == 7 || phoneString.Length == 10)
                && phoneString.All(char.IsDigit);
        }

        private bool DateNotDeafult(DateTime date)
        {
            return date != default;
        }
    }
}
