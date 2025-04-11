using System.Collections;

namespace ASP_spr321.Models
{
    public class RestResponse
    {
        public String Service { get; set; } = null!;
        public RestStatus Status { get; set; } = new();
        public long CacheTime { get; set; } = 0L;
        public String DataType { get; set; } = "empty";
        public Dictionary<String, String> Meta { get; set; } = [];
        public Manipulations Manipulations { get; set; } = new();
        public Object? Data { get; set; }
    }
     public class RestStatus
    {
        public bool IsOk { get; set; } = true;
        public int Code { get; set; } = 200;
        public String Phrase { get; set; } = "OK";
    }

    public class Manipulations
    {
        public string? Create { get; set; }
        public string? Read { get; set; }
        public string? Update{ get; set; }
        public string? Delete { get; set; }

    }
}
