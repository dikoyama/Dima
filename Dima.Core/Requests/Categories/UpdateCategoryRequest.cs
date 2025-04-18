﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Categories
{
    public class UpdateCategoryRequest : Request
    {
        [Required]
        public long Id { get; set; }
        [Required(ErrorMessage = "Título inválido")]
        [MaxLength(80)]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Descrição inválida")]
        public string Description { get; set; } = string.Empty;
    }
}
