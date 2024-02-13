using BlazorAppExcel.Components;
using BlazorAppExcel.Models;

namespace BlazorAppExcel.Interfaces
{
    public interface IExcelService
    {
        Task saveAsync(TableExcel model);
    }
}
