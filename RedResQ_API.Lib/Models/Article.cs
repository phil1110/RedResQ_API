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

		#region Methods

		public static Article ConvertToArticle(DataRow row)
		{
			int length = row.ItemArray.Length - 1;

			string countryName = Convert.ToString(row.ItemArray[length--])!;
			long coId = Convert.ToInt64(row.ItemArray[length--])!;

			string base64 = Convert.ToString(row.ItemArray[length--])!;
			long imageId = Convert.ToInt64(row.ItemArray[length--])!;

			string langName = Convert.ToString(row.ItemArray[length--])!;
			long langId = Convert.ToInt64(row.ItemArray[length--])!;

			DateTime date = (DateTime)row.ItemArray[length--]!;
			string author = Convert.ToString(row.ItemArray[length--])!;
			string content = Convert.ToString(row.ItemArray[length--])!;
			string title = Convert.ToString(row.ItemArray[length--])!;
			long id = Convert.ToInt64(row.ItemArray[length--])!;

			Country co = new Country(coId, countryName);
			Image img = new Image(imageId, base64);
			Language lang = new Language(langId, langName);

			return new Article(id, title, content, author, date, lang, img, co);
		}

		#endregion
	}
}
