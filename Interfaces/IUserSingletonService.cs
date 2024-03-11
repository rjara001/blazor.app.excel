using BlazorAppExcel.Models;

namespace BlazorAppExcel.Interfaces
{
    public interface ISessionSingletonService
    {
        public User User { get; }

        Task SetUser(User user);

        void SetTableActive(TableExcel table);
    }
}
