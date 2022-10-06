using CheckoutBLL.Abstract.DTOs;
using CheckoutBLL.Abstract.Exceptions;
using CheckoutBLL.Abstract.Interfaces;
using CheckoutBLL.Implementation;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;

namespace CheckoutBLL.Tests
{
    [TestFixture]
    public class OrderTotalCalculatorTests
    {
        private ISupermarketOfferProvider _supermarketOfferProvider;

        private string currentOfferAsJson = "{" +
            " \"products\" : [ " +
                "{ \"productId\" : \"Apple\", " +
                "  \"priceRules\":[ " +
                "           {\"amount\": 1, \"price\": 30}," +
                "           {\"amount\": 2, \"price\": 45} ] " +
                "}, " +
                "{ \"productId\" : \"Banana\"," +
                "  \"priceRules\":[ " +
                "           {\"amount\": 1, \"price\": 50}, " +
                "           {\"amount\": 3, \"price\": 130} ]" +
                " }, " +
                "{ \"productId\" : \"Peach\"," +
                "  \"priceRules\":[ " +
                "           {\"amount\": 1, \"price\": 60}" +
                "] } " +
            "  ]" +
            "}";

        [SetUp]
        public void Setup()
        {
            _supermarketOfferProvider = Substitute.For<ISupermarketOfferProvider>();

            var currentOffer = JsonConvert.DeserializeObject<SupermarketOffer>(currentOfferAsJson);
            
            _supermarketOfferProvider.GetCurrent().Returns(currentOffer);
        }

        [TestCaseSource(nameof(CalculateTotal_WhenSucceded_ShouldReturnExpectedTotal_TestCaseData))]
        public void CalculateTotal_WhenSucceded_ShouldReturnExpectedTotal(Order order, decimal expectedTotal)
        {
            //Arrange
            var sut = Substitute.For<OrderTotalCalculator>(_supermarketOfferProvider);

            //Act
            var total = sut.CalculateTotal(order);

            //Assert
            total.Should().Be(expectedTotal);
        }

        static IEnumerable<TestCaseData> CalculateTotal_WhenSucceded_ShouldReturnExpectedTotal_TestCaseData()
        {
            return new TestCaseData[] {

#region Apples

                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Apple", Amount = 1m }
                          }
                     },
                     30.0m
                ),
                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Apple", Amount = 2m }
                          }
                     },
                     45.0m
                ),
                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Apple", Amount = 3.0m }
                          }
                     },
                     75.0m
                ),
                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Apple", Amount = 4.0m }
                          }
                     },
                     90.0m
                ),
                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Apple", Amount = 4.5m }
                          }
                     },
                     105.0m
                ),
#endregion

#region Banana

                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Banana", Amount = 1m }
                          }
                     },
                     50.0m
                ),
                 new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Banana", Amount = 2m }
                          }
                     },
                     100.0m
                ),
                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Banana", Amount = 3m }
                          }
                     },
                     130.0m
                ),
                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Banana", Amount = 4m }
                          }
                     },
                     180.0m
                ),
                 new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Banana", Amount = 4.5m }
                          }
                     },
                     205.0m
                ),
#endregion


#region Peaches

                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Peach", Amount = 1m }
                          }
                     },
                     60.0m
                ),
                new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Peach", Amount = 2m }
                          }
                     },
                     120.0m
                ),
                 new TestCaseData(
                     new Order{
                          OrderItems = new []{
                              new OrderItem{ ProductId = "Peach", Amount = 2.5m }
                          }
                     },
                     150.0m
                ),
#endregion



            };
        }

        [TestCaseSource(nameof(CalculateTotal_WhenOrderIsInvalid_ShouldThrowException_TestCaseData))]
        public void CalculateTotal_WhenOrderIsInvalid_ShouldThrowException(Order order)
        {
            //Arrange
            var sut = Substitute.For<OrderTotalCalculator>(_supermarketOfferProvider);

            //Act & Assert
            Assert.Throws<InvalidOrderException>(() => sut.CalculateTotal(order));
        }

        static IEnumerable<TestCaseData> CalculateTotal_WhenOrderIsInvalid_ShouldThrowException_TestCaseData()
        {
            return new TestCaseData[] {
                new TestCaseData(
                         new Order
                         {
                             OrderItems = null
                         }
                ),
                new TestCaseData(
                         new Order
                         {
                             OrderItems = new[] {
                                 new OrderItem { ProductId = "Apple", Amount = 0.0m }
                             }
                         }
                ),
            };
        }


        [TestCaseSource(nameof(CalculateTotal_WhenProductNotFound_ShouldThrowProductOutOfStockException_TestCaseData))]
        public void CalculateTotal_WhenProductNotFound_ShouldThrowProductOutOfStockException(Order order)
        {
            //Arrange
            var sut = Substitute.For<OrderTotalCalculator>(_supermarketOfferProvider);

            //Act & Assert
            Assert.Throws<ProductOutOfStockException>(() => sut.CalculateTotal(order));
        }

        static IEnumerable<TestCaseData> CalculateTotal_WhenProductNotFound_ShouldThrowProductOutOfStockException_TestCaseData()
        {
            return new TestCaseData[] {
                new TestCaseData(
                         new Order
                         {
                             OrderItems = new[] {
                                 new OrderItem { ProductId = "Orange", Amount = 1.0m }
                             }
                         }
                ),
            };
        }


        

        [Test]
        public void CalculateTotal_WhenOrderIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            var sut = Substitute.For<OrderTotalCalculator>(_supermarketOfferProvider);

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.CalculateTotal(default(Order)));
        }
    }
}