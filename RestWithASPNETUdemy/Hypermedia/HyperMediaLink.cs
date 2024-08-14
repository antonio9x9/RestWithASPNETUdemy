using System.Globalization;
using System.Text;

namespace RestWithASPNETUdemy.Hypermedia
{
    public sealed class HyperMediaLink
    {
        public string Rel {  get; set; }
        
        private string _href = string.Empty;
        public string Href {
            get
            {
                object _lock = new object();
                lock (_lock)
                {
                    StringBuilder sb = new StringBuilder(_href);
                    return sb.Replace("%2F", "/").ToString();
                }
            }  
            set => _href = value;
        }
        public string Type {  get; set; }
        public string Action {  get; set; }
    }
}
