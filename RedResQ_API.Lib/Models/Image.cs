using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Image
	{
		#region Constructor

		public Image(long id, string source)
		{
			Id = id;
			Source = source;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string Source { get; private set; }

		#endregion
	}
}
