namespace AdminWebApp.Server.Models
{
    public class Bus
    {        
        public required  int seq { get; set; }        // 添    seq: number;
        public required  string station { get; set; } // 站名
        public required  string arrivaltime { get; set; } // 到站時間
        public required  string color { get; set; } // 顏色
        public required  string type { get; set; } // 類型
    }

    public class Equationinfo
    {
        public required string Name { get; set; } // 名稱

        public required string Equation { get; set; } // 公式
    }
}