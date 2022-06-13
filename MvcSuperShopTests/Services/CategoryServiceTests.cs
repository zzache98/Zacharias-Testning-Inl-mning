using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Services;

namespace MvcSuperShopTests.Services
{
    [TestClass]
    public class CategoryServiceTests
    {
        private ApplicationDbContext _context;
        private CategoryService sut;

        [TestInitialize]
        public void Initialize()
        {
            
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _context = new ApplicationDbContext(contextOptions);

            sut = new CategoryService(_context);
        }

        [TestMethod]
        public void When_getting_trending_categories_should_return_count()
        {
            var categoryList = new List<Category>
            {
                new Category()
                {
                    Name = "Sven Svensson",
                    Icon = ""
                },
                new Category()
                {
                    Name = "Sven Sveensson",
                    Icon = ""
                },
                new Category()
                {
                    Name = "Sven Sveensson",
                    Icon = ""
                },
                new Category()
                {
                    Name = "Sven Sveensson",
                    Icon = ""
                },
                new Category()
                {
                    Name = "Sven Sveensson",
                    Icon = ""
                }

            };
            _context.Categories.AddRange(categoryList);
            _context.SaveChanges();

            
            
            var cnt = sut.GetTrendingCategories(5);

            Assert.AreEqual(5, cnt.Count());


        }
    }
}
