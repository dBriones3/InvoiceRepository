using InvoiceRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceRepository.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IQueryable<Invoice> _invoices;

        public InvoiceRepository(IQueryable<Invoice> invoices)
        {
            if (invoices == null) throw new ArgumentNullException();

            _invoices = invoices;
        }

        /// <summary>
        /// Should return a total value of an invoice with a given id. If an invoice does not exist null should be returned.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public decimal? GetTotal(int invoiceId)
        {
            var invoices = _invoices.Where(x => x.Id == invoiceId);

            if (!invoices.Any()) return null;

            return invoices.Sum(x => x.InvoiceItems.Select(y => y.Price * y.Count).Sum());
        }

        /// <summary>
        /// Should return a total value of all unpaid invoices.
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalOfUnpaid()
        {
            var invoices = _invoices.Where(x => x.AcceptanceDate == null);

            return invoices.Sum(x => x.InvoiceItems.Select(y => y.Price * y.Count).Sum());
        }

        /// <summary>
        /// Should return a dictionary where the name of an invoice item is a key and the number of bought items is a value.
        /// The number of bought items should be summed within a given period of time (from, to). Both the from date and the end date can be null.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<string, long> GetItemsReport(DateTime? from, DateTime? to)
        {
            //var dictionary = new Dictionary<string, long>();

            var query = GetItemsReportQuery(from, to);
            var invoices = _invoices.Where(query);

            var items = invoices.SelectMany(i => i.InvoiceItems)
                .GroupBy(x => x.Name)
                .Select(i => new { Name = i.First().Name, Count = i.Sum(t => t.Count) });
            var dictionary = items.ToDictionary(x => x.Name, y => (long)y.Count);

            return dictionary;
        }

        private Expression<Func<Invoice, bool>> GetItemsReportQuery(DateTime? from, DateTime? to)
        {
            if (from is null && to is null)
            {
                return x => x.AcceptanceDate != null;
            }
            else if (from == null && to != null)
            {
                return x => x.AcceptanceDate != null && x.AcceptanceDate <= to;
            }
            else if (from != null && to == null)
            {
                return x => x.AcceptanceDate != null && x.AcceptanceDate >= from;
            }
            else
            {
                return x => x.AcceptanceDate != null && x.AcceptanceDate >= from && x.AcceptanceDate <= to;
            }
        }
    }
}
