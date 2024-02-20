
using BlazorAppExcel.Components;
using BlazorAppExcel.Models;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class Util
{

    public static TableExcel getDataTableFromSheet(ISheet sheet, string IdUser)
    {

        IRow row = sheet.GetRow(0);

        int cc = row.LastCellNum>=12?12:row.LastCellNum;

        TableExcel dt = new TableExcel(sheet.SheetName, IdUser);

        for (int i = 0; i < cc; i++)
        {
            ICell cell = row.GetCell(i);
            
            if (cell?.ToString().Length > 0)
                dt.setColumns(cell.ToString(), i);
        }

        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
        {
            var r = sheet.GetRow(i);
            RowExcel _row = new RowExcel();
            dt.Rows.Add(_row);

            int index = 0;
            if (r != null)
                for (int j = r.FirstCellNum; j < cc; j++)
                {
                    index++;
                    if (r.GetCell(j) != null)
                    {
                        var item = r.GetCell(j).ToString();
                        if (item!=null)
                            _row.setValue(item);
                    }
                }
        }

        return dt;
    }

    public static async Task<IList<TableExcel>> getDataSetAsync(InputFileChangeEventArgs e)
    {
        IList<ISheet> sheets = await getDataSetFromStreamAsync(e);

        // DataSet ds = new DataSet();
        IList<TableExcel> ds = new List<TableExcel>();

        foreach (var item in sheets)
        {
            ds.Add(getDataTableFromSheet(item, "User"));
        }

        return ds;
    }

    private static async Task<IList<ISheet>> getDataSetFromStreamAsync(InputFileChangeEventArgs e)
    {
        string sFileExtension = Path.GetExtension(e.File.Name).ToLower();

        IList<ISheet> sheets = new List<ISheet>();

        using (var fileStream = e.File.OpenReadStream())
        using (MemoryStream ms = new MemoryStream())
        {
            await fileStream.CopyToAsync(ms);

            ms.Position = 0;

            IWorkbook hssfwb = getWBFromType(sFileExtension, ms); // new HSSFWorkbook(ms);

            var numberSheets = hssfwb.NumberOfSheets;
            for (int i = 0; i < numberSheets; i++)
            {
                sheets.Add(hssfwb.GetSheetAt(i));
            }

        }

        return sheets;
    }

    private static IWorkbook getWBFromType(string type, MemoryStream ms)
    {
        return type == ".xls" ? new HSSFWorkbook(ms) : new XSSFWorkbook(ms);
    }
}