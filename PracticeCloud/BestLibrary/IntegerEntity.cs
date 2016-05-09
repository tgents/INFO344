using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestLibrary
{
    public class IntegerEntity : TableEntity
    {
        public IntegerEntity(int result)
        {
            this.PartitionKey = result.ToString();
            this.RowKey = Guid.NewGuid().ToString();

            this.Result = result;
        }

        public IntegerEntity() { }

        public int Result { get; private set; }
    }
}
