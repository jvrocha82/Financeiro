using MediatR;

namespace Financial.Application.UseCases.User.GetUser;
public interface IGetUser : IRequestHandler<GetUserInput, GetUserOutput>
{ }
