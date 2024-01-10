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

		public JwtClaims(TokenType tokenType, long id, string username, string email, long role, DateTime expiryDate)
		{
			TokenType = tokenType;
			Id = id;
			Username = username;
			Email = email;
			Role = role;
			ExpiryDate = expiryDate;
		}

		public JwtClaims(TokenType tokenType, long role, DateTime expiryDate)
		{
			TokenType=tokenType;
			Id = -1;
			Username = null!;
			Email = null!;
			Role = role;
			ExpiryDate = expiryDate;
		}

		#endregion

		#region Properties

		public TokenType TokenType { get; set; }

		public long Id { get; set; }

		public string Username { get; private set; }

		public string Email { get; private set; }

		public long Role { get; private set; }

		public DateTime ExpiryDate { get; set; }

		#endregion
	}
}
