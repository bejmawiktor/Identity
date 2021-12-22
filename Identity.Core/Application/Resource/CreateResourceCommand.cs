using MediatR;
using System;

namespace Identity.Core.Application
{
    public record CreateResourceCommand(
        string ResourceName,
        string ResourceDescription,
        Guid UserId)
    : IRequest;
}