using InvoiceRepository.Repositories;
using InvoiceRepository.Models;

namespace InvoiceRepositoryTests.Repositories
{
    public class InvoiceRepositoryTests
    {
        [Fact]
        public void GetTotal_WhenIdExists_ReturnTotalOfThatInvoice()
        {
            var invoices = GetInvocesTestData();

            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            Assert.Equal(1300, repo.GetTotal(1));
        }

        [Fact]
        public void GetTotal_WhenIdDoesNotExist_ReturnNull()
        {
            var invoices = GetInvocesTestData();

            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            Assert.Null(repo.GetTotal(0));
        }

        [Fact]
        public void GetTotalOfUnpaid_WhenCalled_ReturnTotalUnpaid()
        {
            var invoices = GetInvocesTestData();

            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            Assert.Equal(244, repo.GetTotalOfUnpaid());
        }

        [Fact]
        public void GetItemsReport_WhenDatesAreNull_ReturnDictionaryWithAllInfo()
        {
            var invoices = GetInvocesTestData();

            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            var expected = new Dictionary<string, long>()
            {
                { "a", 37 },
                { "b", 44 },
            };

            Assert.Equal(expected, repo.GetItemsReport(null, null));
        }

        [Fact]
        public void GetItemsReport_WhenFromDateIsNull_ReturnDictionaryWithInfoUntilToDate()
        {
            var invoices = GetInvocesTestData();

            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            var expected = new Dictionary<string, long>()
            {
                { "a", 30 },
                { "b", 35 },
            };

            Assert.Equal(expected, repo.GetItemsReport(null, new DateTime(2022, 05, 01)));
        }

        [Fact]
        public void GetItemsReport_WhenToDateIsNull_ReturnDictionaryWithInfoSinceToDate()
        {
            var invoices = GetInvocesTestData();

            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            var expected = new Dictionary<string, long>()
            {
                { "a", 17 },
                { "b", 14 },
            };

            Assert.Equal(expected, repo.GetItemsReport(new DateTime(2022, 04, 15), null));
        }

        [Fact]
        public void GetItemsReport_WhenDatesAreNotNull_ReturnDictionaryWithInfoBetweenTheRange()
        {
            var invoices = GetInvocesTestData();


            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            var expected = new Dictionary<string, long>()
            {
                { "a", 17 },
                { "b", 14 },
            };

            Assert.Equal(expected, repo.GetItemsReport(new DateTime(2022, 04, 15), new DateTime(2022, 06, 15)));
        }

        [Fact]
        public void GetItemsReport_WhenDatesHaveNoMatches_ReturnEmptyDictionary()
        {
            var invoices = GetInvocesTestData();


            var repo = new InvoiceRepository.Repositories.InvoiceRepository(invoices);

            var expected = new Dictionary<string, long>()
            {
            };

            Assert.Equal(expected, repo.GetItemsReport(new DateTime(2023, 04, 15), new DateTime(2023, 06, 15)));
        }

        private IQueryable<Invoice> GetInvocesTestData()
        {
            var invo1 = new Invoice()
            {
                Id = 1,
                AcceptanceDate = new DateTime(2022, 01, 01),
                InvoiceItems = {
                    new InvoiceItem { Name = "a", Count = 20, Price = 20},
                    new InvoiceItem { Name = "b", Count = 30, Price = 30},
                }
            };

            var invo2 = new Invoice()
            {
                Id = 2,
                AcceptanceDate = new DateTime(2022, 04, 15),
                InvoiceItems = {
                    new InvoiceItem { Name = "a", Count = 10, Price = 10},
                    new InvoiceItem { Name = "b", Count = 5, Price = 5},
                }
            };

            var invo3 = new Invoice()
            {
                Id = 3,
                AcceptanceDate = new DateTime(2022, 06, 15),
                InvoiceItems = {
                    new InvoiceItem { Name = "a", Count = 7, Price = 7},
                    new InvoiceItem { Name = "b", Count = 9, Price = 9},
                }
            };

            var invo4 = new Invoice()
            {
                Id = 4,
                InvoiceItems = {
                    new InvoiceItem { Name = "a", Count = 10, Price = 10},
                    new InvoiceItem { Name = "b", Count = 12, Price = 12},
                }
            };


            List<Invoice> invoices = new();
            invoices.Add(invo1);
            invoices.Add(invo2);
            invoices.Add(invo3);
            invoices.Add(invo4);

            return invoices.AsQueryable();
        }

    }
}
