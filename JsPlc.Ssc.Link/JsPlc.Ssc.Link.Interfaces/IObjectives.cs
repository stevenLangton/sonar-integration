using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Interfaces
{
    public interface IObjectives
    {
        
        Objectives GetObjective(int id);
       
        bool UpdateObjective(int id, Objectives objectives);
       
        bool InsertObjective(Objectives objectives);
       
        bool DeleteObjective(int id);

        IEnumerable<Objectives> GetListOfObjectives(int userId, DateTime year);
        
        void Dispose();
        
    }
}
