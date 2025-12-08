using Backend.Shared.Classes;

namespace Backend.Application.Interfaces.Validators;

public interface IEmailValidator
{
    Result Validate(string email);
}
