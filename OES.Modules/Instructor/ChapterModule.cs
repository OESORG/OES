using OES.Data;
using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Modules.Instructor
{
    public class ChapterModule
    {
        private OESData db = new OESData();
        public ChapterModule() { }

        public void Add(Chapter chapter)
        {
            db.Chapters.Add(chapter);
            db.SaveChanges();
        }
    }
}
