using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ManagingSalesApp.Shared
{
    public class Order
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The 'Title' field is mandatory.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The 'Staff' field is mandatory.")]
        public string State { get; set; }

        public List<Window>? Windows { get; set; }
    }

    // Window.cs
    public class Window
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The 'Title' field is mandatory.")]
        public string Name {get; set; }

        [Required(ErrorMessage = "Поле 'Количество окон' обязательно для заполнения.")]
        [Range(1, int.MaxValue, ErrorMessage = "The 'Number of windows' field is mandatory.")]
        public int QuantityOfWindows { get; set; }
        public int TotalSubElements { get; set; }

        public List<SubElement>? SubElements { get; set; }

        // Foreign key
        
        public int OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
    }

    // SubElement.cs
    public class SubElement
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The 'Type' field is mandatory")]
        public string Type { get; set; }
        [Required(ErrorMessage = "The 'Width' field is mandatory")]
        [Range(1, int.MaxValue, ErrorMessage = "The 'Number of windows' value must be greater than 0")]
        public int Width { get; set; }
        [Required(ErrorMessage = "The 'Height' field is mandatory")]
        [Range(1, int.MaxValue, ErrorMessage = "The 'Number of windows' value must be greater than 0")]
        public int Height { get; set; }

        // Foreign key
        public int WindowId { get; set; }

        [JsonIgnore]
        public Window? Window { get; set; }
    }

    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }

        public string KeyWord{get; set; }
    }
    public class ActivePageService
    {
        public string ActivePage { get; set; }
    }

}
