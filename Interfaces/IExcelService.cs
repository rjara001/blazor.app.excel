using BlazorAppExcel.Components;
using BlazorAppExcel.Models;

namespace BlazorAppExcel.Interfaces
{
    public interface IExcelService
    {
        Task saveAsync(TableExcel model);
        Task<IList<TableExcel>> getTableExcels(string user);

        Task Delete(string user, string id);
    }
}
