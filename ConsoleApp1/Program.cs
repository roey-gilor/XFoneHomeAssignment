using System;
using DAO;
using BusinessLogic;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Book book = new Book
            {
                Id=7,
                Name = "50 shades of gray",
                CategoryId = 8,
                Author = "Leon Alon",
                Price = (decimal)69.40,
                UnitsInStock = 69
            };
            BooksFacade booksFacade = new BooksFacade();
            List<Category> books = booksFacade.GetAllCategories();
        }
    }
}
