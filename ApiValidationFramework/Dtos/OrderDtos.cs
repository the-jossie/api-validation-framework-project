
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiValidationFramework.Dtos
{
    public partial class CreateOrderDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public required string ProductName { get; set; } = "";

        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public int Quantity { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public partial class UpdateOrderDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public required string ProductName { get; set; } = "";

        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public int Quantity { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public partial class OrderResponseDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = "";

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
