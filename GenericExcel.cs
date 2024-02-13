


using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class TableExcel {
    public string Name { get; set; }
    public IList<RowExcel> Rows {get;set;} = new List<RowExcel>();
    public IDictionary<string,string> Columns {get;set;} = new Dictionary<string,string>();

    public string getColumn(string name) {
        try
        {
            if (this.Columns.ContainsKey(name))
                return this.Columns[name];
            return String.Empty;
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    internal void setColumns(string column, int index)
    {
        this.Columns.Add($"Column{index + 1}", column);
    }
}

public class RowExcel {

    public RowExcel() {
        this.Name = String.Empty;
        this.Values = new List<string>();
    }
    public string Name { get; set; }
    public IList<string> Values { get; set; }

    internal void setValue(string item)
    {
        this.Values.Add(item);
    }
}
// public class RowExcel
// {
       
//     [Display(Name="Column1")] public string Column1 { get; set; }


//     [Display(Name="Column2")] public string Column2 { get; set; }
//     [Display(Name="Column3")] public string Column3 { get; set; }
//     [Display(Name="Column4")] public string Column4 { get; set; }
//     [Display(Name="Column5")] public string Column5 { get; set; }
//     [Display(Name="Column6")] public string Column6 { get; set; }
//     [Display(Name="Column7")] public string Column7 { get; set; }
//     [Display(Name="Column8")] public string Column8 { get; set; }
//     [Display(Name="Column9")] public string Column9 { get; set; }
//     [Display(Name="Column10")] public string Column10 { get; set; }
//     [Display(Name="Column11")] public string Column11 { get; set; }
//     [Display(Name="Column12")] public string Column12 { get; set; }
//     internal void setValue(string value, int column, int index)
//     {

//         switch(index) {
//             case 1: this.Column1 = value;
//             break;
//             case 2: this.Column2 = value;
//             break;
//             case 3: this.Column3 = value;
//             break;
//             case 4: this.Column4 = value;
//             break;
//             case 5: this.Column5 = value;
//             break;
//             case 6: this.Column6 = value;
//             break;
//             case 7: this.Column7 = value;
//             break;
//             case 8: this.Column8 = value;
//             break;
//             case 9: this.Column9 = value;
//             break;
//             case 10: this.Column10 = value;
//             break;
//             case 11: this.Column11 = value;
//             break;
//             case 12: this.Column12 = value;
//             break;

//         }
//     }
// }

// public class Column
// {
//     public Column(string value, int column)
//     {
//         this.Value = value;
//         this.Name = $"Column{column+1}";
//         this.Active = true;
//     }
//     public string Name { get; set; }
//     public bool Active { get; } = false;
 
//     public string Value { get; set; } 
// }