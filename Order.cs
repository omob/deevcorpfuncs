using System;

public class Order
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }

    public int OrderId { get; set; }
    public string Email { get; set; }
    public string ProductId { get; set; }
    public float Price { get; set; }
}