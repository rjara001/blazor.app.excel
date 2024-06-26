
using BlazorAppExcel.Components;
using BlazorAppExcel.Models;
using BlazorAppExcel.Share.Enums;
using BlazorAppExcel.Share.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using static MudBlazor.CategoryTypes;

public class Util
{

    public static TableExcel getDataTableFromSheet(ISheet sheet, string IdUser, string nameFile)
    {

        IRow row = null;
        for (int i = 0; i < 20; i++)
        {
            if (row == null)
                row = sheet.GetRow(i);
            else
                break;
        }

        int numColumns = row.LastCellNum>=12?12:row.LastCellNum;

        TableExcel dt = new TableExcel("", sheet.SheetName, IdUser);
        dt.FileName = nameFile;
        dt.DateCreation = DateTime.Now;

        for (int i = 0; i < numColumns; i++)
        {
            ICell cell = row.GetCell(i);
            
            if (cell?.ToString().Length > 0)
                dt.setColumns(cell.ToString());
        }
        // fix the size
        numColumns = dt.Columns.Count;

        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
        {
            var rowSheet = sheet.GetRow(i);
            RowExcel rowExcel = new RowExcel();
           
            dt.Rows.Add(rowExcel);

            int index = 0;
            if (rowSheet != null)
                for (int j = rowSheet.FirstCellNum; j < numColumns; j++)
                {
                    index++;
               
                    string item = rowSheet.GetCell(j)==null?String.Empty: rowSheet.GetCell(j).ToString();
                      
                    rowExcel.setValue(item);
                    
                }
        }

        return dt;
    }

    public static async Task<IList<TableExcel>> getDataSetAsync(InputFileChangeEventArgs e, IList<TableExcel> tables)
    {
        IList<ISheet> sheets = await getDataSetFromStreamAsync(e);

        // DataSet ds = new DataSet();
        IList<TableExcel> ds = new List<TableExcel>();

        foreach (var item in sheets)
        {
            var newTable = getDataTableFromSheet(item, "user", e.File.Name);

            tables = Util.CheckTablesMatchedAt(newTable, tables);
   
            ds.Add(newTable);
        }

        return ds;
    }
    private static TableExcel MergesTable(TableExcel newTable, TableExcel table)
    {
        foreach (var row in table.Rows)
        {
            newTable.Rows.Add(row);
        }

        return newTable;
    }
    private static IList<TableExcel> CheckTablesMatchedAt(TableExcel newTable, IList<TableExcel> tables)
    {
        int index = 0;
        Column column = null;
        TableExcel table = null;

        while (index < tables.Count() && column == null)
        {

            table = tables[index];

            column = table.Columns.FirstOrDefault(item =>
                                                          newTable.Columns.Any(newItem =>
                                                              string.Equals(newItem.Key, item.Key, StringComparison.OrdinalIgnoreCase)));
            index++;
        }

        if (column == null)
        {
            newTable.Upload.IsTablesMatched = false;
        }
        else
        {
            newTable.Upload.IsTablesMatched = true;

            newTable.Upload.TableMached = table.Name;
        }

        return tables;


    }

    public static string GetName(IDictionary<string, TableExcel> tables, TableExcel table, string newName)
    {
        string nameCode = table.GenerateCodeName(newName, true);
        int index = 0;
        while (tables.ContainsKey(nameCode) && index<20)
        {
            nameCode = table.GenerateCodeName(nameCode, true);
            index++;    
        }
           
        return nameCode;
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

                IWorkbook hssfwb = getWBFromType(sFileExtension, ms);

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

  

    public static string ExtractNumberFromString(string input, ExcelCellType type)
    {
      
        if (input.StartsWith(","))
        {
            input = "0" + input;
        }

        if (type == ExcelCellType.Percentage)
        {
            input = input.Replace("%", string.Empty);
        }

        if (type == ExcelCellType.Currency) {
            input = input.Replace("$", string.Empty);    
        }

       
        return input;
    }
    public static bool CheckTypes(CellExcel cellExcel)
    {
        switch ((ExcelCellType)cellExcel.Type)
        {
            case ExcelCellType.String:
                // No validation for string type
                return true;
            case ExcelCellType.Number:
                return IsNumber(cellExcel.Value);
            case ExcelCellType.DateTime:
                return IsDateTime(cellExcel.Value);
            case ExcelCellType.Boolean:
                return IsBoolean(cellExcel.Value);
            case ExcelCellType.Decimal:
                return IsDecimal(cellExcel.Value);
            case ExcelCellType.Unique:
                return true;
            case ExcelCellType.Currency:
                return IsCurrency(cellExcel.Value);
            case ExcelCellType.Percentage:
                return IsPercentage(cellExcel.Value);
            case ExcelCellType.Period:
                return IsPeriod(cellExcel.Value);
            default:
                // For unsupported types, consider them valid
                return true;
        }
    }

    public static bool IsNumber(string value)
    {
        return double.TryParse(value, out _);
    }

    public static bool IsDateTime(string value)
    {
        return DateTime.TryParse(value, out _);
    }

    public static bool IsBoolean(string value)
    {
        return bool.TryParse(value, out _);
    }

    public static bool IsDecimal(string value)
    {
        return decimal.TryParse(value, out _);
    }

    public static bool IsCurrency(string value)
    {

        return IsDecimal(value);
    }

    public static bool IsPercentage(string value)
    {

        return IsDecimal(value);
    }

    public static bool IsPeriod(string value)
    {

        return false;
    }

    public static string getDSToExcel(TableExcel table)
    {
    
        using (MemoryStream memoryStream = new MemoryStream())
        {
           
            IWorkbook wb = new XSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            ICreationHelper cH = wb.GetCreationHelper();

            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < table.Columns.Count; i++)
            {

                ICell cell = row.CreateCell(i);
                var cellTtext = table.Columns[i].Key;
                cell.SetCellValue(cH.CreateRichTextString(cellTtext));
            }

            for (int i = 0; i < table.Rows.Count; i++)
            {
                RowExcel _tableRow = table.Rows[i];

                row = sheet.CreateRow(i+1);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    for (int k = 0; k < _tableRow.Values.Count;k++)
                    {
                        var item = _tableRow.Values[k];

                        ICell cell = row.CreateCell(k);
                        var cellTtext = item.ToString();
                        cell.SetCellValue(cH.CreateRichTextString(cellTtext));
                    }
                    
                }
            }
            wb.Write(memoryStream);
            //memoryStream.Seek(0, SeekOrigin.Begin);

            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }

}