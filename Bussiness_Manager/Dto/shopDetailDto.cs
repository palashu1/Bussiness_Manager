namespace Bussiness_Manager.Dto
{
    public class shopDetailDto
    {
        public int memberId {  get; set; }
        public int shopId { get; set; }
        public string shopName { get; set; }
        public string shopDescription { get; set; }
        public string bussinessType { get; set; }
        public string? logo { get; set; }
        public string shopAddress { get; set; }
        public string? dstatus { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
