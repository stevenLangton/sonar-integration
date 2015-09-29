using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public class LinkRepository
    {
        private RepositoryContext db;

        public LinkRepository(RepositoryContext context) { db = context; }

        public IEnumerable<Question> GetQuestions()
        {
            return db.Questions.ToList();
        }

        public Employee GetEmployee(int id)
        {
            return db.Employees.FirstOrDefault(e => e.Id == id);
        }

        public void SaveAnswers(IEnumerable<Answer> answers)
        {
            foreach (var answer in answers)
            {
                db.Answers.Add(answer);
            }
            db.SaveChanges();
        }
    }
}
