using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Order {
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("productName")]
    public required string ProductName { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }
}
