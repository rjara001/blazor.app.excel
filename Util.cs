
using BlazorAppExcel.Components;
using BlazorAppExcel.Models;
using BlazorAppExcel.Share.Enums;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;

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
            ds.Add(getDataTableFromSheet(item, "user"));
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

    public static void LoadUniqueValues(TableExcel tableExcel)
    {
        for (int i = 0; i < tableExcel.Types.Count; i++)
        {
            var type = (ExcelCellType)tableExcel.Types[i];
            if (type == ExcelCellType.Unique)
                calculateUniqueValues(i, tableExcel);
        }
    }

    private static void calculateUniqueValues(int index, TableExcel tableExcel)
    {
        var column = tableExcel.Columns[$"Column{index+1}"];

        try
        {
            if (tableExcel.UniqueValues.ContainsKey(column))
                return;

            var uniqueValues = tableExcel.Rows.Select(row => {
                return row.Values[index];
            }
            ).ToList();

            // Convert the result to a list000000000000
            List<string> uniqueList = uniqueValues.ToList();

            var list = uniqueList.Distinct().ToList();


            tableExcel.UniqueValues.Add(column, list);
        }
        catch (Exception e)
        {

            throw;
        }
      
    }

    public static string ExtractNumberFromString(string input, ExcelCellType type)
    {
      
        if (input.StartsWith(","))
        {
            input = "0" + input;
        }

        if (type == ExcelCellType.Porcentage)
        {
            input = input.Replace("%", string.Empty);
        }

        if (type == ExcelCellType.Currency) {
            input = input.Replace("$", string.Empty);    
        }

       
        return input;
    }
}