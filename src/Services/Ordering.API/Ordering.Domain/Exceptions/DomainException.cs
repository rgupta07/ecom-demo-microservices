using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Exceptions
{
	internal class DomainException : Exception
	{
		public DomainException(string message) 
			: base($"Exception thrown from Ordering.Domain with message: {message}")
		{
		}
	}
}
