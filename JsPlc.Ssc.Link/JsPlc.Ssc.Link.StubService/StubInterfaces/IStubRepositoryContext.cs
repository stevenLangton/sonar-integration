using System.Data.Entity;
using JsPlc.Ssc.Link.StubService.StubModels;

namespace JsPlc.Ssc.Link.StubService.StubInterfaces
{
    public interface IStubRepositoryContext
    {
        IDbSet<StubColleague> Colleagues { get; }
    }
}
