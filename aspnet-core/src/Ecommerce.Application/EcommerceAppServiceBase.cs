using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Ecommerce.Authorization.Users;
using Ecommerce.MultiTenancy;
using Ecommerce.ProductStores.Dto;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Ecommerce.Entity;
using Ecommerce.Entitys;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class EcommerceAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected EcommerceAppServiceBase()
        {
            LocalizationSourceName = EcommerceConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        //lấy Queryable thông tin sản phẩm, cửa hàng
        private IQueryable<ProductStoreDto> GetQueryableProductStore(
            IRepository<ProductStore, long> productStoreRepository,
            IRepository<Product, long> productRepository,
            IRepository<Store, long> storeRepository)
        {
            var query = from ps in productStoreRepository.GetAll()
                        join p in productRepository.GetAll() on ps.ProductCode equals p.Code into prodGroup
                        from p in prodGroup.DefaultIfEmpty()  // LEFT JOIN Product
                        join s in storeRepository.GetAll() on ps.StoreCode equals s.Code into shopGroup
                        from s in shopGroup.DefaultIfEmpty()  // LEFT JOIN Shop
                        select new ProductStoreDto
                        {
                            Id = ps.Id,
                            StoreCode = ps.StoreCode,
                            ProductCode = ps.ProductCode,
                            ProductName = p != null ? p.Name : null,
                            StoreName = s != null ? s.Name : null
                        };
            return query;
        }

        protected async Task<List<ProductStoreDto>> GetListProductStoreByListId(List<long> listProductStoreId,
            IRepository<ProductStore, long> productStoreRepository,
            IRepository<Product, long> productRepository,
            IRepository<Store, long> storeRepository)
        {
            IQueryable<ProductStoreDto> query = GetQueryableProductStore(productStoreRepository, productRepository, storeRepository);
            query = query.Where(s => listProductStoreId.Contains(s.Id));

            return await query.ToListAsync();
        }

        //lấy thông tin chi tiết, sản phẩm, thông tin cửa hàng
        protected async Task<List<ProductStoreDetailDto>> GetListProductStoreDetailByListId(List<long> listProductStoreDetailId,
            IRepository<ProductStoreDetail, long> productStoreDetailRepository,
            IRepository<ProductStore, long> productStoreRepository,
            IRepository<Product, long> productRepository,
            IRepository<Store, long> storeRepository)
        {
            IQueryable<ProductStoreDetail> queryProductStoreDetail = productStoreDetailRepository.GetAll().Where(s=> listProductStoreDetailId.Contains(s.Id));
            IQueryable<ProductStoreDto> queryProductStoreDto = GetQueryableProductStore(productStoreRepository, productRepository, storeRepository);

            IQueryable<ProductStoreDetailDto> query = from a in queryProductStoreDetail
                                                      join b in queryProductStoreDto on a.ProductStoreId equals b.Id into aa
                                                      from ps in aa.DefaultIfEmpty()
                                                      select new ProductStoreDetailDto()
                                                      {
                                                          Id = a.Id,
                                                          ProductStoreId = a.ProductStoreId,
                                                          PathImage = a.PathImage,
                                                          CapacityCode = a.CapacityCode,
                                                          ColorCode = a.ColorCode,
                                                          MachineConditionCode = a.MachineConditionCode,
                                                          PackageCode = a.PackageCode,
                                                          LeaseTermCode = a.LeaseTermCode,
                                                          Prepay = a.Prepay,
                                                          Price = a.Price,
                                                          Count = a.Count,
                                                          ProductName = ps.ProductName,
                                                          ProductCode = ps.ProductCode,
                                                          StoreName = ps.StoreName,
                                                          StoreCode = ps.StoreCode,

                                                      };

            query = query.Where(s => listProductStoreDetailId.Contains(s.Id));
            return await query.ToListAsync();
        }
    }
}
