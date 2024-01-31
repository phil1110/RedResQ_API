using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
    public static class QuizService
    {
        public static Quiz Get(JwtClaims claims, long id)
        {
            if (PermissionService.IsPermitted("getQuiz", claims.Role))
            {
                List<QuizViewRow> quizViewRows = new List<QuizViewRow>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Qz_GetQuiz";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                DataTable quizTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (quizTable.Rows.Count > 0)
                {
                    List<QuizTypeStage> stages = new List<QuizTypeStage>();
                    List<Question> questions = new List<Question>();

                    foreach (DataRow row in quizTable.Rows)
                    {
                        quizViewRows.Add(Converter.ToQuizViewRow(row.ItemArray.ToList()!));
                    }

                    #region LINQ Segments

                    var answerList = quizViewRows.Select(q => new { q.Answer_QuizID, q.Answer_QuestionID, q.Answer_ID, q.Answer_text, q.Answer_isTrue })
                        .GroupBy(q => new { q.Answer_QuizID, q.Answer_QuestionID, q.Answer_ID, q.Answer_text, q.Answer_isTrue })
                        .Select(group => group.First())
                        .ToList();

                    var questionList = quizViewRows.Select(q => new { q.Question_QuizID, q.Question_ID, q.Question_text })
                        .GroupBy(q => new { q.Question_QuizID, q.Question_ID, q.Question_text })
                        .Select(group => group.First())
                        .ToList();

                    var images = quizViewRows.Select(q => new { q.Image_ID, q.Image_Description, q.Image_bytes })
                        .GroupBy(q => new { q.Image_ID, q.Image_Description, q.Image_bytes })
                        .Select(group => group.First())
                        .ToList();

                    var quizTypeStages = quizViewRows.Select(q => new { q.QuizTypeStage_QuizTypeID, q.QuizTypeStage_Stage, q.QuizTypeStage_ImageID })
                        .GroupBy(q => new { q.QuizTypeStage_QuizTypeID, q.QuizTypeStage_Stage, q.QuizTypeStage_ImageID })
                        .Select(group => group.First())
                        .ToList();

                    var quizTypes = quizViewRows.GroupBy(q => new { q.QuizType_ID, q.QuizType_Name })
                        .SelectMany(group => group.Select(q => new { q.QuizType_ID, q.QuizType_Name }))
                        .ToList();

                    var quizData = quizViewRows.Select(q => new { q.Quiz_ID, q.Quiz_Name, q.Quiz_MaxScore, q.Quiz_TypeID })
                        .GroupBy(q => new { q.Quiz_ID, q.Quiz_Name, q.Quiz_MaxScore, q.Quiz_TypeID })
                        .Select(group => group.First())
                        .ToList();

                    #endregion

                    foreach (var stage in quizTypeStages)
                    {
                        var imagesForStage = images.Where(q => q.Image_ID == stage.QuizTypeStage_ImageID).ToList();

                        Image img = new Image(imagesForStage[0].Image_ID, imagesForStage[0].Image_Description, imagesForStage[0].Image_bytes);

                        stages.Add(new QuizTypeStage(stage.QuizTypeStage_QuizTypeID, stage.QuizTypeStage_Stage, img));
                    }

                    foreach (var question in questionList)
                    {
                        List<Answer> answers = new List<Answer>();
                        var answersForQuestion = answerList.Where(q => q.Answer_QuizID == question.Question_QuizID && q.Answer_QuestionID == question.Question_ID).ToList();

                        foreach (var answer in answersForQuestion)
                        {
                            answers.Add(new Answer(answer.Answer_QuizID, answer.Answer_QuestionID, answer.Answer_ID, answer.Answer_text, answer.Answer_isTrue));
                        }

                        questions.Add(new Question(question.Question_QuizID, question.Question_ID, question.Question_text, answers.ToArray()));
                    }

                    QuizType quizType = new QuizType(quizTypes[0].QuizType_ID, quizTypes[0].QuizType_Name, stages.ToArray());

                    return new Quiz(quizData[0].Quiz_ID, quizData[0].Quiz_Name, quizData[0].Quiz_MaxScore, questions.ToArray(), quizType);
                }
            }

            throw new NotFoundException();
        }
    }
}
