using OES.Data;
using OES.Model.Examination;
using OES.Modules.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace OES.Modules.Common
{
    public class GenerateExamModule
    {

        public Result<ExamVersion> GenerateExamVersion(string id)
        {
            OESData db = new OESData();
            var exam = db.Exams
                .Include(e => e.Registration)
                .Include(e => e.Versions)
                .FirstOrDefault(e => e.ExamId.Equals(id, StringComparison.OrdinalIgnoreCase));
            Result<ExamVersion> result = new Result<ExamVersion>();
            var questions = db.Questions.Include(q => q.Answers)
                .Include(q => q.Chapter).
                Where(q => q.Chapter.RegistrationId.Equals(exam.RegistrationId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            result.Errors = ValidateQuestions(exam, questions);
            result.ReturnObject = new ExamVersion { 
                RegistrationId = exam.RegistrationId
            };

            result.Success = result.Errors.Count < 1;
            if (result.Success)
            {
                ExamVersion version = WrapToExamVersion(exam);
                version.Questions = PickupQuestions(version, questions);
                exam.Versions.Add(version);
                result.ReturnObject = version;
                db.SaveChanges();
            }
            return result;
        }

        private List<ResultError> ValidateQuestions(Exam exam, List<Question> questions)
        {
            List<ResultError> errors = new List<ResultError>();
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.High)) < exam.NumberOfHighQuestion)
            {
                errors.Add(new ResultError("", "There is no enough high questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Medium)) < exam.NumberOfMediumQuestion)
            {
                errors.Add(new ResultError("", "There is no enough medium questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Low)) < exam.NumberOfLowQuestion)
            {
                errors.Add(new ResultError("", "There is no enough low questions to generate this exam."));
            }
            foreach (var q in questions.Where(q => q.Type != QuestionType.TrueFalse && q.Answers.Count < 4))
            {
                errors.Add(new ResultError("", string.Format("Chapter: \"{0}\"Question: \"{1}\" with less than four answers.", q.Chapter.Title, q.QuestionText)));
            }
            foreach (var q in questions.Where(q => q.Answers.Count(a => a.IsCorrectAnswer) < 1))
            {
                errors.Add(new ResultError("", string.Format("Chapter: \"{0}\"Question: \"{1}\" should have at least one correct answer.", q.Chapter.Title, q.QuestionText)));
            }
            return errors;
        }

        private ExamVersion WrapToExamVersion(Exam exam)
        {
            return new ExamVersion()
            {
                NumberOfHighQuestion = exam.NumberOfHighQuestion,
                NumberOfLowQuestion = exam.NumberOfLowQuestion,
                NumberOfMediumQuestion = exam.NumberOfMediumQuestion,
                HighQuestionScore = exam.HighQuestionScore,
                MediumQuestionScore = exam.MediumQuestionScore,
                LowQuestionScore = exam.LowQuestionScore,
                StartDate = exam.StartDate,
                EndDate = exam.EndDate,
                Registration = exam.Registration,
                RegistrationId = exam.RegistrationId,
                Exam = exam,
                ExamId = exam.ExamId
            };
        }
        private List<QuestionVersion> PickupQuestions(ExamVersion exam, List<Question> questions)
        {
            var finalQuestions = new List<QuestionVersion>();
            var chapters = questions.Select(q => q.Chapter).OrderBy(c => c.Number).ToList();
            var highQuestions = questions.Where(q => q.Difficulty.Equals(QuestionDifficulty.High)).ToList();
            var mediumQuestions = questions.Where(q => q.Difficulty.Equals(QuestionDifficulty.Medium)).ToList();
            var lowQuestions = questions.Where(q => q.Difficulty.Equals(QuestionDifficulty.Low)).ToList();


            var tempQuestions = new List<QuestionVersion>();
            for (int i = 0; i < exam.NumberOfHighQuestion; i++)
            {
                foreach (var chapter in chapters)
                {
                    var question = highQuestions.FirstOrDefault(q => q.ChapterId.Equals(chapter.ChapterId, StringComparison.OrdinalIgnoreCase));
                    if (question != null)
                    {
                        highQuestions.Remove(question);
                        tempQuestions.Add(WrapToQuestionVersion(question));
                        break;
                    }
                }
            }
            for (int i = 0; i < exam.NumberOfMediumQuestion; i++)
            {
                foreach (var chapter in chapters)
                {
                    var question = mediumQuestions.FirstOrDefault(q => q.ChapterId.Equals(chapter.ChapterId, StringComparison.OrdinalIgnoreCase));
                    if (question != null)
                    {
                        mediumQuestions.Remove(question);
                        tempQuestions.Add(WrapToQuestionVersion(question));
                        break;
                    }
                }
            }


            for (int i = 0; i < exam.NumberOfLowQuestion; i++)
            {
                foreach (var chapter in chapters)
                {
                    var question = lowQuestions.FirstOrDefault(q => q.ChapterId.Equals(chapter.ChapterId, StringComparison.OrdinalIgnoreCase));
                    if (question != null)
                    {
                        lowQuestions.Remove(question);
                        tempQuestions.Add(WrapToQuestionVersion(question));
                        break;
                    }
                }
            }
            List<int> selectedRandoms = new List<int>();
            for (int i = 0; i < tempQuestions.Count; i++)
            {
                Random r = new Random();
                int random = r.Next(0, tempQuestions.Count);
                while (selectedRandoms.Contains(random))
                {
                    r = new Random();
                    random = r.Next(0, tempQuestions.Count);
                }
                selectedRandoms.Add(random);
                finalQuestions.Add(tempQuestions[random]);
            }

            return finalQuestions;
        }


        private QuestionVersion WrapToQuestionVersion(Question question)
        {
            var version = new QuestionVersion()
            {
                Type = question.Type,
                QuestionText = question.QuestionText,
                Difficulty = question.Difficulty,
                Chapter = question.Chapter,
                ChapterId = question.ChapterId

            };
            version.Answers = new List<AnswerVersion>();
            if (question.Type.Equals(QuestionType.TrueFalse))
            {
                version.Answers = question.Answers.Select(a => WrapToAnswerVersion(a)).ToList();
            }
            else
            {
                List<Answer> answers = new List<Answer>();
                answers.AddRange(question.Answers.Where(a => a.IsCorrectAnswer));
                if (answers.Count < 4)
                {
                    answers.AddRange(question.Answers.Where(a => !a.IsCorrectAnswer).Take(4 - answers.Count));
                }
                List<int> selectedRansdom = new List<int>();
                for (int i = 0; i < answers.Count; i++)
                {
                    Random rand = new Random();
                    int random = rand.Next(0, answers.Count);
                    while (selectedRansdom.Contains(random))
                    {
                        rand = new Random();
                        random = rand.Next(0, answers.Count);
                    }
                    selectedRansdom.Add(random);
                    version.Answers.Add(WrapToAnswerVersion(answers[random]));
                }

            }
            return version;
        }

        private AnswerVersion WrapToAnswerVersion(Answer answer)
        {
            return new AnswerVersion()
            {
                AnswerText = answer.AnswerText,
                IsCorrectAnswer = answer.IsCorrectAnswer
            };
        }
    }
}
