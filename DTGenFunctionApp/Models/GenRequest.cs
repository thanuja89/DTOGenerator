using Core;

namespace DTGenFunctionApp.Models
{
    public class GenRequest
    {
        public string Source { get; set; }
        public bool IsCamelCaseEnabled { get; set; }
        public Language Language { get; set; }
        public bool IsMapToInterfaceEnabled { get; set; }
    }
}
