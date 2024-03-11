using BlazorAppExcel.Components;

namespace BlazorAppExcel.Models
{
    public class User
    {
        public TableExcel TableActive { get; set; }

        public string Name { get; set; }

        public User(IDictionary<string, TableExcel> tables)
        {
            this.Tables = tables;
        }
        public User() { 
            this.Tables = new Dictionary<string, TableExcel>();
        }
        public IDictionary<string, TableExcel> Tables { get; }

        public void AddTable(TableExcel table)
        {
            if (this.Tables.ContainsKey(table.Name))
            {
                this.ChangeNameCounter(table, 0);
            }
           this.Tables.Add(table.Name, table);
        }

        public void SetTables(IList<TableExcel> tables)
        {
            foreach (var table in tables) {
                this.Tables.Add(table.Name, table);
            }
        }

        private void ChangeNameCounter(TableExcel table, int count)
        {
            var newName = $"{table.Name}_{count}";

            if (this.Tables.ContainsKey(newName))
            {
                Console.WriteLine(newName);
                this.ChangeNameCounter(table, count+1);
            }
            else
                table.Name = newName;

        }

        internal void AddTables(IList<TableExcel> tables)
        {
            foreach (var item in tables)
            {
                this.AddTable(item);
            }
        }

        public IList<TableExcel> TablesToList()
        {
            return this.Tables.Select(_ => _.Value).ToList();
        }
    }
}
