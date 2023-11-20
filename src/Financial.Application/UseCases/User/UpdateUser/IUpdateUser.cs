using MediatR;

namespace Financial.Application.UseCases.User.UpdateUser;
public interface IUpdateUser
    :IRequestHandler<UpdateUserInput, UpdateUserOutput>
{
}
