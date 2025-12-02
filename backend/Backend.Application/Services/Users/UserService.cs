using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Users;

namespace Backend.Application.Services.Users;

public sealed class UserService(IAppDbContext context) : IUserService { }
