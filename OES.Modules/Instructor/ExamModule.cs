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

        public Result AddQuestion(Question question)
        {
            Result result = new Result();
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


        public Result UpdateAnswer(Answer answer)
        {
            Result result = new Result();
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


    }
}
