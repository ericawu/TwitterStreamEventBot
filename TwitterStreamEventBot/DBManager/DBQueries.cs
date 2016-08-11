using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D = System.Data;
using C = System.Data.SqlClient;
using System.Diagnostics;
using TwitterStreamEventBot.Resource;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs;

namespace TwitterStreamEventBot.DBManager
{
    public class DBQueries
    {
        // private C.SqlConnection connection;

        public static async void GetLastCount(string name, IDialogContext context)
        {
            var num = -1;
            while (num < 0)
            {
                C.SqlConnection connection;
                using (connection = new C.SqlConnection(Constants.DBConnection))
                {
                    using (var command = new C.SqlCommand())
                    {
                        connection.Open();
                        Debug.WriteLine("it worked! hopefully");
                        command.Connection = connection;
                        command.CommandType = D.CommandType.Text;
                        var sqlString = $"SELECT TOP 1 * FROM {name} ORDER BY TimeWindow DESC";
                        command.CommandText = sqlString;

                        C.SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            /*Debug.WriteLine("{0}\t{1}",
                                reader.GetDateTime(0),
                                reader.GetInt32(1));*/
                            num = reader.GetInt32(1);
                        }
                        Debug.WriteLine(num);
                    }
                    connection.Close();
                    Thread.Sleep(5000);
                }

            }

        }

    }
}