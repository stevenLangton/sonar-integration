using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IObjectivesService
    {
        Objectives GetObjective(int id);
       
        bool UpdateObjective(int id, Objectives objectives);
       
        bool InsertObjective(Objectives objectives);
       
        bool DeleteObjective(int id);

        IEnumerable<Objectives> GetListOfObjectives(int userId, DateTime year);
        
        void Dispose();
        
    }
}
