namespace BlazorAppExcel.Models
{
    public class User
    {
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
