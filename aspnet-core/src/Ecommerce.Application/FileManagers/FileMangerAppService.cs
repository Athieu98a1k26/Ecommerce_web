using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Ecommerce.Entitys;
using Ecommerce.FileManagers.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.FileManagers
{
    public interface IFileMangerAppService
    {
        Task<List<long>> UploadFilesAsync(
            List<IFormFile> files,
            string subsystem
        );

        Task DeleteFilesAsync(List<long> ids);

        Task<List<FileManagerDto>> GetByListId(List<long> ids);
        Task<byte[]> GetFileAsync(long id);
    }

    
    public class FileMangerAppService: EcommerceAppServiceBase,IFileMangerAppService
    {
        private readonly IRepository<FileManager,long> _fileManagerRepository;

        public FileMangerAppService(IRepository<FileManager, long> fileManagerRepository)
        {
            _fileManagerRepository = fileManagerRepository;
        }

        [AbpAuthorize]
        public async Task<List<FileManagerDto>> GetByListId(List<long> ids)
        {
            List<FileManager> listFile = await _fileManagerRepository.GetAll().Where(s=>ids.Contains(s.Id)).ToListAsync();

            return ObjectMapper.Map<List<FileManagerDto>>(listFile);
        }

        public async Task<byte[]> GetFileAsync(long id)
        {
            // 1. Lấy thông tin file từ DB
            FileManager file = await _fileManagerRepository.GetAsync(id);
            if (file == null)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            // 2. Ghép đường dẫn file
            // Ví dụ: Path = "uploads/2025/01"
            // Name = "invoice"
            // Extension = ".pdf"
            string fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                file.Path.TrimStart('/', '\\')
            );

            // 3. Check file tồn tại
            if (!File.Exists(fullPath))
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            // 4. Đọc file thành byte[]
            return await File.ReadAllBytesAsync(fullPath);
        }

        [AbpAuthorize]
        public async Task<List<long>> UploadFilesAsync(
            List<IFormFile> files,
            string subsystem
        )
        {
            if (files == null || !files.Any())
                throw new UserFriendlyException("NoFileUpload");

            var resultIds = new List<long>();
            var uploadRoot = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                subsystem
            );

            if (!Directory.Exists(uploadRoot))
            {
                Directory.CreateDirectory(uploadRoot);
            }    
                
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);

                var newFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadRoot, newFileName);

                // 1️⃣ Lưu file vật lý
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 2️⃣ Tạo entity
                FileManager fileEntity = new FileManager
                {
                    Name = fileNameWithoutExt,
                    Extension = extension,
                    Path = $"/uploads/{subsystem}/{newFileName}",
                    Subsystem = subsystem
                };

                // 3️⃣ Lưu DB
                long id = await _fileManagerRepository.InsertAndGetIdAsync(fileEntity);

                resultIds.Add(id);
            }

            return resultIds;
        }


        [AbpAuthorize]
        public async Task DeleteFilesAsync(List<long> ids)
        {
            if (ids == null || !ids.Any())
                return;

            // 1️⃣ Lấy danh sách file trong DB
            List<FileManager> files = await _fileManagerRepository
                .GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();

            if (!files.Any())
            {
                return;
            }
                
            foreach (var file in files)
            {
                // 2️⃣ Xóa file vật lý
                if (!string.IsNullOrWhiteSpace(file.Path))
                {
                    string physicalPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        file.Path.TrimStart('/')
                    );

                    if (File.Exists(physicalPath))
                    {
                        File.Delete(physicalPath);
                    }
                }

                // 3️⃣ Soft Delete DB (vì FullAuditedEntity)
                await _fileManagerRepository.DeleteAsync(file);
            }
            // 👉 ABP auto commit UnitOfWork
        }
    }
}
