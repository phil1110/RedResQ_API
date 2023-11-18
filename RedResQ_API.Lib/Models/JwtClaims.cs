using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class JwtClaims
	{
		#region Constructor

		public JwtClaims(string id, string username, string email, int role)
		{
			Id = id;
			Username = username;
			Email = email;
			Role = role;
		}

		#endregion

		#region Properties

		public string Id { get; set; }

		public string Username { get; private set; }

		public string Email { get; private set; }

		public int Role { get; private set; }

		#endregion
	}
}
