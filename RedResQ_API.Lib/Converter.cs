using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib
{
    public static class Converter
    {
        public static Answer ToAnswer(List<object> items)
        {
            int pos = 0;

            long quizId = Convert.ToInt64(items[pos++])!;
            long questionId = Convert.ToInt64(items[pos++])!;
            long id = Convert.ToInt64(items[pos++])!;
            string text = Convert.ToString(items[pos++])!;
            bool isTrue = Convert.ToBoolean(Convert.ToInt16(items[pos])!);

            return new Answer(quizId, questionId, id, text, isTrue);
        }

        public static Article ToArticle(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string title = Convert.ToString(items[pos++])!;
            string content = Convert.ToString(items[pos++])!;
            string author = Convert.ToString(items[pos++])!;
            DateTime date = (DateTime)items[pos++]!;

            Language lang = ToLanguage(items.GetRange(pos, 2));

            pos += 2;

            Image img = ToImage(items.GetRange(pos, 3));

            pos += 3;

            Country co = ToCountry(items.GetRange(pos, 2));

            return new Article(id, title, content, author, date, lang, img, co);
        }

        public static Country ToCountry(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string name = Convert.ToString(items[pos++])!;

            return new Country(id, name);
        }

        public static Gender ToGender(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string name = Convert.ToString(items[pos])!;

            return new Gender(id, name);
        }

        public static Image ToImage(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string description = Convert.ToString(items[pos++])!;
            byte[] bytes = (byte[])items[pos]!;

            return new Image(id, description, bytes);
        }

        public static Language ToLanguage(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string name = Convert.ToString(items[pos])!;

            return new Language(id, name);
        }

        public static Location ToLocation(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string city = Convert.ToString(items[pos++])!;
            string postalCode = Convert.ToString(items[pos++])!;

            Country country = ToCountry(items.GetRange(pos, 2));

            return new Location(id, city, postalCode, country);
        }

        public static Permission ToPermission(List<object> items)
        {
            

            string permName = Convert.ToString(items[pos++])!;

            Role role = ToRole(items.GetRange(pos, 2));

            return new Permission(permName, role);
        }

        public static Question ToQuestion(List<object> items)
        {
            throw new NotImplementedException();
        }

        public static QuizType ToQuizType(List<object> items)
        {
            throw new NotImplementedException();
        }

        public static QuizTypeStage ToQuizTypeStage(List<object> items)
        {
            throw new NotImplementedException();
        }

        internal static Role ToRole(List<object> items)
        {
            int length = row.ItemArray.Length - 1;

            string roleName = Convert.ToString(row.ItemArray[length--])!;
            long roleId = Convert.ToInt64(row.ItemArray[length--]);

            return new Role(roleId, roleName);
        }

        public static User ToUser(List<object> items)
        {
            int length = row.ItemArray.Length - 1;

            string roleName = Convert.ToString(row.ItemArray[length--])!;
            long roleId = Convert.ToInt64(row.ItemArray[length--])!;

            string countryName = Convert.ToString(row.ItemArray[length--])!;
            long countryId = Convert.ToInt64(row.ItemArray[length--])!;

            string postalCode = Convert.ToString(row.ItemArray[length--])!;
            string city = Convert.ToString(row.ItemArray[length--])!;
            long locationId = Convert.ToInt64(row.ItemArray[length--])!;

            string languageName = Convert.ToString(row.ItemArray[length--])!;
            long languageId = Convert.ToInt64(row.ItemArray[length--])!;

            string genderName = Convert.ToString(row.ItemArray[length--])!;
            long genderId = Convert.ToInt64(row.ItemArray[length--])!;

            DateTime birthdate = (DateTime)row.ItemArray[length--]!;
            string email = Convert.ToString(row.ItemArray[length--])!;
            string lastName = Convert.ToString(row.ItemArray[length--])!;
            string firstName = Convert.ToString(row.ItemArray[length--])!;
            string username = Convert.ToString(row.ItemArray[length--])!;
            long id = Convert.ToInt64(row.ItemArray[length--])!;

            Role role = new Role(roleId, roleName);
            Country country = new Country(countryId, countryName);
            Location location = new Location(locationId, city, postalCode, country);
            Language language = new Language(languageId, languageName);
            Gender gender = new Gender(genderId, genderName);

            return new User(id, username, firstName, lastName, email, birthdate, gender, language, location, role);
        }
    }
}
