using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class Quiz
    {

        #region Constructor

        public Quiz(long id, string name, int maxScore, Question[] questions, QuizType type)
        {
            Id = id;
            Name = name;
            MaxScore = maxScore;
            Questions = questions;
            Type = type;
        }

        #endregion

        #region Properties

        public long Id { get; set; }

        public string Name { get; set; }

        public int MaxScore { get; set; }

        public Question[] Questions { get; private set; }

        public QuizType Type { get; private set; }

        #endregion
    }
}
