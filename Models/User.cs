using BlazorAppExcel.Components;

namespace BlazorAppExcel.Models
{
    public class User
    {
        public TableExcel TableActive { get; set; }

        public User(IDictionary<string, TableExcel> tables)
        {
            this.Tables = tables;
        }
        public User() { 
            this.Tables = new Dictionary<string, TableExcel>();
        }
        public IDictionary<string, TableExcel> Tables { get; set; }

        public void AddTable(TableExcel table)
        {
            var _count = this.Tables.Count(_ => _.Key == table.CodeName);
            if (_count>=0)
                this.Tables.Add(table.CodeName, table);
        }
    }
}
