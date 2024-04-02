using ExchangeRateUpdater.Model;
using ExchangeRateUpdater.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        [Test]
        public async Task GetExchangeRates_ReturnsNoExchangeRatesWhenNoMatch()
        {
            // Arrange
            var exchangeRateServiceMock = new Mock<IExchangeRateService>();

            var exchangeRateInfos = new List<ExchangeRateInfo>
            {
                new ExchangeRateInfo { CurrencyCode = "AUD", Rate = 15.243m },
                new ExchangeRateInfo { CurrencyCode = "BRL", Rate = 4.677m },
            };

            // Serializing the list to JSON
            var json = JsonSerializer.Serialize(new ExchangeRateData { Rates = exchangeRateInfos });

            exchangeRateServiceMock.Setup(x => x.GetExchangeRatesData(It.IsAny<DateTime>())).ReturnsAsync(json);
            var exchangeRateProvider = new ExchangeRateProvider(exchangeRateServiceMock.Object);
            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR"), new Currency("CZK") };


            // Act
            var exchangeRates = await exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            Assert.IsNotNull(exchangeRates);
            Assert.AreEqual(0, exchangeRates.Count()); 

        }

        [Test]
        public async Task GetExchangeRates_ReturnsAllExchangeRatesWhenMatch()
        {
            // Arrange
            var exchangeRateServiceMock = new Mock<IExchangeRateService>();

            var exchangeRateInfos = new List<ExchangeRateInfo>
            {
                new ExchangeRateInfo { CurrencyCode = "EUR", Rate = 25.265m },
                new ExchangeRateInfo { CurrencyCode = "JPY", Rate = 15.425m },
                new ExchangeRateInfo { CurrencyCode = "THB", Rate = 64.193m },
                new ExchangeRateInfo { CurrencyCode = "TRY", Rate = 72.527m },
                new ExchangeRateInfo { CurrencyCode = "USD", Rate = 23.315m },
            };

            var json = JsonSerializer.Serialize(new ExchangeRateData { Rates = exchangeRateInfos });

            exchangeRateServiceMock.Setup(x => x.GetExchangeRatesData(It.IsAny<DateTime>())).ReturnsAsync(json);
            var exchangeRateProvider = new ExchangeRateProvider(exchangeRateServiceMock.Object);
            var currencies = new List<Currency> { new Currency("EUR"), new Currency("JPY"), new Currency("THB"), new Currency("TRY"), new Currency("USD"), new Currency("CZK") };


            // Act
            var exchangeRates = await exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            Assert.IsNotNull(exchangeRates);
            Assert.AreEqual(5, exchangeRates.Count());

        }

        [Test]
        public async Task GetExchangeRates_ReturnsEmptyExchangeRatesWhenServiceReturnsEmptyData()
        {
            // Arrange
            var exchangeRateServiceMock = new Mock<IExchangeRateService>();

            exchangeRateServiceMock.Setup(x => x.GetExchangeRatesData(It.IsAny<DateTime>())).ReturnsAsync("[]");

            var exchangeRateProvider = new ExchangeRateProvider(exchangeRateServiceMock.Object);
            var currencies = new List<Currency> { new Currency("EUR"), new Currency("JPY"), new Currency("THB") };

            // Act
            var exchangeRates = await exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            Assert.IsNotNull(exchangeRates);
            Assert.AreEqual(0, exchangeRates.Count());
        }
    }
}