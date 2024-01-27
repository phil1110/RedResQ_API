using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Article
	{
		#region Constructor

		public Article(long id, string title, string content, string author, DateTime date, Language language,
			Image image, Country country)
		{
			Id = id;
			Title = title;
			Content = content;
			Author = author;
			Date = date;
			Language = language;
			Image = image;
			Country = country;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string Title { get; private set; }

		public string Content { get; private set; }

		public string Author { get; private set; }

		public DateTime Date { get; private set; }

		public Language Language { get; private set; }

		public Image Image { get; private set; }

		public Country Country { get; private set; }

		#endregion
	}
}
