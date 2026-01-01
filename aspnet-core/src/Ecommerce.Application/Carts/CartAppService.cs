using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Ecommerce.Authorization;
using Ecommerce.Authorization.Users;
using Ecommerce.Bases;
using Ecommerce.Carts.Dto;
using Ecommerce.Entity;
using Ecommerce.Entitys;
using Ecommerce.Orders;
using Ecommerce.ProductStores.Dto;
using Ecommerce.Transactions.Dto;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Carts
{
    public interface ICartAppService
    {
        Task CreateOrEdit(CreateUpdateCartDto request);
        Task<PagedResultDto<CartDto>> GetPaging(BaseRequest request);
    }

    public class CartAppService : EcommerceAppServiceBase, ICartAppService
    {
        private readonly IRepository<Cart,long> _cartRepository;
        private readonly IRepository<Person,long> _personRepository;
        private readonly IRepository<ProductStoreDetail, long> _productStoreDetailRepository;
        private readonly IRepository<ProductStore, long> _productStoreRepository;
        private readonly IRepository<Product,long> _productRepository;
        private readonly IRepository<Store, long> _storeRepository;
        public CartAppService(IRepository<Cart, long> cartRepository, IRepository<Person, long> personRepository, IRepository<ProductStoreDetail, long> productStoreDetailRepository, IRepository<ProductStore, long> productStoreRepository, IRepository<Product, long> productRepository, IRepository<Store, long> storeRepository)
        {
            _cartRepository = cartRepository;
            _personRepository = personRepository;
            _productStoreDetailRepository = productStoreDetailRepository;
            _productStoreRepository = productStoreRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
        }

        public async Task CreateOrEdit(CreateUpdateCartDto request)
        {
           await Validate(request);

           User user = await GetCurrentUserAsync();

            // lấy thông tin person
           Person person =  _personRepository.GetAll().FirstOrDefault(s => s.UserId == user.Id);

            // kiểm tra đã có trong giỏ hàng chưa
            Cart cart = _cartRepository.GetAll().FirstOrDefault(s => s.PersonId == person.Id && s.ProductStoreDetailId == request.ProductStoreDetailId);

            if (cart == null)
            {
                // chưa có , thêm mới
                cart = ObjectMapper.Map<Cart>(request);
                await _cartRepository.InsertAsync(cart);
            }
            else
            {
                // cập nhật số lượng
                cart.Quantity += (request.Quantity??0);
                await _cartRepository.UpdateAsync(cart);
            }
        }

        private async Task Validate(CreateUpdateCartDto request)
        {
            if (!request.Quantity.HasValue)
            {
                throw new UserFriendlyException(L("QuantityNotValid"));
            }

            if (!request.ProductStoreDetailId.HasValue)
            {
                throw new UserFriendlyException(L("ProductStoreDetailNotFound"));
            }

            if(!await _productStoreDetailRepository.GetAll().AnyAsync(s=>s.Id == request.ProductStoreDetailId))
            {
                throw new UserFriendlyException(L("ProductStoreDetailNotFound"));
            }
        }

        public async Task<PagedResultDto<CartDto>> GetPaging(BaseRequest request)
        {
            User user = await GetCurrentUserAsync();

            // lấy thông tin person
            Person person = _personRepository.GetAll().FirstOrDefault(s => s.UserId == user.Id);

            var query = _cartRepository.GetAll().Where(s => s.PersonId == person.Id);

            var pagedAndFiltered = query
                .PageBy(request);

            var totalCount = await query.CountAsync();

            var listData = await pagedAndFiltered.ToListAsync();

            List<CartDto> listDataModel = ObjectMapper.Map<List<CartDto>>(listData);

            // lấy thông tin chi tiết đơn hàng
            List<long> listProductStoreDetailId = listDataModel.Select(s=>s.ProductStoreDetailId).ToList();

            List<ProductStoreDetailDto> listProductStoreDetail = await GetListProductStoreDetailByListId(listProductStoreDetailId,_productStoreDetailRepository,_productStoreRepository,_productRepository,_storeRepository);

            foreach (CartDto cart in listDataModel)
            {
                ProductStoreDetailDto productStoreDetail = listProductStoreDetail.FirstOrDefault(s=>s.Id == cart.ProductStoreDetailId);

                cart.ProductName = productStoreDetail.ProductName;
                cart.ProductCode = productStoreDetail.ProductCode;
                cart.PathImage = productStoreDetail.PathImage;
                cart.Price = productStoreDetail.Price;
            }

            return new PagedResultDto<CartDto>(
               totalCount,
               listDataModel
            );
        }

        
    }
}
