using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSuperShopTests.Services
{
    [TestClass]
    public class PricingServiceTests
    {
        private PricingService sut;
        
        [TestInitialize]
        public void Initialize()
        {
            sut = new PricingService();
        }

        [TestMethod]
        public void When_no_agreement_exists_product_baseprice_is_used()
        {

            var productList = new List<ProductServiceModel>
            {
            new ProductServiceModel { BasePrice = 50000 }
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>()
            };

            var products = sut.CalculatePrices(productList, customerContext);

            Assert.AreEqual(50000, products.First().Price);
        }

        [TestMethod]
        public void When_agreement_matches_category_return_successful()
        {
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel()
                {
                    BasePrice = 50000, CategoryName = "Caddilac", Name = "Karl"
                }
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>()
                {
                    new Agreement()
                    {
                        AgreementRows = new List<AgreementRow>()
                        {
                            new AgreementRow()
                            {
                                CategoryMatch = "Caddilac",
                                PercentageDiscount = 6
                                
                            }
                        }
                    }
                }
            };

            var products = sut.CalculatePrices(productList, customerContext);

            Assert.AreEqual(47000, products.First().Price);
        }
    }

}
