namespace RegenbogenRadar.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HourlyBrief
    {
        public string Time { get; set; } = string.Empty;
        public int TempC { get; set; }
        public string? Summary { get; set; }
    }
}