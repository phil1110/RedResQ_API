using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class QuizTypeStage
    {
        #region Constructor

        public QuizTypeStage(long quizTypeId, int stage, Image image)
        {
            QuizTypeID = quizTypeId;
            Stage = stage;
            Image = image;
        }

        #endregion

        #region Properties

        public long QuizTypeID { get; private set; }

        public int Stage { get; private set; }

        public Image Image { get; private set; }

        #endregion
    }
}
