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

        #region Methods

        public static QuizTypeStage ConvertToQuizTypeStage(DataRow row)
        {
            int length = row.ItemArray.Length - 1;

            string base64 = Convert.ToString(row.ItemArray[length--])!;
            long imageId = Convert.ToInt64(row.ItemArray[length--])!;

            int stage = Convert.ToInt32(row.ItemArray[length--])!;
            long quizTypeId = Convert.ToInt64(row.ItemArray[length--])!;

            Image img = new Image(imageId, base64);

            return new QuizTypeStage(quizTypeId, stage, img);
        }

        #endregion
    }
}
