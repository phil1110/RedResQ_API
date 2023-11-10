using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Credentials
	{
		#region Instance Variables

		private string _identifier;
		private string _secret;

		#endregion

		#region Constructor

		public Credentials(string identifier, string secret)
		{
			Identifier = identifier;
			Secret = secret;
		}

		#endregion

		#region Properties

		public string Identifier
		{
			get => _identifier;
			internal set => _identifier = value;
		}

		public string Secret
		{
			get => _secret;
			internal set => _secret = value;
		}

		#endregion
	}
}
