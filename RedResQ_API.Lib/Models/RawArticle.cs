using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class RawArticle
	{
		#region Constructor

		public RawArticle(string title, string content, string author, DateTime date, long language,
			long image, long location)
		{
			Title = title;
			Content = content;
			Author = author;
			Date = date;
			Language = language;
			Image = image;
			Location = location;
		}

		#endregion

		#region Properties

		public string Title { get; private set; }

		public string Content { get; private set; }

		public string Author { get; private set; }

		public DateTime Date { get; private set; }

		public long Language { get; private set; }

		public long Image { get; private set; }

		public long Location { get; private set; }

		#endregion
	}
}
