using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions
{
	public class NotFoundException: Exception
	{
		public NotFoundException(): base() { }

		public NotFoundException(string name, object key): base($"Entity \"{name}\"  ({key}) was not found.") { }
		public NotFoundException(string name, object key, string selector): base($"Entity \"{name}\" was not found using {selector} - {key}.") { }

	}
}
