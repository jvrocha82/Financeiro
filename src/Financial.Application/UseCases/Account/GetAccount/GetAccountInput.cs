using MediatR;

namespace Financial.Application.UseCases.Account.GetAccount;
public class GetAccountInput : IRequest<GetAccountOutput>
{
    public GetAccountInput(Guid id)
    => Id = id;
    

    public Guid Id { get; set; }
}
