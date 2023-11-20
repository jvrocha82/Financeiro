using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Application.UseCases.User.UpdateUser;
public class UpdateUserInput : IRequest<UpdateUserOutput>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public UpdateUserInput(Guid id,string name, bool isActive = true)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }
}
