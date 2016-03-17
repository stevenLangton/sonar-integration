using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IObjectivesService
    {
        LinkObjective GetObjective(int id);

        bool UpdateObjective(int id, LinkObjective objectives);

        bool InsertObjective(LinkObjective objectives);
       
        bool DeleteObjective(int id);

        IEnumerable<LinkObjective> GetListOfObjectives(string userId, DateTime year);

        Task<List<LinkObjective>> GetAllObjectives(string userId);

        Task<List<LinkObjective>> GetSharedObjectives(string userId);
        
        void Dispose();
    }
}
