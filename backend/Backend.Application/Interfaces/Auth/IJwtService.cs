using Backend.Domain.Entities.Users;

namespace Backend.Application.Interfaces.Auth;

public interface IJwtService
{
    string IssueAccessToken(UserEntity user);
}
