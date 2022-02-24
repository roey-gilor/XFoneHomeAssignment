using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class BooksFacade
    {
        public void AddNewBook(Book book)
        {
            try
            {
                BookDaoMssql bookDaoMssql = new BookDaoMssql();
                bookDaoMssql.AddNewBook(book);
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
        public void UpdateBookPrice(Book book)
        {
            try
            {
                BookDaoMssql bookDaoMssql = new BookDaoMssql();
                bookDaoMssql.UpdateBookPrice(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Book> GetAllBooks()
        {
            List<Book> books;
            try
            {
                BookDaoMssql bookDaoMssql = new BookDaoMssql();
                books = bookDaoMssql.GetAllBooks();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return books;
        }
        public List<Category> GetAllCategories()
        {
            List<Category> categories;
            try
            {
                BookDaoMssql bookDaoMssql = new BookDaoMssql();
                categories = bookDaoMssql.GetAllCategories();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categories;
        }
    }
}
