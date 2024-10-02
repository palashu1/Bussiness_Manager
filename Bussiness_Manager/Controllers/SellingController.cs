using Bussiness_Manager.Dto;
using Bussiness_Manager.Models;
using Bussiness_Manager.Services;
using Bussiness_Manager.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bussiness_Manager.Controllers
{
    [Authorize]
    public class SellingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISelling _selling;
        private readonly ApplicationDbContex _context;
        public SellingController(IConfiguration configuration, ISelling selling, ApplicationDbContex _dbContex)
        {
            this._configuration = configuration;
            this._selling = selling;
            this._context = _dbContex;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> createUpdateShop(int shopId=0)
        {
            shopDetailDto shop = new shopDetailDto();
            if (shopId > 0) 
            {
                shop = _context.shopDetails.Where(w => w.memberId == Convert.ToInt32(Helper.GetClaimsFromToken(HttpContext, _configuration)) && w.shopId == shopId && w.dstatus == "V").Select(s => new shopDetailDto()
                {
                    shopId=s.shopId,
                    memberId=s.memberId,
                    shopName=s.shopName,
                    shopDescription=s.shopDescription,
                    bussinessType=s.bussinessType,
                    logo=s.logo,
                    shopAddress=s.shopAddress,
                    dstatus=s.dstatus,
                    createdOn=s.createdOn,
                    updatedOn=s.updatedOn
                }).FirstOrDefault();
            }
            else
            {
                shop.shopId = 0;
            }
            
            return View(shop);
        }
        [HttpPost]
        public async Task<IActionResult> createUpdateShop(shopDetailDto model)
        {
            IGenericContainer<int> result = new GenericContainer<int>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            model.memberId = Convert.ToInt32(memberId);
            result = await _selling.addShops(model);
            return RedirectToAction("manageShop", "Selling");
        }
        public async Task<IActionResult> manageShop(string searchString, int? pageNumber)
        {
            IGenericContainer<List<shopDetailDto>> result=new GenericContainer<List<shopDetailDto>>();
            int pageSize = 10;
            result = await _selling.manageShops(Convert.ToInt32(Helper.GetClaimsFromToken(HttpContext,_configuration)),Convert.ToInt32(Helper.GetShopIdFromToken(HttpContext,_configuration)));
            if (!string.IsNullOrEmpty(searchString))
            {
                result.Value = result.Value.Where(s => s.shopName.Contains(searchString)
                                                       || s.shopDescription.Contains(searchString)
                                                       || s.bussinessType.ToString().Contains(searchString)
                                                       || s.shopAddress.ToString().Contains(searchString)).ToList();
            }
            var paginatedList = PaginatedList<shopDetailDto>.manageShopPagination(result, pageNumber ?? 1, pageSize);
            return View(paginatedList);
        }
        //public async Task<IActionResult> deleteShop(int shopId)
        //{
        //    IGenericContainer<int> result = new GenericContainer<int>();
        //    result = await _selling.DeleteShop(Convert.ToInt32(Helper.GetClaimsFromToken(HttpContext, _configuration)), shopId);
        //    return RedirectToAction("manageShop");
        //}
        public async Task<IActionResult> deleteItem(int? id)
        {
            var product = await _context.products.Where(w => w.productId == id && w.dstatus == "V").FirstOrDefaultAsync();
            if (product != null)
            {
                //_context.products.Remove(product);
                product.dstatus = "D";
                _context.products.Update(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("manageItem");
        }
        public async Task<IActionResult> addItem(int? id)
        {
            var getAllUnits = await _context.units.ToListAsync();
            var product = await _context.products.Where(w => w.productId == id && w.dstatus == "V").FirstOrDefaultAsync();
            ViewData["Units"] = getAllUnits;
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> addItemForm(Product product)
        {
            IGenericContainer<string> result = new GenericContainer<string>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            //if (ModelState.IsValid)
            //{
            productDto dto = new productDto()
            {
                productId = product.productId,
                memberId = Convert.ToInt32(memberId),
                shopId = Convert.ToInt32(shopId),
                productName = product.productName,
                productSummary = product.productSummary,
                qty = product.qty,
                hsnCode = product.hsnCode,
                salePrice = product.salePrice,
                color = product.color,
                brand = product.brand,
                size = product.size,
                productCode = product.productCode,
                unit = product.unit,
            };
            result = await _selling.createUpdateProduct(dto);
            if (result.IsSuccessful)
            {
                return RedirectToAction("manageItem", "Selling");
            }
            else
            {
                TempData["ShowAlert"] = true;
                ViewBag.message = result.status;
                return RedirectToAction("addItem", "Selling");
            }
            // }
            return RedirectToAction("addItem", "Selling");
        }
        public async Task<IActionResult> manageItem(string searchString, int? pageNumber)
        {
            IGenericContainer<List<productDto>> result = new GenericContainer<List<productDto>>();
            int pageSize = 10;
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            var Product = _context.products.Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId) && w.dstatus == "V").OrderByDescending(o => o.updatedOn).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                Product = Product.Where(p => p.productName.Contains(searchString)
                                        || p.salePrice.ToString().Contains(searchString)
                                        || p.hsnCode.Contains(searchString)
                                        || p.unit.Contains(searchString));
            }
            var paginatedList = PaginatedList<Product>.Create(Product, pageNumber ?? 1, pageSize);
            return View(paginatedList);
        }
        public async Task<IActionResult> addCustomer(int customerId = 0)
        {
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            Customer customer = new Customer();
            if (customerId > 0)
            {
                customer = await _context.customers.Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId) && w.customerId == customerId && w.dstatus == "V").FirstOrDefaultAsync();
            }
            else
            {
                customer.customerId = 0;
            }
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> addCustomer(Customer customer)
        {
            IGenericContainer<string> result = new GenericContainer<string>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            if (ModelState.IsValid)
            {
                customerDto dto = new customerDto()
                {
                    customerId = customer.customerId,
                    memberId = Convert.ToInt32(memberId),
                    shopId = Convert.ToInt32(shopId),
                    name = customer.name,
                    address = customer.address,
                    mobileNo = customer.mobileNo,
                };
                result = await _selling.createUpdateCustomer(dto);
                if (result.IsSuccessful)
                {
                    return RedirectToAction("manageCustomer", "Selling");
                }
                else
                {
                    return RedirectToAction("manageCustomer", "Selling");
                }
            }

            return View();
        }
        public async Task<IActionResult> deleteCustomer(int customerId)
        {
            IGenericContainer<string> result = new GenericContainer<string>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            result = await _selling.deleteCustomers(Convert.ToInt32(memberId), Convert.ToInt32(shopId), customerId);
            return RedirectToAction("manageCustomer", "Selling");
        }
        public async Task<IActionResult> manageCustomer(string searchString, int? pageNumber)
        {
            IGenericContainer<List<customerDto>> result = new GenericContainer<List<customerDto>>();
            int pageSize = 10;
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            result = await _selling.getAllCustomers(Convert.ToInt32(memberId), Convert.ToInt32(shopId));
            if (!string.IsNullOrEmpty(searchString))
            {
                result.Value = result.Value.Where(s => s.name.Contains(searchString)
                                                       || s.address.Contains(searchString)
                                                       || s.mobileNo.ToString().Contains(searchString)
                                                       || s.balanceAmount.ToString().Contains(searchString)).ToList();
            }
            var paginatedList = PaginatedList<customerDto>.manageCustomerPagination(result, pageNumber ?? 1, pageSize);
            return View(paginatedList);
        }
        public async Task<IActionResult> CreateSaleInvoice()
        {
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            var customers = await _context.customers.Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId) && w.dstatus == "V").Select(s => new customerDto()
            {
                customerId = s.customerId,
                memberId = s.memberId,
                shopId = s.shopId,
                name = s.name,
                address = s.address,
                mobileNo = s.mobileNo,
                dstatus = s.dstatus,
                createdOn = s.createdOn,
                updatedOn = s.updatedOn
            }).ToListAsync();
            var products = await _context.products.Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId) && w.dstatus == "V").Select(s => new productDto()
            {
                productId = s.productId,
                memberId = s.memberId,
                shopId = s.shopId,
                productName = s.productName,
                productSummary = s.productSummary,
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
                updatedOn = s.updatedOn
            }).ToListAsync();

            var modeOfPayments = new List<modeOfPaymentDto>
            {
                new modeOfPaymentDto{id=1, paymentMode="Bank Transfer"},
                new modeOfPaymentDto{id=2, paymentMode="Cash"},
                new modeOfPaymentDto{id=3, paymentMode="Credit Card"},
                new modeOfPaymentDto{id=4, paymentMode="Cheque"},
                new modeOfPaymentDto{id=4, paymentMode="UPI"}
            };

            var model = new SaleInvoiceViewModel
            {
                Customers = customers,
                Products = products,
                SaleInvoice = new saleInvoiceDto(),
                paymentModes = modeOfPayments
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSaleInvoice(SaleInvoiceViewModel model)
        {
            IGenericContainer<string> result = new GenericContainer<string>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            serviceSaleInvoiceDto dto = new serviceSaleInvoiceDto()
            {
                saleId = model.SaleInvoice.saleId,
                memberId = Convert.ToInt32(memberId),
                shopId = Convert.ToInt32(shopId),
                customerId = model.SaleInvoice.customerId,
                saleInvoiceNo = model.SaleInvoice.saleInvoiceNo,
                discount = model.SaleInvoice.discount,
                paymentMode = Helper.getPaymentMode(Convert.ToInt32(model.SaleInvoice.paymentMode)),
                paymentAmount = model.SaleInvoice.paymentAmount,
                saleInvoiceDetailDtos = model.SaleInvoiceList
            };

            result = await _selling.createUpdateSellingInvoice(dto);
            return RedirectToAction("salesView", new { saleId = Convert.ToInt32(result.Value) });
        }
        public async Task<IActionResult> salesView(int saleId)
        {
            IGenericContainer<serviceSaleInvoiceDto> result = new GenericContainer<serviceSaleInvoiceDto>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            result = await _selling.getSalesView(Convert.ToInt32(memberId), Convert.ToInt32(shopId), saleId);
            return View(result);
        }
        public async Task<IActionResult> manageSellingInvoice(string searchString, int? pageNumber)
        {
            IGenericContainer<List<saleInvoiceListDto>> result = new GenericContainer<List<saleInvoiceListDto>>();
            int pageSize = 10;
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            result = await _selling.manageSales(Convert.ToInt32(memberId), Convert.ToInt32(shopId));
            if (!string.IsNullOrEmpty(searchString))
            {
                result.Value = result.Value.Where(s => s.customerName.ToLower().Contains(searchString)
                                                       || s.invoiceNo.Contains(searchString)
                                                       || s.netAmount.ToString().Contains(searchString)
                                                       || s.paidAmount.ToString().Contains(searchString)
                                                       || s.balanceAmount.ToString().Contains(searchString)).ToList();
            }
            var paginatedList = PaginatedList<saleInvoiceListDto>.Create1(result, pageNumber ?? 1, pageSize);
            return View(paginatedList);
        }
        public async Task<IActionResult> EditSaleInvoice(int saleId)
        {
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            var paymentModes = new List<modeOfPaymentDto>
                {
                     new modeOfPaymentDto{id=1, paymentMode="Bank Transfer"},
                     new modeOfPaymentDto{id=2, paymentMode="Cash"},
                     new modeOfPaymentDto{id=3, paymentMode="Credit Card"},
                     new modeOfPaymentDto{id=4, paymentMode="Cheque"},
                     new modeOfPaymentDto{id=4, paymentMode="UPI"}
                };
            var model = await _context.saleInvoices.Include(i => i.saleInvoiceDetails).Include(i => i.Transactions).AsNoTracking().Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId)
            && w.saleId == saleId && w.dstatus == "V").Select(s => new SaleInvoiceViewModel()
            {
                SaleInvoice = new saleInvoiceDto()
                {
                    saleInvoiceNo = s.saleInvoiceNo,
                    customerId = s.customerId,
                    netAmount = s.netAmount,
                    saleId = s.saleId,
                },
                SaleInvoiceList = s.saleInvoiceDetails.Select(x => new saleInvoiceDetailDto()
                {
                    sdId = x.sdId,
                    saleId = x.saleId,
                    productId = x.productId,
                    netAmount = x.netAmount,
                    dstatus = x.dstatus,
                    createdOn = x.createdOn,
                    updatedOn = x.updatedOn,
                    qty = x.qty,
                    discount = x.discount,
                    price = x.price,
                }).ToList(),
                Customers = _context.customers.Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId) && w.dstatus == "V").Select(s => new customerDto()
                {
                    customerId = s.customerId,
                    memberId = s.memberId,
                    shopId = s.shopId,
                    name = s.name,
                    address = s.address,
                    mobileNo = s.mobileNo,
                    dstatus = s.dstatus,
                    createdOn = s.createdOn,
                    updatedOn = s.updatedOn
                }).ToList(),
                Products = _context.products.Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId) && w.dstatus == "V").Select(s => new productDto()
                {
                    productId = s.productId,
                    memberId = s.memberId,
                    shopId = s.shopId,
                    productName = s.productName,
                    productSummary = s.productSummary,
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
                    updatedOn = s.updatedOn
                }).ToList(),
                paymentModes = paymentModes
            }).FirstOrDefaultAsync();
            return View(model);
        }
        public async Task<IActionResult> deleteSaleInvoice(int saleId)
        {
            IGenericContainer<int> result = new GenericContainer<int>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            result = await _selling.deleteSale(Convert.ToInt32(memberId), Convert.ToInt32(shopId), saleId);
            return RedirectToAction("manageSellingInvoice", "Selling");
        }
        public async Task<IActionResult> paymentInHistory(string searchString, int? pageNumber)
        {
            IGenericContainer<List<paymentInHistoryDto>> result = new GenericContainer<List<paymentInHistoryDto>>();
            int pageSize = 10;
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            result = await _selling.paymentInHistoryList(Convert.ToInt32(memberId), Convert.ToInt32(shopId));
            if (!string.IsNullOrEmpty(searchString))
            {
                result.Value = result.Value.Where(s => s.saleInvoiceNo.Contains(searchString)
                                                       || s.transactionDate.ToString().Contains(searchString)
                                                       || s.customerName.ToString().Contains(searchString)
                                                       || s.netAmount.ToString().Contains(searchString)
                                                       || s.paidAmount.ToString().Contains(searchString)
                                                       || s.balanceAmount.ToString().Contains(searchString)
                                                       || s.paymentNo.ToString().Contains(searchString)).ToList();
            }
            var paginatedList = PaginatedList<paymentInHistoryDto>.paymentInHistoryPagination(result, pageNumber ?? 1, pageSize);
            return View(paginatedList);
        }
        public async Task<IActionResult> paymentReceipt(int transactionId)
        {
            IGenericContainer<paymentInHistoryDto> result = new GenericContainer<paymentInHistoryDto>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            result = await _selling.paymentView(Convert.ToInt32(memberId), Convert.ToInt32(shopId), transactionId);
            return View(result);
        }
        public async Task<IActionResult> paymentIn(int customerId)
        {
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            var shopId = Helper.GetShopIdFromToken(HttpContext, _configuration);
            var model1 = await _context.customers.Include(i => i.saleInvoices).Include(i => i.Transactions).AsNoTracking().Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId)
            && w.customerId == customerId && w.dstatus == "V").FirstOrDefaultAsync();
            var model = await _context.customers.Include(i => i.saleInvoices).Include(i => i.Transactions).AsNoTracking().Where(w => w.memberId == Convert.ToInt32(memberId) && w.shopId == Convert.ToInt32(shopId)
            && w.customerId == customerId && w.dstatus == "V").Select(s => new PaymentInDto()
            {
                customerId = customerId,
                customerName = s.name,
                balanceDue = (s.saleInvoices.Where(w => w.dstatus == "V").Sum(s => s.netAmount)) - (s.Transactions.Where(w => w.dstatus == "V").Sum(s => s.transactionAmount)),
                updatedOn = s.updatedOn,
                paymentInDetailDtos = s.saleInvoices.Where(w => w.dstatus == "V").Select(t => new PaymentInDetailDto()
                {
                    saleId = t.saleId,
                    saleInvoiceNo = t.saleInvoiceNo,
                    invoiceDate = t.createdOn,
                    originalAmount = t.totalAmount,
                    currentAmount = t.netAmount,
                    paidAmount = s.Transactions.Where(w => w.saleId == t.saleId && w.dstatus == "V").Sum(m => m.transactionAmount),
                    balanceAmount = (t.netAmount - s.Transactions.Where(w => w.saleId == t.saleId && w.dstatus == "V").Sum(m => m.transactionAmount)),
                    createdOn = t.createdOn,
                    updatedOn = t.updatedOn
                }).ToList()
            }).FirstOrDefaultAsync();
            model.paymentInDetailDtos.RemoveAll(r => r.balanceAmount == 0);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> paymentIn(PaymentInDto model)
        {
            IGenericContainer<PaymentInDto> result = new GenericContainer<PaymentInDto>();
            model.memberId = Convert.ToInt32(Helper.GetClaimsFromToken(HttpContext, _configuration));
            model.shopId = Convert.ToInt32(Helper.GetShopIdFromToken(HttpContext, _configuration));
            result = await _selling.paymentIn(model);
            return RedirectToAction("paymentInHistory", "Selling");
        }
    }
}
