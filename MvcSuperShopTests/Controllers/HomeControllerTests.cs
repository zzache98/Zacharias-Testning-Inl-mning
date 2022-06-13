﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcSuperShop.Controllers;
using MvcSuperShop.Data;
using MvcSuperShop.Services;
using MvcSuperShop.ViewModels;

namespace MvcSuperShopTests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController sut;
        private Mock<ICategoryService> categoryServiceMock;
        private Mock<IProductService> productServiceMock;
        private Mock<IMapper> mapperMock;
        private ApplicationDbContext context;

        [TestInitialize]
        public void Initialize()
        {
            categoryServiceMock = new Mock<ICategoryService>();
            productServiceMock = new Mock<IProductService>();
            mapperMock = new Mock<IMapper>();

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(contextOptions);
            context.Database.EnsureCreated();

            sut = new HomeController(categoryServiceMock.Object,
                productServiceMock.Object, mapperMock.Object, context);
        }

        [TestMethod]
        public void Index_should_show_3_categories()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "gunnar@somecompany.com")
            }, "TestAuthentication"));

            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = user
            };

            categoryServiceMock.Setup(e => e.GetTrendingCategories(3)).Returns(new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            });

            mapperMock.Setup(m => m.Map<List<CategoryViewModel>>(It.IsAny<List<Category>>()))
                .Returns(new List<CategoryViewModel>
                {
                    new CategoryViewModel(),
                    new CategoryViewModel(),
                    new CategoryViewModel()
                });

            var result = sut.Index() as ViewResult;

            var model = result.Model as HomeIndexViewModel;

            Assert.AreEqual(3, model.TrendingCategories.Count);
        }
    }
}
