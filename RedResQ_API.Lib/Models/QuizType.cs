using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class QuizType
    {
        #region Constructor

        [JsonConstructor]
        public QuizType(long id, string name, QuizTypeStage[]? stages = null)
        {
            Id = id;
            Name = name;
            Stages = stages ?? Array.Empty<QuizTypeStage>();
        }

        #endregion

        #region Properties

        [JsonRequired]
        public long Id { get; set; }

        [JsonRequired]
        public string Name { get; set; }

        public QuizTypeStage[]? Stages { get; private set; }

        #endregion
    }
}
