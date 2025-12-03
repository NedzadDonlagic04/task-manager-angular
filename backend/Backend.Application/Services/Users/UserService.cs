using Backend.Application.Interfaces.Database;
using Backend.Application.Interfaces.Users;

namespace Backend.Application.Services.Users;

public sealed class UserService(IAppDbContext context) : IUserService { }
