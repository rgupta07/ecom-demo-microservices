using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions
{
	internal interface IEntity<T> : IEntity
	{
		public T Id { get; set; }
	}

	internal interface IEntity
	{
		DateTime? CreatedAt { get; set; }
		string? CreatedBy { get; set; }
		DateTime? LastModifiedAt { get; set; }
		string? LastModifiedBy { get; set; }
	}
}
