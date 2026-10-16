using System;
using System.Collections.Generic;
using System.Text;

namespace RaceDay.DTOS
{
    public class CategoryDto
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
