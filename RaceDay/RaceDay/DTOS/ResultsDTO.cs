using System;
using System.Collections.Generic;
using System.Text;

namespace RaceDay.DTOS
{
    public class ResultDto
    {
        public int EnrollmentId { get; set; }
        public string FinishTime { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
