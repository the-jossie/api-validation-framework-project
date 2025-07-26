
namespace ApiValidationFramework.Dtos
{
    public partial class CreateOrderDto
    {
        public required string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
