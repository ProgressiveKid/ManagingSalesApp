namespace ManagingSalesApp.Shared
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }

        public List<Window> Windows { get; set; }
    }

    // Window.cs
    public class Window
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityOfWindows { get; set; }
        public int TotalSubElements { get; set; }

        public List<SubElement> SubElements { get; set; }

        // Foreign key
        public int OrderId { get; set; }
      //  public Order Order { get; set; }
    }

    // SubElement.cs
    public class SubElement
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Foreign key
        public int WindowId { get; set; }
      //  public Window Window { get; set; }
    }
}
