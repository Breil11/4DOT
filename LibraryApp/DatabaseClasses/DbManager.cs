using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Xml.XPath;
using LibraryApp.Models;

namespace LibraryApp.DatabaseClasses
{
    public class DbManager
    {
        private readonly string connectionString;

        public DbManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Books> GetBooks()
        {
            var book = new List<Books>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var command = new SQLiteCommand("SELECT * FROM Books", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            book.Add(CreateABook(reader));
                        }
                    }
                }
            }
            return book;
        }

        private Books GetBooksById(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var command = new SQLiteCommand("SELECT * FROM Books WHERE ISBN=@iId", conn))
                {
                    command.Parameters.AddWithValue("id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateABook(reader);
                        }
                    }
                }
            }
            return null;
        }

        private Books CreateABook(SQLiteDataReader reader)
        {
            return new Books
            {
                ISBN = Convert.ToInt32(reader["id"]),
                Title = Convert.ToString(reader["Title"]),
                NameAuthor = Convert.ToString(reader["NameAuthor"])
            };
        }

        public int AddBook(Books books)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();    

                using (var command = new SQLiteCommand("INSERT INTO Books (ISBN, Title, NameAuthor)" + "VALUES (@isbn, @Title, @NameAuthor); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@isbn", books.ISBN);
                    command.Parameters.AddWithValue("@Title", books.Title);
                    command.Parameters.AddWithValue("@NameAuthor", books.NameAuthor);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new book id.");
                    }
                }
            }
        }
    }
}
