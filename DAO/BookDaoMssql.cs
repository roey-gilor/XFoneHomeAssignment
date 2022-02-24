using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class BookDaoMssql
    {
        public BookDaoMssql()
        {

        }
        private bool ExecuteNonQuery(string procedureName, List<SqlParameter> parameters)
        {
            bool retVal = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    SqlCommand command = GetCommandWithParameters(connection, procedureName, parameters);
                    command.CommandTimeout = 0;

                    connection.Open();
                    IAsyncResult result = command.BeginExecuteNonQuery();
                    var affectedRows = command.EndExecuteNonQuery(result);
                    retVal = (affectedRows > 0);
                    connection.Close();
                }

                return retVal;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key row in object"))
                    throw new DuplicateBookNameException("book Name already Exists");
                throw new Exception("Failed to Add new book");
            }
        }

        private SqlCommand GetCommandWithParameters(SqlConnection connection, string commandName, List<SqlParameter> parameters = null)
        {
            SqlCommand command = new SqlCommand();
            command.CommandTimeout = 0;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandName;
            command.Connection = connection;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                    command.Parameters.Add(parameter);
            }
            return command;
        }

        private DataTable ExecuteProcedure(string procedureName, List<SqlParameter> parameters = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    if (parameters != null) command.Parameters.AddRange(parameters.ToArray());
                    IAsyncResult result = command.BeginExecuteReader();
                    IDataReader reader = command.EndExecuteReader(result);
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Task Failed");
            }
            return dt;
        }

        private string GetConnectionString()
        {
            return AppConfig.Instance.ConnectionString;
        }

        public bool AddNewBook (Book book)
        {
            try
            {
                var lstParameters = new List<SqlParameter>()
            {
                new SqlParameter("@bookName", book.Name),
                new SqlParameter("@categoryId", book.CategoryId),
                new SqlParameter("@author", book.Author),
                new SqlParameter("@price", book.Price),
                new SqlParameter("@unitsInStock", book.UnitsInStock)
            };
                return ExecuteNonQuery("sp_Add_New_Book", lstParameters);
            }
            catch (DuplicateBookNameException ex)
            {
                throw new DuplicateBookNameException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool UpdateBookPrice(Book book)
        {
            try
            {
                var lstParameters = new List<SqlParameter>()
            {
                new SqlParameter("@bookId", book.Id),
                new SqlParameter("@bookName", book.Name),
                new SqlParameter("@categoryId", book.CategoryId),
                new SqlParameter("@author", book.Author),
                new SqlParameter("@price", book.Price),
                new SqlParameter("@unitsInStock", book.UnitsInStock)
            };
                return ExecuteNonQuery("sp_Update_Book_Price", lstParameters);
            }
            catch (DuplicateBookNameException ex)
            {
                throw new DuplicateBookNameException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Book> GetAllBooks()
        {
            try
            {
                List<Book> books = new List<Book>();
                DataTable dt = ExecuteProcedure("sp_Get_All_Books");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    books.Add(new Book
                    {
                        Id = (long)dt.Rows[i][0],
                        Name = dt.Rows[i][1].ToString(),
                        Category = dt.Rows[i][2].ToString(),
                        Author = dt.Rows[i][3].ToString(),
                        Price = (decimal)dt.Rows[i][4],
                        UnitsInStock = (int)dt.Rows[i][5]
                    });
                }
                return books;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Category> GetAllCategories()
        {
            try
            {
                List<Category> categories = new List<Category>();
                DataTable dt = ExecuteProcedure("sp_Get_All_Categories");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    categories.Add(new Category
                    {
                        Id = (int)dt.Rows[i][0],
                        Name = dt.Rows[i][1].ToString()
                    });
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
