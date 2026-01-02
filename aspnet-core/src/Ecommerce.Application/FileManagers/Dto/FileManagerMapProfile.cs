using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Entitys;

namespace Ecommerce.FileManagers.Dto
{
    public class FileManagerMapProfile:Profile
    {
        public FileManagerMapProfile()
        {
            CreateMap<FileManager,FileManagerDto>();
        }
    }
}
