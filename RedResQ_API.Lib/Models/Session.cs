using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Session
	{
		#region Constructor

		public Session(long id, string deviceId, User person)
		{
			Id = id;
			DeviceId = deviceId;
			Person = person;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string DeviceId { get; private set; }

		public User Person { get; private set; }

		#endregion
	}
}
