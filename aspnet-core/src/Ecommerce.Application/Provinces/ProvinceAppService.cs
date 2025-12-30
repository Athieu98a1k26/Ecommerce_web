using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Ecommerce.Entitys;
using Ecommerce.Provinces.Dto;

namespace Ecommerce.Provinces
{
    public interface IProvinceAppService
    {
        Task InitProvince();
    }
    public class ProvinceAppService: EcommerceAppServiceBase, IProvinceAppService
    {
        private readonly IRepository<Province,long> _provinceRepository;
        private readonly HttpClient _httpClient;
        private const string BaseUrl =
       "https://production.cas.so/address-kit/2025-07-01";
        public ProvinceAppService(IRepository<Province, long> provinceRepository, HttpClient httpClient)
        {
            _provinceRepository = provinceRepository;
            _httpClient = httpClient;
        }

        [UnitOfWork]
        public async Task InitProvince()
        {
            // 1. Lấy tỉnh / thành phố
            var provincesResponse =
                await _httpClient.GetFromJsonAsync<ApiResponse>(
                    $"{BaseUrl}/provinces");

            foreach (var province in provincesResponse?.provinces)
            {
                // 2. Insert TỈNH (CHA)
                var provinceEntity = new Province
                {
                    Code = province.Code,
                    Name = province.Name,
                    ParentId = null
                };

                long id = await _provinceRepository.InsertAndGetIdAsync(
                    provinceEntity
                );

                // 3. Lấy xã / phường
                var communesResponse =
                    await _httpClient.GetFromJsonAsync<ApiResponse>(
                        $"{BaseUrl}/provinces/{province.Code}/communes");

                // 4. Insert XÃ (CON)
                foreach (var commune in communesResponse.communes)
                {
                    var communeEntity = new Province
                    {
                        Code = commune.Code,
                        Name = commune.Name,
                        ParentId = id
                    };

                    await _provinceRepository.InsertAsync(communeEntity);
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }


        public async Task<List<ProvinceDto>> GetAllProvince()
        {
            var listProvince = _provinceRepository.GetAll().Where(s=>s.ParentId == null).ToList();

            List<ProvinceDto> result = ObjectMapper.Map<List<ProvinceDto>>(listProvince);

            return result;
        }

        public async Task<List<ProvinceDto>> GetAllWard(string code)
        {
            Province province = _provinceRepository.GetAll().FirstOrDefault(s => s.Code == code);

            if(province==null)
            {
                return null;
            }

            var listProvince = _provinceRepository.GetAll().Where(s => s.ParentId == province.Id).ToList();

            List<ProvinceDto> result = ObjectMapper.Map<List<ProvinceDto>>(listProvince);

            return result;
        }
    }
}
