using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class UserDaoMssql
    {
        public UserDaoMssql()
        {

        }

        private List<SqlParameter> GetSqlParametersAddUser(User user)
        {
            var lstParameters = new List<SqlParameter>()
            {
                new SqlParameter("@userName", user.UserName),
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@Password", user.Password)
            };
            return lstParameters;
        }
        private List<SqlParameter> GetSqlParametersLoginUser(User user)
        {
            var lstParameters = new List<SqlParameter>()
            {
                new SqlParameter("@userName", user.UserName),
                new SqlParameter("@Password", user.Password)
            };
            return lstParameters;
        }
        public bool AddNewUser(User user)
        {
            bool retVal = false;
            string procedureName = "sp_Add_User";
            List<SqlParameter> parameters = GetSqlParametersAddUser(user);
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
                    throw new DuplicateNameException("User Name already Exists");
                throw new Exception("Failed to Add new user");
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
        public User LoginUser(User user)
        {
            try
            {
                DataTable dt = new DataTable();
                List<SqlParameter> parameters = GetSqlParametersLoginUser(user);
                string procedureName = "sp_Get_User_Details";

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
                    if (dt.Rows.Count != 0)
                    {
                        user.Id = (int)dt.Rows[0][0];
                        user.FirstName = dt.Rows[0][1].ToString();
                        user.LastName = dt.Rows[0][2].ToString();
                    }
                    else
                        return null;
                }

                return user;
            }
            catch (Exception)
            {
                throw new Exception("Task Failed");
            }

        }
        private string GetConnectionString()
        {
            return AppConfig.Instance.ConnectionString;
        }
    }

}
