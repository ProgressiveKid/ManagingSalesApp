using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ManagingSalesApp.Shared
{
    public class Order
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле 'Штат' обязательно для заполнения.")]
        public string State { get; set; }

        public List<Window>? Windows { get; set; }
    }

    // Window.cs
    public class Window
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения.")]
        public string Name {get; set; }

        [Required(ErrorMessage = "Поле 'Количество окон' обязательно для заполнения.")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение 'Количество окон' должно быть больше 0.")]
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
        [Required(ErrorMessage = "Поле 'Тип' обязательно для заполнения.")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Поле 'Ширина' обязательно для заполнения.")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение 'Количество окон' должно быть больше 0.")]
        public int Width { get; set; }
        [Required(ErrorMessage = "Поле 'Высота' обязательно для заполнения.")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение 'Количество окон' должно быть больше 0.")]
        public int Height { get; set; }

        // Foreign key
        public int WindowId { get; set; }

        [JsonIgnore]
        public Window? Window { get; set; }
    }
}
