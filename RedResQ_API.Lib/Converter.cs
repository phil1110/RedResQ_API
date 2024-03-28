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

            Image img = null!;

            if (items[pos] is not DBNull)
            {
                img = ToImage(items.GetRange(pos, 3));
            }

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

        public static Hazard ToHazard(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string title = Convert.ToString(items[pos++])!;
            double latitude = Convert.ToDouble(items[pos++])!;
            double longitude = Convert.ToDouble(items[pos++])!;
            int radius = Convert.ToInt32(items[pos++])!;
            DateTime timestamp = (DateTime)items[pos++]!;

            int typeId = Convert.ToInt32(items[pos++])!;
            string typeName = Convert.ToString(items[pos])!;

            var type = new HazardType(typeId, typeName);

            return new Hazard(id, title, latitude, longitude, radius, timestamp, type);
        }

        public static HazardType ToHazardType(List<object> items)
        {
            int pos = 0;

            int id = Convert.ToInt32(items[pos++])!;
            string name = Convert.ToString(items[pos])!;

            return new HazardType(id, name);
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
            int pos = 0;

            string permName = Convert.ToString(items[pos++])!;

            Role role = ToRole(items.GetRange(pos, 2));

            return new Permission(permName, role);
        }

        public static Question ToQuestion(List<object> items, Answer[] answers)
        {
            int pos = 0;

            long quizId = Convert.ToInt64(items[pos++])!;
            long id = Convert.ToInt64(items[pos++])!;
            string text = Convert.ToString(items[pos])!;

            return new Question(quizId, id, text, answers);
        }

        public static QuestionViewRow ToQuestionViewRow(List<object> items)
        {
            int pos = 0;

            long question_quizId = Convert.ToInt64(items[pos++])!;
            long question_id = Convert.ToInt64(items[pos++])!;
            string question_text = Convert.ToString(items[pos++])!;

            long answer_quizId = Convert.ToInt64(items[pos++])!;
            long answer_questionId = Convert.ToInt64(items[pos++])!;
            long answer_id = Convert.ToInt64(items[pos++])!;
            string answer_text = Convert.ToString(items[pos++])!;
            bool answer_isTrue = Convert.ToBoolean(Convert.ToInt16(items[pos])!);

            return new QuestionViewRow(question_quizId, question_id, question_text, answer_quizId, answer_questionId, answer_id, answer_text, answer_isTrue);
        }

        public static Quiz ToQuiz(List<object> items, Question[] questions, QuizType quizType)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string name = Convert.ToString(items[pos++])!;
            int maxScore = Convert.ToInt32(items[pos])!;

            return new Quiz(id, name, maxScore, questions, quizType);
        }

        public static QuizType ToQuizType(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string name = Convert.ToString(items[pos])!;

            return new QuizType(id, name);
        }

        public static QuizViewRow ToQuizViewRow(List<object> items)
        {
            int pos = 0;

            long quiz_id = Convert.ToInt64(items[pos++])!;
            string quiz_name = Convert.ToString(items[pos++])!;
            int quiz_maxScore = Convert.ToInt32(items[pos++])!;
            long quiz_typeId = Convert.ToInt64(items[pos++])!;

            long quizType_id = Convert.ToInt64(items[pos++])!;
            string quizType_name = Convert.ToString(items[pos++])!;

            long question_quizId = Convert.ToInt64(items[pos++])!;
            long question_id = Convert.ToInt64(items[pos++])!;
            string question_text = Convert.ToString(items[pos++])!;

            long answer_quizId = Convert.ToInt64(items[pos++])!;
            long answer_questionId = Convert.ToInt64(items[pos++])!;
            long answer_id = Convert.ToInt64(items[pos++])!;
            string answer_text = Convert.ToString(items[pos++])!;
            bool answer_isTrue = Convert.ToBoolean(Convert.ToInt16(items[pos])!);

            return new QuizViewRow(quiz_id, quiz_name, quiz_maxScore, quiz_typeId, 
                quizType_id, quizType_name, question_quizId, question_id, question_text, 
                answer_quizId, answer_questionId, answer_id, answer_text, answer_isTrue);
        }

        internal static Role ToRole(List<object> items)
        {
            int pos = 0;

            long roleId = Convert.ToInt64(items[pos++]);
            string roleName = Convert.ToString(items[pos])!;

            return new Role(roleId, roleName);
        }

        public static User ToUser(List<object> items)
        {
            int pos = 0;

            long id = Convert.ToInt64(items[pos++])!;
            string username = Convert.ToString(items[pos++])!;
            string firstName = Convert.ToString(items[pos++])!;
            string lastName = Convert.ToString(items[pos++])!;
            string email = Convert.ToString(items[pos++])!;
            DateTime birthdate = (DateTime)items[pos++]!;

            long genderId = Convert.ToInt64(items[pos++])!;
            string genderName = Convert.ToString(items[pos++])!;

            long languageId = Convert.ToInt64(items[pos++])!;
            string languageName = Convert.ToString(items[pos++])!;

            long locationId = Convert.ToInt64(items[pos++])!;
            string city = Convert.ToString(items[pos++])!;
            string postalCode = Convert.ToString(items[pos++])!;

            long countryId = Convert.ToInt64(items[pos++])!;
            string countryName = Convert.ToString(items[pos++])!;

            long roleId = Convert.ToInt64(items[pos++])!;
            string roleName = Convert.ToString(items[pos])!;

            Gender gender = new Gender(genderId, genderName);
            Language language = new Language(languageId, languageName);
            Country country = new Country(countryId, countryName);
            Location location = new Location(locationId, city, postalCode, country);
            Role role = new Role(roleId, roleName);

            return new User(id, username, firstName, lastName, email, birthdate, gender, language, location, role);
        }
    }
}
