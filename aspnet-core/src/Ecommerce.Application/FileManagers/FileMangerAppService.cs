using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Ecommerce.Entitys;
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
    }
    public class FileMangerAppService: EcommerceAppServiceBase,IFileMangerAppService
    {
        private readonly IRepository<FileManager,long> _fileManagerRepository;

        public FileMangerAppService(IRepository<FileManager, long> fileManagerRepository)
        {
            _fileManagerRepository = fileManagerRepository;
        }

        public async Task<List<long>> UploadFilesAsync(
            List<IFormFile> files,
            string subsystem
        )
        {
            if (files == null || !files.Any())
                throw new UserFriendlyException("No file uploaded");

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
                var fileEntity = new FileManager
                {
                    Name = fileNameWithoutExt,
                    Extension = extension,
                    Path = $"/uploads/{subsystem}/{newFileName}",
                    Subsystem = subsystem
                };

                // 3️⃣ Lưu DB
                await _fileManagerRepository.InsertAsync(fileEntity);

                resultIds.Add(fileEntity.Id);
            }

            return resultIds;
        }

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
