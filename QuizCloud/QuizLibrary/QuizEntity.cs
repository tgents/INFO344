using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizLibrary
{
    public class QuizEntity : TableEntity
    {
        public QuizEntity(string username, string password)
        {
            this.PartitionKey = username;
            this.RowKey = password;

            this.Username = username;
            this.Password = password;
        }

        public QuizEntity() { }

        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
