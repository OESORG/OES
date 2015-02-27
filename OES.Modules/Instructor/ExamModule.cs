using OES.Data;
using OES.Model.Examination;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

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
                .Include(c => c.Questions.Select(q => q.Chapter))
                .Include(c => c.Registration)
                .Include(c => c.Registration.Semester)
                .Include(c => c.Registration.Course)
                .FirstOrDefault(c => c.ChapterId.Equals(chapterId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
