using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Ecommerce.Authorization;
using Ecommerce.Bases;
using Ecommerce.Entity;
using Ecommerce.Products.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using Ecommerce.Entitys;
using Ecommerce.ProductStores.Dto;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.ProductStores
{
    public interface IProductStoreAppService
    {
        Task InitData();
        Task<PagedResultDto<ProductStoreDto>> GetPaging(BaseRequest baseRequest);
        Task<ProductStoreDto> Get(long id);
        Task CreateOrEdit(CreateUpdateProductStoreDto request);

        Task Delete(long id);
    }

    [AbpAuthorize(PermissionNames.Pages_ProductStores)]
    public class ProductStoreAppService : EcommerceAppServiceBase, IProductStoreAppService
    {
        private readonly IRepository<Order,long> _orderRepository;
        private readonly IRepository<ProductStoreDetail, long> _productStoreDetailRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        private readonly IRepository<Store, long> _storeRepository;
        public ProductStoreAppService(IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<ProductStore, long> productStoreRepository, IRepository<Product, long> productRepository, IRepository<Store, long> storeRepository, IRepository<Order, long> orderRepository)
        {
            _productStoreDetailRepository = productStoreDetailRepository;
            _productStoreRepository = productStoreRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _orderRepository = orderRepository;
        }


        [HttpPost]
        public async Task<PagedResultDto<ProductStoreDto>> GetPaging(BaseRequest baseRequest)
        {
            var query = from ps in _productStoreRepository.GetAll()
            join p in _productRepository.GetAll() on ps.ProductCode equals p.Code into prodGroup
            from p in prodGroup.DefaultIfEmpty()  // LEFT JOIN Product
            join s in _storeRepository.GetAll() on ps.StoreCode equals s.Code into shopGroup
            from s in shopGroup.DefaultIfEmpty()  // LEFT JOIN Shop
            select new ProductStoreDto
            {
                Id = ps.Id,
                StoreCode = ps.StoreCode,
                ProductCode = ps.ProductCode,
                ProductName = p != null ? p.Name : null,
                StoreName = s != null ? s.Name : null
            };

            var pagedAndFiltered = query
                .OrderBy(baseRequest.Sorting ?? "Id desc")
                .PageBy(baseRequest);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            return new PagedResultDto<ProductStoreDto>(
                   totalCount,
                   listData
                );
        }

        public async Task<List<ProductStoreDetailDto>> GetListProductDetailStore(List<long> productStoreId)
        {
            List<ProductStoreDetail> listData = _productStoreDetailRepository.GetAll().Where(s => productStoreId.Contains(s.ProductStoreId)).ToList();

            List<ProductStoreDetailDto> result = ObjectMapper.Map<List<ProductStoreDetailDto>>(listData);

            return result;
        }

        public async Task<ProductStoreDto> Get(long id)
        {
            var data = await _productStoreRepository.GetAsync(id);
            var dataModel = ObjectMapper.Map<ProductStoreDto>(data);

            List<ProductStoreDetailDto> listDetail = await GetListProductDetailStore(new List<long>()
            {
                dataModel.Id,
            });

            if(listDetail == null || listDetail.Count == 0)
            {
                return dataModel;
            }

            dataModel.ListProductStoreDetailDto = listDetail;

            return dataModel;
        }

        public async Task InitData()
        {
            var capacities = new List<string> { "64GB", "128GB", "256GB", "512GB" };
            var colors = new List<string> { "Black", "White", "Blue", "Red" };
            var conditions = new List<string> { "New", "Used", "Refurbished", "LikeNew" };

            for (int i = 1; i <= 1; i++)
            {
                var store = new CreateUpdateProductStoreDto
                {
                    StoreCode = $"CH2",
                    ProductCode = $"Iphone16",
                    ListProductStoreDetailDto = new List<ProductStoreDetailDto>()
                };

                // Giả lập 2 chi tiết sản phẩm mỗi cửa hàng
                for (int j = 1; j <= 4; j++)
                {
                    store.ListProductStoreDetailDto.Add(new ProductStoreDetailDto
                    {
                        ProductStoreId = i,
                        PathImage = $"assets/img/product/iphone_17pro.png",
                        CapacityCode = capacities[j - 1],     // lấy thứ tự để không trùng
                        ColorCode = colors[j - 1],
                        MachineConditionCode = conditions[j - 1],
                        PackageCode = "RentThenBuy",
                        LeaseTermCode = j + 2,
                        Prepay = 4 + (j * 10),
                        Price = 25000000 + j * 1000000,
                        DetailPrice = "1-1-12495000,2-4-4915000",
                        Count = j,
                    });
                }

                ProductStoreDetailDto productStoreDetailDto = store.ListProductStoreDetailDto
                          .OrderBy(x => x.Price)
                          .FirstOrDefault();

                store.Price = productStoreDetailDto.Price;
                store.PathImage = productStoreDetailDto.PathImage;
                store.Count = store.ListProductStoreDetailDto.Sum(x => x.Count);

                store.Stars = 4;
                store.Sold = 1000;

                await CreateOrEdit(store);
            }

        }

        [UnitOfWork]
        public async Task CreateOrEdit(CreateUpdateProductStoreDto request)
        {
            await Validate(request);

            if (request.Id == null)
            {
                var data = ObjectMapper.Map<ProductStore>(request);
                long id = await _productStoreRepository.InsertAndGetIdAsync(data);

                List<ProductStoreDetail> listDetail = ObjectMapper.Map<List<ProductStoreDetail>>(request.ListProductStoreDetailDto);

                foreach (var item in listDetail)
                {
                    item.ProductStoreId = id;
                }

                await _productStoreDetailRepository.InsertRangeAsync(listDetail);
            }
            else
            {
                ProductStore data = _productStoreRepository.Get(request.Id ?? 0);

                if (data == null)
                {
                    throw new UserFriendlyException(L("ProductStoreNotFound"));
                }

                data.StoreCode = request.StoreCode;
                data.ProductCode = request.ProductCode;
                await _productStoreRepository.UpdateAsync(data);

                // xử lý detail 
                // xóa
                List<ProductStoreDetail> listDetail = _productStoreDetailRepository.GetAll().Where(s => s.ProductStoreId == data.Id).ToList();

                foreach (var item in listDetail)
                {
                    await _productStoreDetailRepository.DeleteAsync(item);
                }

                // add lại
                listDetail = ObjectMapper.Map<List<ProductStoreDetail>>(request.ListProductStoreDetailDto);

                foreach (var item in listDetail)
                {
                    item.ProductStoreId = data.Id;
                }

                await _productStoreDetailRepository.InsertRangeAsync(listDetail);
            }
        }

        private async Task Validate(CreateUpdateProductStoreDto request)
        {
            if (string.IsNullOrEmpty(request.StoreCode))
            {
                throw new UserFriendlyException(L("CodeStoreRequired"));
            }

            if (request.StoreCode.Length > 100)
            {
                throw new UserFriendlyException(L("CodeStoreMaxLength"));
            }

            if (string.IsNullOrEmpty(request.ProductCode))
            {
                throw new UserFriendlyException(L("CodeProductRequired"));
            }

            

            if (await _productStoreRepository.GetAll().AnyAsync(s => (s.StoreCode == request.StoreCode && s.ProductCode == request.ProductCode) && s.Id != request.Id))
            {
                throw new UserFriendlyException(L("ProductStoreDuplicate"));
            }
        }

        [UnitOfWork]
        public async Task Delete(long id)
        {
            await ValidateDelete(id);
            await _productStoreRepository.DeleteAsync(id);

            List<ProductStoreDetail> listDetail = _productStoreDetailRepository.GetAll().Where(s => s.ProductStoreId == id).ToList();

            foreach (var item in listDetail)
            {
                await _productStoreDetailRepository.DeleteAsync(item);
            }
        }

        public async Task ValidateDelete(long id)
        {
            var product = _productRepository.Get(id);

            //if (await _orderRepository.GetAll().AnyAsync(s => s.ProductStoreId == id))
            //{
            //    throw new UserFriendlyException(L("ProductStoreUser"));
            //}
        }
    }
}
