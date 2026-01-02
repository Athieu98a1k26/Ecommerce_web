using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MimeTypes;
using Ecommerce.Controllers;
using Ecommerce.FileManagers;
using Ecommerce.FileManagers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Host.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class FileController : EcommerceControllerBase
    {
        private readonly IFileMangerAppService _fileMangerAppService;

        public FileController(
            IFileMangerAppService fileMangerAppService
        )
        {
            _fileMangerAppService = fileMangerAppService;
        }

        [HttpGet("DownloadFile")]
        public async Task<ActionResult> DownloadFile([FromQuery] FileManagerDto file)
        {
            byte[] fileBytes = await _fileMangerAppService.GetFileAsync(file.Id);
            if (fileBytes == null)
            {
                return NotFound(L("FileNotFound"));
            }

            return File(fileBytes, "application/octet-stream", $"{file.Name}{file.Extension}");
        }
    }
}
