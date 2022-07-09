
namespace InvoiceRepository.Models
{
    public class InvoiceItem
    {
        // A name of an item e.g. eggs.
        public string Name { get; set; }
        // A number of bought items e.g. 10.
        public int Count { get; set; }
        // A price of an item e.g. 20.5.
        public decimal Price { get; set; }
    }
}
