using OES.Data;
using OES.Model.Examination;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using OES.Modules.Core;

namespace OES.Modules.Instructor
{
    public class ExamModule
    {
        #region Chapter
        public List<Question> GetChapterQuestions(string chapterId)
        {
            OESData db = new OESData();
            return db.Questions.Include(q => q.Chapter).Where(q => q.ChapterId.Equals(chapterId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Chapter GetChapterById(string chapterId)
        {
            OESData db = new OESData();
            return db.Chapters.Include(c => c.Questions)
                .Include(c => c.Questions.Select(q => q.Answers))
                .Include(c => c.Questions.Select(q => q.Chapter))
                .Include(c => c.Registration)
                .Include(c => c.Registration.Semester)
                .Include(c => c.Registration.Course)
                .FirstOrDefault(c => c.ChapterId.Equals(chapterId, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Question
        public List<Answer> GetAnswers(string questionId)
        {
            OESData db = new OESData();
            return db.Answers.Where(a => a.QuestionId.Equals(questionId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Question GetQuestion(string questionId)
        {
            OESData db = new OESData();
            return db.Questions.Include(q => q.Answers)
                .Include(q => q.Chapter)
                .Include(q => q.Chapter.Registration)
                .FirstOrDefault(q => q.QuestionId.Equals(questionId, StringComparison.OrdinalIgnoreCase));
        }

        public Result<Question> AddQuestion(Question question)
        {
            var result = new Result<Question>();
            OESData db = new OESData();
            try
            {

                db.Questions.Add(question);
                if (question.Type.Equals(QuestionType.TrueFalse))
                {
                    question.Answers = question.Answers ?? new List<Answer>();
                    question.Answers.Add(new Answer()
                    {
                        QuestionId = question.QuestionId,
                        Question = question,
                        AnswerText = "True",
                        IsCorrectAnswer = true
                    });
                    question.Answers.Add(new Answer()
                    {
                        QuestionId = question.QuestionId,
                        Question = question,
                        AnswerText = "False"
                    });
                }
                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = question;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = question;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(){ Key="", Message = ex.Message}
                };
            }
            return result;
        }


        public Result<Answer> UpdateAnswer(Answer answer)
        {
            Result<Answer> result = new Result<Answer>();
            OESData db = new OESData();
            try
            {
                var dbAnswer = db.Answers.FirstOrDefault(a => a.AnswerId.Equals(answer.AnswerId, StringComparison.OrdinalIgnoreCase));
                dbAnswer.AnswerText = answer.AnswerText;
                dbAnswer.IsCorrectAnswer = answer.IsCorrectAnswer;
              
                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = dbAnswer;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = answer;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(){ Key="", Message = ex.Message}
                };
            }
            return result;
        }
        public Result<Question> UpdateQuestion(Question question)
        {
            Result<Question> result = new Result<Question>();
            OESData db = new OESData();
            try
            {
                var dbQuestion = db.Questions.FirstOrDefault(a => a.QuestionId.Equals(question.QuestionId, StringComparison.OrdinalIgnoreCase));
                dbQuestion.QuestionText = question.QuestionText;
                dbQuestion.Difficulty = question.Difficulty;
                dbQuestion.Type = question.Type;

                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = dbQuestion;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = question;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(){ Key="", Message = ex.Message}
                };
            }
            return result;
        }

        #endregion

        #region Exam
        public Registration GetRegistrationForExams(string registrationId)
        {
            OESData db = new OESData();
            return db.Registrations.Include(r => r.Course)
                .Include(r=> r.Semester)
                .Include(r => r.Exams)
                .FirstOrDefault(r => r.RegistrationId.Equals(registrationId, StringComparison.OrdinalIgnoreCase));
        }


        public Result<Exam> AddExam(Exam exam)
        {
            var result = new Result<Exam>();
            OESData db = new OESData();
            try
            {
                var errors = ValidateExam(exam);
                if (errors.Count > 0)
                {
                    result.Errors = new List<ResultError>(errors);
                    result.Success = false;
                }
                else
                {
                    db.Exams.Add(exam);
                    db.SaveChanges();
                    db.Dispose();
                    result.Success = true;
                    result.ReturnObject = exam; 
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = exam;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(ex)
                };
            }
            return result;

        }

        private List<ResultError> ValidateExam(Exam exam)
        {
            List<ResultError> errors = new List<ResultError>();
            if (exam.StartDate <= DateTime.Now)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.StartDate, "Exam start date should be in the future."));
            }
            if (exam.EndDate <= exam.StartDate)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.EndDate, "Exam end date should be after the start date."));
            }
            if ((exam.NumberOfHighQuestion + exam.NumberOfMediumQuestion + exam.NumberOfLowQuestion) < 1)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.NumberOfHighQuestion, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.NumberOfMediumQuestion, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.NumberOfLowQuestion, "Number of questions should be more than zero."));
            }
            if (exam.NumberOfHighQuestion > 0 && exam.HighQuestionScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.HighQuestionScore, "Score shoud be greater than zero."));
            }
            if (exam.NumberOfMediumQuestion > 0 && exam.MediumQuestionScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.MediumQuestionScore, "Score shoud be greater than zero."));
            }
            if (exam.NumberOfLowQuestion > 0 && exam.LowQuestionScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.LowQuestionScore, "Score shoud be greater than zero."));
            }

            return errors;
        }


        #endregion

    }
}
