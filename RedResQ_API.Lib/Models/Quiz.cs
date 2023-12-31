﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class Quiz
    {

        #region Constructor

        public Quiz(long id, string name, int maxScore, Question[] questions)
        {
            Id = id;
            Name = name;
            MaxScore = maxScore;
            Questions = questions;
        }

        #endregion

        #region Properties

        public long Id { get; private set; }

        public string Name { get; private set; }

        public int MaxScore { get; private set; }

        public Question[] Questions { get; private set; }

        #endregion
    }
}
