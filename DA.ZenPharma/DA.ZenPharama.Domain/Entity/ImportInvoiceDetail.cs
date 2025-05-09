using DA.ZenPharma.Domain.Entity;

public class ImportInvoiceDetail : BaseEntity
{
    public Guid ImportInvoiceId { get; set; }
    public Guid ProductId { get; set; }  
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }

    public ImportInvoice ImportInvoice { get; set; }
    public Product Product { get; set; }
    public ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();

}
