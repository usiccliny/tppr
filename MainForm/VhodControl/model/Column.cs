using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VhodControl.model
{
    public class Column
    {
        public string table { get; set; }
        public string column { get; set; }
        public string data_type { get; set; }
        public string description { get; set; }

        public Column (string table, string column, string data_type, string description)
        {
            this.table = table;
            this.column = column;
            this.data_type = data_type;
            this.description = description;
        }
    }
}
