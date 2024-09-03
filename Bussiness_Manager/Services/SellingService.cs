using Bussiness_Manager.Dto;
using Bussiness_Manager.Models;
using Bussiness_Manager.Utility;
using Microsoft.EntityFrameworkCore;

namespace Bussiness_Manager.Services
{
    public class SellingService: ISelling
    {
        private readonly ApplicationDbContex _context;
        public SellingService(ApplicationDbContex _dbContex)
        {
            this._context = _dbContex;
        }

        public async Task<GenericContainer<string>> createUpdateCustomer(customerDto dto)
        {
            GenericContainer<string> result = new GenericContainer<string>();
            int save = 0;
            try
            {
                if (dto.customerId > 0)
                {
                    var customer = await _context.customers.Where(w => w.customerId == dto.customerId && w.memberId == dto.memberId && w.shopId == dto.shopId && w.dstatus == "V").FirstOrDefaultAsync();
                    if(customer == null)
                    {
                        customer.shopId = (int)dto.shopId;
                        customer.memberId = customer.customerId;
                        customer.shopId=customer.shopId;
                        customer.name= dto.name;
                        customer.address = dto.address;
                        customer.mobileNo=dto.mobileNo;
                        customer.dstatus = "V";
                        customer.updatedOn = indiaTimeZone.DateTimeIndia();
                         _context.customers.Update(customer);
                        save = await _context.SaveChangesAsync();
                        if (save > 0)
                        {
                            result.IsSuccessful = true;
                            result.status = "Successfully Update";
                        }
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.status = "Not found";
                    }

                }
                else
                {
                    Customer customer = new Customer()
                    {
                        memberId = dto.customerId,
                        shopId = (int)dto.shopId,
                        name = dto.name,
                        address = dto.address,
                        mobileNo = dto.mobileNo,
                        dstatus="V",
                        createdOn = indiaTimeZone.DateTimeIndia(),
                        updatedOn = indiaTimeZone.DateTimeIndia(), 
                    };
                    await _context.customers.AddAsync(customer);
                    save = await _context.SaveChangesAsync();
                    if (save > 0)
                    {
                        result.IsSuccessful = true;
                        result.status = "Sucessfully add";
                    }
                    else
                    {
                        result.IsSuccessful= false;
                        result.status = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.status = "Server error";
            }
            return result;
        }
    }
}
