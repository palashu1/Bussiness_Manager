using Bussiness_Manager.Dto;
using Bussiness_Manager.Models;
using Bussiness_Manager.Utility;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

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
                        customer.customerId = dto.customerId;
                        customer.memberId = customer.memberId;
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
                        memberId =(int) dto.memberId,
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
        public async Task<GenericContainer<List<customerDto>>> getAllCustomers(int memberId, int shopId)
        {
            GenericContainer<List<customerDto>> result = new GenericContainer<List<customerDto>>();
            try
            {
                var customers = await _context.customers.Where(w => w.memberId == memberId && w.shopId == shopId && w.dstatus == "V").Select(s => new customerDto()
                {
                    customerId = s.customerId,
                    memberId = s.memberId,
                    shopId = s.shopId,
                    name = s.name,
                    address = s.address,
                    mobileNo = s.mobileNo,
                    dstatus = s.dstatus,
                    createdOn = s.createdOn,
                    updatedOn = s.updatedOn,

                }).OrderByDescending(o => o.updatedOn).ToListAsync();
                if (customers != null && customers.Count != 0)
                {
                    result.IsSuccessful = true;
                    result.status = "customeList";
                    result.Value = customers;
                }
                else
                {
                    result.IsSuccessful = true;
                    result.status = "Not found";
                    result.Value = customers;
                }
            }catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.status="Internal Server Error";
            }
            return result;
        }
        public async Task<GenericContainer<string>> createUpdateProduct(productDto dto)
        {
            GenericContainer<string> result = new GenericContainer<string>();
            int save = 0;
            try
            {
                if (dto.productId > 0)
                {
                    var products = await _context.products.Where(w => w.memberId == dto.memberId && w.shopId == dto.shopId && w.productId == dto.productId && w.dstatus == "V").FirstOrDefaultAsync();
                    if (products != null) {
                        products.memberId = dto.memberId;
                        products.shopId = dto.shopId;
                        products.productId = dto.productId;
                        products.productName= dto.productName;
                        products.productSummary= dto.productSummary;
                        products.qty = (decimal)dto.qty;
                        products.hsnCode = dto.hsnCode;
                        products.salePrice =(decimal) dto.salePrice;
                        products.color= dto.color;
                        products.brand= dto.brand;
                        products.size= dto.size;
                        products.productCode= dto.productCode;
                        products.unit= dto.unit;
                        products.dstatus = "V";
                        products.createdOn = products.createdOn;
                        products.updatedOn = indiaTimeZone.DateTimeIndia();
                        _context.products.Update(products);
                        save=await _context.SaveChangesAsync();
                        if (save > 0)
                        {
                            result.IsSuccessful = true;
                            result.status = "successfully update";
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.status = "update failed";
                        }
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.status = "not found";
                    }
                }
                else
                {
                    Product product = new Product()
                    {
                        productId=dto.productId,
                        memberId=dto.memberId,
                        shopId=dto.shopId,
                        productName=dto.productName,
                        productSummary=dto.productSummary,
                        qty=(decimal)dto.qty,
                        hsnCode=dto.hsnCode,
                        salePrice=(decimal)dto.salePrice,
                        color=dto.color,
                        brand=dto.brand,
                        size=dto.size,
                        productCode=dto.productCode,
                        unit=dto.unit,
                        dstatus="V",
                        createdOn=indiaTimeZone.DateTimeIndia(),
                        updatedOn=indiaTimeZone.DateTimeIndia(),
                    };
                    await _context.products.AddAsync(product);
                    save = await _context.SaveChangesAsync();
                    if (save > 0) {
                        result.IsSuccessful = true;
                        result.status = "successfully added";
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.status = "failed";
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful= false;
                result.status = "Server error";
            }
            return result;
        }
        public async Task<GenericContainer<List<productDto>>> getAllProducts(int memberId, int shopId)
        {
            GenericContainer<List<productDto>> result = new GenericContainer<List<productDto>>();
            try
            {
                var products = await _context.products.Where(w => w.memberId == memberId && w.shopId == shopId && w.dstatus == "V").Select(s => new productDto()
                {
                    productId= s.productId,
                    memberId = s.memberId,
                    shopId = s.shopId,
                    productName = s.productName,
                    productSummary=s.productSummary,
                    qty = s.qty,
                    hsnCode = s.hsnCode,
                    salePrice = s.salePrice,
                    color = s.color,
                    brand = s.brand,
                    size = s.size,
                    productCode = s.productCode,
                    unit = s.unit,
                    dstatus = s.dstatus,
                    createdOn = s.createdOn,
                    updatedOn = s.updatedOn,
                }).OrderByDescending(o => o.updatedOn).ToListAsync();

                if (products != null && products.Any()) {
                    result.IsSuccessful=true;
                    result.status = "product list";
                    result.Value = products;
                }
                else
                {
                    result.IsSuccessful = true;
                    result.status = "not found";
                    result.Value = products;
                }
            }
            catch (Exception ex) {
                result.IsSuccessful= false;
                result.status = "Server error";
            }
            return result;
        }
        public async Task<GenericContainer<string>> createUpdateSellingInvoice(serviceSaleInvoiceDto dto)
        {
            GenericContainer<string> result=new GenericContainer<string>();
            int saleId = 0;
            try
            {
                int save = 0;
                if (dto.saleId > 0)
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var saleInvoice = await _context.saleInvoices.Where(w => w.memberId == dto.memberId && w.shopId == dto.shopId && w.saleId == dto.saleId && w.dstatus == "V").FirstOrDefaultAsync();
                        saleInvoice.memberId=dto.memberId;
                        saleInvoice.shopId = dto.shopId;
                        saleInvoice.saleInvoiceNo = dto.saleInvoiceNo;
                        saleInvoice.customerId = dto.customerId;
                        saleInvoice.discount = dto.saleInvoiceDetailDtos.Sum(s => s.discount);
                        saleInvoice.totalAmount = calculateTotalAmount(dto.saleInvoiceDetailDtos);
                        saleInvoice.netAmount = (calculateTotalAmount(dto.saleInvoiceDetailDtos)) - ((dto.saleInvoiceDetailDtos.Sum(s => s.discount)) ?? 0);
                        saleInvoice.balanceAmount = (calculateTotalAmount(dto.saleInvoiceDetailDtos)) - ((dto.saleInvoiceDetailDtos.Sum(s => s.discount)) ?? 0) - (dto.paymentAmount ?? 0);
                        saleInvoice.dstatus = "V";
                        saleInvoice.updatedOn = indiaTimeZone.DateTimeIndia();
                        _context.saleInvoices.Update(saleInvoice);
                        save=await _context.SaveChangesAsync();
                        if (save > 0)
                        {
                            foreach(var saleDetail in dto.saleInvoiceDetailDtos)
                            {

                            }
                        }
                        scope.Complete();
                    }
                }
                else
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        saleInvoice saleInvoice = new saleInvoice()
                        {
                            memberId = dto.memberId,
                            shopId = dto.shopId,
                            saleInvoiceNo = autoGenerateSaleInvoiceNo(dto.memberId, dto.shopId, dto.saleInvoiceNo),
                            customerId = dto.customerId,
                            discount = dto.saleInvoiceDetailDtos.Sum(s => s.discount) ?? 0,
                            totalAmount = calculateTotalAmount(dto.saleInvoiceDetailDtos),
                            netAmount = (calculateTotalAmount(dto.saleInvoiceDetailDtos)) - ((dto.saleInvoiceDetailDtos.Sum(s => s.discount)) ?? 0),
                            balanceAmount = (calculateTotalAmount(dto.saleInvoiceDetailDtos)) - ((dto.saleInvoiceDetailDtos.Sum(s => s.discount)) ?? 0) - (dto.paymentAmount ?? 0),
                            dstatus = "V",
                            createdOn = indiaTimeZone.DateTimeIndia(),
                            updatedOn = indiaTimeZone.DateTimeIndia(),
                        };
                        _context.saleInvoices.Add(saleInvoice);
                        await _context.SaveChangesAsync();
                        saleId = saleInvoice.saleId;
                        if (dto.saleInvoiceDetailDtos != null && dto.saleInvoiceDetailDtos.Any())
                        {
                            foreach (var saleDetail in dto.saleInvoiceDetailDtos)
                            {
                                var product = await _context.products.Where(w => w.productId == saleDetail.productId && w.dstatus == "V").FirstOrDefaultAsync();
                                product.qty = (decimal)(product.qty - saleDetail.qty);
                                _context.products.Update(product);
                                await _context.SaveChangesAsync();
                                saleInvoiceDetail saleInvoiceDetail = new saleInvoiceDetail()
                                {
                                    saleId = saleId,
                                    productId = saleDetail.productId,
                                    qty = saleDetail.qty,
                                    discount = saleDetail.discount ?? 0,
                                    price = calculatePrice(saleDetail.productId, saleDetail.qty),
                                    netAmount = (calculatePrice(saleDetail.productId, saleDetail.qty)) - (saleDetail.discount ?? 0),
                                    dstatus = "V",
                                    createdOn = indiaTimeZone.DateTimeIndia(),
                                    updatedOn = indiaTimeZone.DateTimeIndia()
                                };
                                _context.saleInvoiceDetails.Add(saleInvoiceDetail);
                            }
                            save = await _context.SaveChangesAsync();
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.status = "failed";
                        }

                        if (save > 0)
                        {
                            Transactions transactions = new Transactions()
                            {
                                memberId = dto.memberId,
                                shopId = dto.shopId,
                                customerId = dto.customerId,
                                saleId = saleId,
                                paymentNo = autoGeneratePaymentNo(dto.memberId, dto.shopId, dto.customerId, saleId),
                                transactionAmount = dto.paymentAmount ?? 0,
                                paymentMode = dto.paymentMode,
                                totalAmount = calculateTotalAmount(dto.saleInvoiceDetailDtos),
                                transactionModule = "CreateInvoice",
                                discount = dto.saleInvoiceDetailDtos.Sum(s => s.discount),
                                netAmount = (calculateTotalAmount(dto.saleInvoiceDetailDtos)) - (dto.saleInvoiceDetailDtos.Sum(s => s.discount)),
                                balanceAmount = ((calculateTotalAmount(dto.saleInvoiceDetailDtos)) - (dto.saleInvoiceDetailDtos.Sum(s => s.discount))) - (dto.paymentAmount ?? 0),
                                dstatus = "V",
                                createdOn = indiaTimeZone.DateTimeIndia(),
                                updatedOn = indiaTimeZone.DateTimeIndia(),
                            };
                            _context.transactions.Add(transactions);
                            save += await _context.SaveChangesAsync();
                            if (save > 1)
                            {
                                result.IsSuccessful = true;
                                result.status = "Success";
                                result.Value = saleId.ToString();
                            }
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.status = "failed";
                        }
                        scope.Complete();
                    }
                }
            }
            catch(Exception ex)
            {
                result.IsSuccessful = false;
                result.status = "Server error";
            }
            return result;
        }
        private decimal calculateTotalAmount(List<saleInvoiceDetailDto> saleInvoiceDetailDtos)
        {
            decimal totalAmount = 0;
            foreach(var saleDetail in saleInvoiceDetailDtos)
            {
                var saleprice = _context.products.Where(w => w.productId == saleDetail.productId).Select(s => s.salePrice).FirstOrDefault();
                totalAmount = (decimal)(totalAmount + saleprice * saleDetail.qty);

            }
            return totalAmount;
        }
        private decimal? calculatePrice(int productId, decimal? qty)
        {
            var saleprice = _context.products.Where(w => w.productId == productId).Select(s => s.salePrice).FirstOrDefault();
            var totalprice = saleprice * qty;
            return totalprice;
        }
        private int? autoGeneratePaymentNo(int memberId, int shopId, int customerId, int saleId)
        {
            int? paymentNo = 0;
            var transaction = _context.transactions.Where(w => w.memberId == memberId && w.shopId == shopId && w.customerId == customerId && w.saleId == saleId && w.dstatus == "V").ToList();
            if(transaction!=null && transaction.Any())
            {
                int? payNo = transaction.OrderByDescending(o => o.transactionId).Select(s => s.paymentNo).FirstOrDefault();
                paymentNo = payNo + 1;
            }
            else
            {
                paymentNo = 1; 
            }
            return paymentNo;
        }
        private  string autoGenerateSaleInvoiceNo(int memberId, int shopId, string invoiceNo)
        {
            string saleInvoiceNo = string.Empty;
            bool isInvoiceExist= _context.saleInvoices.Where(w=>w.memberId==memberId && w.shopId==shopId && w.saleInvoiceNo==invoiceNo && w.dstatus=="V").Any();
            if (isInvoiceExist) {
                List<string> invoiceNumberList=_context.saleInvoices.Where(w=>w.memberId==memberId && w.shopId==shopId && w.dstatus=="V").Select(s=>s.saleInvoiceNo).ToList();
                List<int> intNumberList = invoiceNumberList.Select(int.Parse).ToList();
                List<int> missingNumbers = FindMissingNumbers(intNumberList, intNumberList[0], intNumberList[intNumberList.Count-1]);
                if (missingNumbers.Count>0)
                {
                    saleInvoiceNo = missingNumbers[0].ToString();
                }
                else
                {
                    saleInvoiceNo = (intNumberList[intNumberList.Count - 1] + 1).ToString();
                }
            }
            else
            {
                saleInvoiceNo = invoiceNo;
            }
            return saleInvoiceNo;
        }
        private List<int> FindMissingNumbers(List<int> numberList, int start, int end)
        {
            // Generate the full range of numbers from start to end
            List<int> fullRange = Enumerable.Range(start, end - start + 1).ToList();

            // Find numbers in the range that are not in the original list
            List<int> missingNumbers = fullRange.Except(numberList).ToList();

            return missingNumbers;
        }
        public async Task<GenericContainer<serviceSaleInvoiceDto>> getSalesView(int memberId, int shopId, int saleId)
        {
            GenericContainer<serviceSaleInvoiceDto> result = new GenericContainer<serviceSaleInvoiceDto>();
            try
            {
                var saleInvoiceView = await _context.saleInvoices.Include(j=>j.Members).Include(i=>i.ShopDetail).Include(c=>c.Customer).Include(k=>k.saleInvoiceDetails).Include(l=>l.Transactions).AsNoTracking().Where(w => w.memberId == memberId && w.shopId == shopId && w.saleId == saleId && w.dstatus == "V").Select(s => new serviceSaleInvoiceDto()
                {
                    saleId= saleId,
                    memberId= memberId,
                    shopId= shopId,
                    customerId=s.customerId,
                    customerName=s.Customer.name,
                    customerAddress=s.Customer.address,
                    customerMobileNo=s.Customer.mobileNo,
                    shopName=s.ShopDetail.shopName,
                    shopAddress=s.ShopDetail.shopAddress,
                    memberMobileNo=s.Members.phone,
                    saleInvoiceNo=s.saleInvoiceNo,
                    netAmount=(decimal)s.netAmount,
                    discount=s.discount,
                    totalAmount=s.totalAmount,
                    balanceAmount=s.Transactions.Where(n=> (n.transactionModule == "CreateInvoice" || n.transactionModule == "UpdateInvoice") && n.dstatus=="V").Select(m=>m.balanceAmount).FirstOrDefault(),
                    paymentMode=s.Transactions.Where(n=> (n.transactionModule == "CreateInvoice" || n.transactionModule == "UpdateInvoice") && n.dstatus=="V").Select(m=>m.paymentMode).FirstOrDefault(),
                    paymentAmount=s.Transactions.Where(n=>(n.transactionModule== "CreateInvoice" || n.transactionModule== "UpdateInvoice") && n.dstatus=="V").Select(m=>m.transactionAmount).FirstOrDefault(),
                    dstatus=s.dstatus,
                    createdOn=s.createdOn,
                    updatedOn=s.updatedOn,
                    saleInvoiceDetailDtos=s.saleInvoiceDetails.Select(x=>new saleInvoiceDetailDto()
                    {
                        sdId=x.sdId,
                        saleId=x.saleId,
                        productId=x.productId,
                        productName=x.Product.productName,
                        qty=x.qty,
                        price=x.Product.salePrice,
                        discount=x.discount,
                        hsncode=x.Product.hsnCode,
                        netAmount=x.netAmount,
                        dstatus=x.dstatus,
                        createdOn=x.createdOn,
                        updatedOn=x.updatedOn
                    }).ToList()
                }).FirstOrDefaultAsync();
                if (saleInvoiceView != null)
                {
                    result.IsSuccessful = true;
                    result.status = "saleInvoicePreview";
                    result.Value = saleInvoiceView;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.status = "Not found";
                    result.Value = saleInvoiceView;
                }
            }
            catch (Exception ex) {
                result.IsSuccessful=false;
                result.status = "Server Error";
            }
            return result;
        }

        public async Task<GenericContainer<List<saleInvoiceListDto>>> manageSales(int memberId, int shopId)
        {
            GenericContainer<List<saleInvoiceListDto>> result=new GenericContainer<List<saleInvoiceListDto>>();
            try
            {
                var saleList = await _context.saleInvoices.Include(j=>j.Customer).Include(i => i.Transactions).AsNoTracking().Where(w => w.memberId == memberId && w.shopId == shopId && w.dstatus == "V").Select(s => new saleInvoiceListDto()
                {
                    memberId=memberId,
                    shopId=shopId,
                    saleId=s.saleId,
                    customerId=s.customerId,
                    invoiceNo=s.saleInvoiceNo,
                    customerName=s.Customer.name,
                    netAmount=s.netAmount,
                    paidAmount=s.Transactions.Sum(x=>x.transactionAmount),
                    balanceAmount=s.Transactions.OrderByDescending(o=>o.transactionId).Select(x=>x.balanceAmount).FirstOrDefault(),
                    dstatus=s.dstatus,
                    createdOn=s.createdOn,
                    updatedOn=s.updatedOn
                }).OrderByDescending(o => o.updatedOn).ToListAsync();
                if(saleList!=null && saleList.Any())
                {
                    result.IsSuccessful = true;
                    result.status = "saleList";
                    result.Value = saleList;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.status = "notFound";
                    result.Value = saleList;
                }
            }catch(Exception ex)
            {
                result.IsSuccessful=false;
                result.status = "Server error";
            }
            return result;
        }
    }
}
