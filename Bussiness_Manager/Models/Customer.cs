using System.ComponentModel.DataAnnotations;

namespace Bussiness_Manager.Models
{
    public class Customer
    {
        [Key]
        public int customerId { get; set; }
        public int memberId { get; set; }
        public Members Members { get; set; }
        public int shopId { get; set; }
        public ShopDetail ShopDetail { get; set; }
        public string name {  get; set; }
        public string address {  get; set; }
        public string mobileNo { get; set; }
        public string dstatus {  get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }

        public ICollection<saleInvoice> saleInvoices { get; set; }
    }
}
