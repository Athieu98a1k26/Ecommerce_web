using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.FileManagers.Dto
{
    public class FileManagerDto
    {
        public long Id {  get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}
