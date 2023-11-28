using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public interface IUser
	{
		string Username { get; }

		string Email { get; }

		Role Role { get; }
	}
}
