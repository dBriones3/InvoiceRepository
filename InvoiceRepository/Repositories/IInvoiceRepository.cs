
namespace InvoiceRepository.Repositories
{
    public interface IInvoiceRepository
    {
        decimal? GetTotal(int invoiceId);

        decimal GetTotalOfUnpaid();

        IReadOnlyDictionary<string, long> GetItemsReport(DateTime? from, DateTime? to);
    }
}
