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
                    if(customer != null)
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
                var data = await _context.customers.AsNoTracking().Where(w => w.memberId == memberId && w.shopId == shopId && w.dstatus == "V").Include(i => i.Transactions).ToListAsync();
                var customers = await _context.customers.AsNoTracking().Where(w => w.memberId == memberId && w.shopId == shopId && w.dstatus == "V").Include(i => i.Transactions).Select(s => new customerDto()
                {
                    customerId = s.customerId,
                    memberId = s.memberId,
                    shopId = s.shopId,
                    name = s.name,
                    address = s.address,
                    mobileNo = s.mobileNo,
                    balanceAmount= (decimal)((s.Transactions.Where(w=>w.dstatus=="V").Select(s=>s.netAmount).FirstOrDefault()!=null? s.Transactions.Where(w=>w.dstatus=="V").Sum(s=>s.netAmount):0) -(s.Transactions.Where(w=>w.dstatus=="V").Sum(t=>t.transactionAmount))),
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
        public async Task<GenericContainer<string>> deleteCustomers(int memberId, int shopId, int customerId)
        {
            GenericContainer<string> result = new GenericContainer<string>();
            try
            {
                int save = 0;
                var customer = await _context.customers.Where(w => w.memberId == memberId && w.shopId == shopId && w.customerId == customerId && w.dstatus == "V").FirstOrDefaultAsync();
                if (customer != null) 
                {
                    customer.dstatus = "D";
                    _context.customers.Update(customer);
                    save = await _context.SaveChangesAsync();
                    if (save > 0)
                    {
                        var sales = await _context.saleInvoices.Include(i=>i.saleInvoiceDetails).Where(w => w.memberId == memberId && w.shopId == shopId && w.customerId == customerId && w.dstatus == "V").ToListAsync();
                        if (sales != null && sales.Any())
                        {
                            if (sales.Count > 0)
                            {
                                foreach(var sale in sales)
                                {
                                    sale.dstatus = "D";
                                    _context.saleInvoices.Update(sale);
                                    foreach(var saleInvoiceDetail in sale.saleInvoiceDetails)
                                    {
                                        saleInvoiceDetail.dstatus = "D";
                                        _context.saleInvoiceDetails.Update(saleInvoiceDetail);
                                    }
                                }
                                save+=await _context.SaveChangesAsync();
                                if (save > 1)
                                {
                                    var transactions = await _context.transactions.Where(w => w.memberId == memberId && w.shopId == shopId && w.customerId == customerId && w.dstatus == "V").ToListAsync();
                                    if(transactions != null && transactions.Any())
                                    {
                                        if(transactions.Count > 0)
                                        {
                                            foreach(var transaction in transactions)
                                            {
                                                transaction.dstatus="D";
                                                _context.transactions.Update(transaction);
                                            }
                                            save+=await _context.SaveChangesAsync();
                                            if (save > 2)
                                            {
                                                result.status = "Done";
                                                result.Value = customerId.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
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
                        saleInvoice.saleInvoiceNo = saleInvoice.saleInvoiceNo == dto.saleInvoiceNo ? dto.saleInvoiceNo : autoGenerateSaleInvoiceNo(dto.memberId, dto.shopId, dto.saleInvoiceNo);
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
                            var previousSaleInvoiceDetail = await _context.saleInvoiceDetails.Where(w => w.saleId == dto.saleId && w.dstatus == "V").ToListAsync();
                            foreach (var saleDetail in previousSaleInvoiceDetail)
                            {
                               var product=await _context.products.Where(w=>w.productId==saleDetail.productId).FirstOrDefaultAsync();
                                product.qty = (decimal)(product.qty + saleDetail.qty);
                                _context.products.Update(product);
                            }
                            await _context.SaveChangesAsync();
                            _context.saleInvoiceDetails.RemoveRange(previousSaleInvoiceDetail);
                            await _context.SaveChangesAsync();

                            if (dto.saleInvoiceDetailDtos != null && dto.saleInvoiceDetailDtos.Any())
                            {
                                foreach (var saleDetail in dto.saleInvoiceDetailDtos)
                                {
                                    var product = await _context.products.Where(w => w.productId == saleDetail.productId && w.dstatus == "V").FirstOrDefaultAsync();
                                    if (product != null)
                                    {
                                        product.qty = (decimal)(product.qty - saleDetail.qty);
                                        _context.products.Update(product);
                                        await _context.SaveChangesAsync();
                                        saleInvoiceDetail saleInvoiceDetail = new saleInvoiceDetail()
                                        {
                                            saleId=dto.saleId,
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
                                }
                                await _context.SaveChangesAsync();
                                var transactions = await _context.transactions.Where(w => w.memberId == dto.memberId && w.shopId == dto.shopId && w.saleId == dto.saleId).ToListAsync();
                                var transaction = transactions.Where(w => (w.transactionModule == "CreateInvoice" || w.transactionModule == "UpdateInvoice")).FirstOrDefault();
                                transaction.customerId = dto.customerId;
                                transaction.saleId = dto.saleId;
                                transaction.totalAmount = calculateTotalAmount(dto.saleInvoiceDetailDtos);
                                transaction.discount = dto.saleInvoiceDetailDtos.Sum(s => s.discount);
                                transaction.netAmount = (calculateTotalAmount(dto.saleInvoiceDetailDtos)) - (dto.saleInvoiceDetailDtos.Sum(s => s.discount));
                                transaction.balanceAmount = ((calculateTotalAmount(dto.saleInvoiceDetailDtos)) - (dto.saleInvoiceDetailDtos.Sum(s => s.discount)??0)) - ((dto.paymentAmount ?? 0)+ (_context.transactions.Where(w => w.memberId == dto.memberId && w.shopId == dto.shopId && w.customerId == dto.customerId && w.saleId == dto.saleId && w.dstatus == "V" && w.transactionModule != "CreateInvoice" && w.transactionModule != "UpdateInvoice").Sum(s => s.transactionAmount)));
                                transaction.transactionAmount = dto.paymentAmount ?? 0;
                                transaction.paymentMode = dto.paymentMode;
                                transaction.transactionModule = "UpdateInvoice";
                                transaction.updatedOn = indiaTimeZone.DateTimeIndia();
                                _context.transactions.Update(transaction);
                                await _context.SaveChangesAsync();

                                transactions.RemoveAll(r => r.transactionModule == "CreateInvoice" || r.transactionModule == "UpdateInvoice");
                                if(transactions!=null && transactions.Any())
                                {
                                    if (transactions.Count > 0)
                                    {
                                        foreach(var item in transactions)
                                        {
                                            item.balanceAmount = saleInvoice.netAmount - item.transactionAmount;
                                            _context.transactions.Update(item);
                                        }
                                        await _context.SaveChangesAsync();
                                    }
                                }
                                result.IsSuccessful = true;
                                result.status = "update successfully";
                                result.Value = dto.saleId.ToString();
                            }
                            else
                            {
                                result.IsSuccessful = false;
                                result.status = "failed";
                                return result;
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
                                    saleId= saleId,
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
                            if (save > 0)
                            {
                                Transactions transactions = new Transactions()
                                {
                                    memberId = dto.memberId,
                                    shopId = dto.shopId,
                                    customerId = dto.customerId,
                                    saleId = saleId,
                                    paymentNo = autoGeneratePaymentNo(dto.memberId, dto.shopId, dto.customerId),
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
        private int? autoGeneratePaymentNo(int memberId, int shopId, int customerId)
        {
            int? paymentNo = 0;
            var transaction = _context.transactions.Where(w => w.memberId == memberId && w.shopId == shopId && w.customerId == customerId && w.dstatus == "V").ToList();
            if(transaction!=null && transaction.Any())
            {
                if (transaction.Count > 0)
                {
                    int? payNo = transaction.OrderByDescending(o => o.transactionId).Select(s => s.paymentNo).FirstOrDefault();
                    paymentNo = payNo + 1;
                }
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
                    balanceAmount=/*s.Transactions.Where(n=> (n.transactionModule == "CreateInvoice" || n.transactionModule == "UpdateInvoice") && n.dstatus=="V").Select(m=>m.balanceAmount).FirstOrDefault()*/s.netAmount- s.Transactions.Where(w=>w.dstatus=="V").Sum(s=>s.transactionAmount),
                    paymentMode=s.Transactions.Where(n=> (n.transactionModule == "CreateInvoice" || n.transactionModule == "UpdateInvoice") && n.dstatus=="V").Select(m=>m.paymentMode).FirstOrDefault(),
                    paymentAmount=/*s.Transactions.Where(n=>(n.transactionModule== "CreateInvoice" || n.transactionModule== "UpdateInvoice") && n.dstatus=="V").Select(m=>m.transactionAmount).FirstOrDefault()*/s.Transactions.Where(w=>w.dstatus=="V").Sum(s=>s.transactionAmount),
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
                    balanceAmount=s.netAmount-s.Transactions.Where(w=>w.dstatus=="V").Sum(s=>s.transactionAmount),
                    dstatus=s.dstatus,
                    createdOn=s.createdOn,
                    updatedOn=s.updatedOn,
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
        public async Task<GenericContainer<List<paymentInHistoryDto>>> paymentInHistoryList(int memberId, int shopId)
        {
            GenericContainer<List<paymentInHistoryDto>> result = new GenericContainer<List<paymentInHistoryDto>>();
            List<paymentInHistoryDto> finalPaymentData = new List<paymentInHistoryDto>();
            try
            {
                var data = await _context.transactions.Include(i => i.saleInvoice).Include(c => c.Customer).AsNoTracking().Where(w => w.memberId == memberId && w.shopId == shopId && w.dstatus == "V").Select(s => new paymentInHistoryDto()
                {
                    memberId = memberId,
                    shopId = shopId,
                    saleId = s.saleId,
                    saleInvoiceNo = s.saleInvoice.saleInvoiceNo,
                    transactionId = s.transactionId,
                    transactionDate = s.createdOn,
                    customerId = s.customerId,
                    customerName = s.Customer.name,
                    netAmount = s.saleInvoice.netAmount,
                    paidAmount = 0,
                    transactionAmount=s.transactionAmount,
                    balanceAmount = 0,
                    paymentNo = s.paymentNo,
                    paymentMode = s.paymentMode,
                    transactionModule = s.transactionModule,
                    dstatus = s.dstatus,
                    createdOn = s.createdOn,
                    updatedOn = s.updatedOn,
                }).ToListAsync();
                data.RemoveAll(r => r.transactionAmount == 0);
                var dataGroupBy=data.GroupBy(r => r.saleId).ToList();
                foreach(var group in dataGroupBy)
                {
                    decimal? grb = 0;
                    var DataGroupBySaleId = data.Where(w => w.saleId == group.Key).ToList();
                    foreach(var item in DataGroupBySaleId)
                    {
                        grb = grb + item.transactionAmount;
                        item.paidAmount = grb;
                        item.balanceAmount = item.netAmount - grb;
                    }
                    finalPaymentData.AddRange(DataGroupBySaleId);
                }
                finalPaymentData = finalPaymentData.OrderByDescending(o => o.transactionId).ToList();

                result.IsSuccessful = true;
                result.status = "paymentInHistoryList";
                result.Value = finalPaymentData;
            }
            catch (Exception ex) 
            {
                result.IsSuccessful = false;
                result.status = "Server error";
            }
            return result;
        }
        public async Task<GenericContainer<paymentInHistoryDto>> paymentView(int memberId, int shopId, int transactionId)
        {
            GenericContainer<paymentInHistoryDto> result = new GenericContainer<paymentInHistoryDto>();
            try
            {
                var data = await _context.transactions.Include(i => i.Members).Include(i => i.Customer).AsNoTracking().Where(w => w.memberId == memberId && w.shopId == shopId && w.transactionId == transactionId && w.dstatus == "V").Select(s => new paymentInHistoryDto()
                {
                    memberId = memberId,
                    shopId = shopId,
                    saleId = s.saleId,
                    memberMobileNo = s.Members.phone,
                    shopName = s.ShopDetail.shopName,
                    transactionId = transactionId,
                    transactionDateView = (DateTime)s.createdOn,
                    customerName = s.Customer.name,
                    customerMobileNo = s.Customer.mobileNo,
                    netAmount = s.netAmount,
                    paidAmount = s.transactionAmount,
                    amountInWords = Helper.ConvertNumberToWords(Convert.ToInt32(s.transactionAmount)) + " " + "Rupees Only.",
                    paymentNo = s.paymentNo,
                    paymentMode = s.paymentMode,
                    logo=Helper.GetInitials(s.ShopDetail.shopName),
                    dstatus = s.dstatus
                }).FirstOrDefaultAsync();
                result.status = "receiptView";
                result.Value = data;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.status = "Server error";
            }
            return result;
        }
        public async Task<GenericContainer<int>> deleteSale(int memberId, int shopId, int saleId)
        {
            GenericContainer<int> result = new GenericContainer<int>();
            int save = 0;
            try
            {
                var sale = await _context.saleInvoices.Include(i => i.saleInvoiceDetails).Include(i => i.Transactions).Where(w => w.memberId == memberId && w.shopId == shopId && w.saleId == saleId && w.dstatus == "V").FirstOrDefaultAsync();
                if (sale != null) 
                {
                    sale.dstatus = "D";
                    _context.saleInvoices.Update(sale);
                    save = await _context.SaveChangesAsync();
                    if (save > 0)
                    {
                        if(sale.saleInvoiceDetails!=null && sale.saleInvoiceDetails.Any())
                        {
                            if (sale.saleInvoiceDetails.Count > 0)
                            {
                                foreach(var saleInvoiceDetail in sale.saleInvoiceDetails)
                                {
                                    saleInvoiceDetail.dstatus = "D";
                                    _context.saleInvoiceDetails.Update(saleInvoiceDetail);
                                }
                                await _context.SaveChangesAsync();
                            }
                        }

                        if(sale.Transactions!=null && sale.Transactions.Any())
                        {
                            if(sale.Transactions.Count > 0)
                            {
                                foreach(var transaction in sale.Transactions)
                                {
                                    transaction.dstatus = "D";
                                    _context.transactions.Update(transaction);
                                }
                                await _context.SaveChangesAsync();
                                result.status = "Done";
                                result.Value = saleId;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return result;
        }
        public async Task<GenericContainer<PaymentInDto>> paymentIn(PaymentInDto dto)
        {
            GenericContainer<PaymentInDto> result = new GenericContainer<PaymentInDto>();
            int save = 0;
            try
            {
                dto.paymentInDetailDtos.RemoveAll(r => r.paymentAmount == null || r.paymentAmount == 0);
                int? autoPaymentNo = autoGeneratePaymentNo(dto.memberId, dto.shopId, dto.customerId);
                foreach (var payment in dto.paymentInDetailDtos)
                {
                    Transactions transactions = new Transactions()
                    {
                        memberId=dto.memberId,
                        shopId=dto.shopId,
                        customerId = dto.customerId,
                        saleId= payment.saleId,
                        totalAmount=0,
                        netAmount=0,
                        balanceAmount= (_context.transactions.Where(w=>w.memberId==dto.memberId && w.shopId==dto.shopId && w.customerId==dto.customerId && w.dstatus=="V").OrderByDescending(o=>o.transactionId).Select(s=>s.balanceAmount).FirstOrDefault())-payment.paymentAmount,
                        dstatus="V",
                        createdOn=indiaTimeZone.DateTimeIndia(),
                        updatedOn=indiaTimeZone.DateTimeIndia(),
                        paymentNo= autoPaymentNo,
                        transactionAmount=payment.paymentAmount,
                        paymentMode=dto.paymentMode,
                        transactionModule="paymentIn"
                    };
                    _context.transactions.Add(transactions);
                }
                save = await _context.SaveChangesAsync();
                if (save > 0) {
                    result.IsSuccessful = true;
                }
                else
                {
                    result.IsSuccessful= false;
                }
            }catch(Exception ex)
            {
                result.IsSuccessful= false;
                result.status = "Server error";
            }
            return result;
        }
        public async Task<GenericContainer<int>> addShops(shopDetailDto dto)
        {
            GenericContainer<int> result = new GenericContainer<int>();
            int save = 0;
            try
            {
                if (dto.shopId > 0)
                {
                    var shop = await _context.shopDetails.Where(w => w.memberId == dto.memberId && w.shopId == dto.shopId && w.dstatus == "V").FirstOrDefaultAsync();
                    if (shop != null)
                    {
                        shop.memberId = dto.memberId;
                        shop.shopName = dto.shopName;
                        shop.shopDescription= dto.shopDescription;
                        shop.bussinessType= dto.bussinessType;
                        shop.shopAddress= dto.shopAddress;
                        shop.logo = "Logo";
                        shop.dstatus = "V";
                        shop.updatedOn = indiaTimeZone.DateTimeIndia();
                        _context.shopDetails.Update(shop);
                        save = await _context.SaveChangesAsync();
                        if (save > 0)
                        {
                            result.IsSuccessful = true;
                            result.status = "Update Successfully";
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.status = "Update Failed";
                        }
                    }
                }
                else
                {
                    ShopDetail shopDetail = new ShopDetail()
                    {
                        memberId = dto.memberId,
                        shopName = dto.shopName,
                        shopDescription = dto.shopDescription,
                        bussinessType = dto.bussinessType,
                        shopAddress = dto.shopAddress,
                        logo = "Logo",
                        dstatus = "V",
                        createdOn = indiaTimeZone.DateTimeIndia(),
                        updatedOn = indiaTimeZone.DateTimeIndia(),
                    };
                    _context.shopDetails.Add(shopDetail);
                    save = await _context.SaveChangesAsync();
                    if (save > 0)
                    {
                        result.IsSuccessful = true;
                        result.status = "Done";
                        result.Value = shopDetail.shopId;
                    }
                    else
                    {
                        result.IsSuccessful = false;
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
        public async Task<GenericContainer<List<shopDetailDto>>> manageShops(int memberId)
        {
            GenericContainer<List<shopDetailDto>> result = new GenericContainer<List<shopDetailDto>>();
            try
            {
                var shopDetails = await _context.shopDetails.Where(w => w.memberId == memberId &&  w.dstatus == "V").Select(s => new shopDetailDto()
                {
                    memberId = s.memberId,
                    shopId = s.shopId,
                    shopName = s.shopName,
                    shopDescription = s.shopDescription,
                    bussinessType = s.bussinessType,
                    logo = s.logo,
                    shopAddress = s.shopAddress,
                    dstatus = s.dstatus,
                    createdOn = s.createdOn,
                    updatedOn = s.updatedOn,
                }).OrderByDescending(o => o.updatedOn).ToListAsync();
                result.IsSuccessful = true;
                result.Value = shopDetails;
            }
            catch(Exception ex)
            {
                result.IsSuccessful = false;
                result.status = "Server error";
            }
            return result;
        }
    }
}
