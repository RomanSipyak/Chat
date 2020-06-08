using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chat.Contracts.Dtos
{
    public class PaginationFilterDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int PageNo { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Limit { get; set; }
    }
}
