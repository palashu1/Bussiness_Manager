using Bussiness_Manager.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bussiness_Manager.Dto
{
    public class customerDto
    {
        public int customerId { get; set; }
        public int? memberId { get; set; }
        public int? shopId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string mobileNo { get; set; }
        public string dstatus { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
