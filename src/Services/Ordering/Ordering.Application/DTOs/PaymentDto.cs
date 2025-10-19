using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.DTOs;
public record PaymentDto(
	string CardName, 
	string CardNumber, 
	string Expiration, 
	string Cvv, 
	PaymentMethod PaymentMethod
);
