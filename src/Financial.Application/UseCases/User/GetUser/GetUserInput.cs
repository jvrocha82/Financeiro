using MediatR;

namespace Financial.Application.UseCases.User.GetUser;
public class GetUserInput : IRequest<GetUserOutput>
{

    public Guid Id { get; set; }

    public GetUserInput(Guid id)
    => Id = id;

}
