using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace FactChecker.WordcountDB
{
    public class WordCount
    {
        public List<WordCountItem> FetchDB(string word)
        {
            List<WordCountItem> list = new List<WordCountItem>();
            string connection_string = "Data Source=wordcount.db";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT * FROM WORDCOUNT WHERE WORD = \"{word}\"";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetInt32(2)} {rdr.GetInt32(3)}");
                list.Add(new WordCountItem(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
            }
            connection.Close();
            return list;
        }

        public int FetchSumOfOccurences(string word)
        {
            string connection_string = "Data Source=wordcount.db";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT SUM(OCCURRENCE) FROM WORDCOUNT WHERE WORD = \"{word}\"";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            int sum = 0;
            if(reader.Read() && reader.GetFieldType(0) == typeof(System.Int64))
            {
              sum = reader.GetInt32(0);
            }
            connection.Close();
            return sum; 
        }
    }
}

