using BlazorAppExcel.Components;
using BlazorAppExcel.Models;

namespace BlazorAppExcel.Interfaces
{
    public interface IExcelService
    {
        Task SetUser(User user, TableExcel table);
        //Task<IList<TableExcel>> getTableExcels(string user);

        Task<User> GetUser(string user);

        Task Delete(string user, string id);
    }
}
