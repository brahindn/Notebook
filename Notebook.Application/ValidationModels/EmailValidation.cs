using Microsoft.Extensions.DependencyInjection;
using Notebook.Application.Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Notebook.Application.ValidationModels
{
    public class EmailValidation : ValidationAttribute
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public EmailValidation(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value != null)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                    if (service.ContactService.GetContactByFieldAsync(
                        firstName: null, lastName: null, phoneNumber: null, email: value.ToString()) != null)
                    {
                        return new ValidationResult("Email already exists");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
