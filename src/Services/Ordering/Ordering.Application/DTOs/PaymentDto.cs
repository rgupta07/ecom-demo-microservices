using Ordering.Domain.Enums;

namespace Ordering.Application.DTOs;
public record PaymentDto(
	string CardName, 
	string CardNumber, 
	string Expiration, 
	string Cvv,
	PaymentMethod PaymentMethod
);
