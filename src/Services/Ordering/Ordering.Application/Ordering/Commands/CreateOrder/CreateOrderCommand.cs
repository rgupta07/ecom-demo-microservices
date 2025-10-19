using BuildingBlocks.CQRS;
using Ordering.Application.DTOs;
using Ordering.Domain.Entities;

namespace Ordering.Application.Ordering.Commands.CreateOrder;
public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid OrderId);
