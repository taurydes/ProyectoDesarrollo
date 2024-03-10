using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class ErrorResponseServiceModel
    {
        public int ErrorCode { get; set; }
        public List<string>? ErrorMessages { get; set; }
    }
}
