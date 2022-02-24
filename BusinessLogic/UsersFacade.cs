using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;

namespace BusinessLogic
{
    public class UsersFacade
    {
        public void AddNewUser(User user)
        {
            try
            {
                UserDaoMssql userDaoMssql = new UserDaoMssql();
                userDaoMssql.AddNewUser(user);
            }
            catch (DuplicateNameException ex)
            {
                throw new DuplicateNameException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public User TryLogin(User user)
        {
            try
            {
                UserDaoMssql userDaoMssql = new UserDaoMssql();
                user = userDaoMssql.LoginUser(user);
                if (user == null)
                    throw new UserNotExistsException("One or more of the details are wrong");
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
