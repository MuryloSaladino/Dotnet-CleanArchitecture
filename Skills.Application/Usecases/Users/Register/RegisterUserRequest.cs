using MediatR;

namespace Skills.Application.Usecases.Users.Register;

public sealed record RegisterUserRequest(
    string Username,
    string Password
) : IRequest<RegisterUserResponse>;
