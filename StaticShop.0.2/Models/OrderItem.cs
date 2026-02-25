using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StaticShop._0._2.Models;

[Table("order_items")]
[Index("OrderId", Name = "order_id")]
[Index("ProductId", Name = "product_id")]
public partial class OrderItem
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("order_id", TypeName = "int(11)")]
    public int OrderId { get; set; }

    [Column("product_id", TypeName = "int(11)")]
    public int ProductId { get; set; }

    [Column("quantity", TypeName = "int(11)")]
    public int Quantity { get; set; }

    [Column("price")]
    [Precision(10, 0)]
    public decimal Price { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderItems")]
    public virtual Product Product { get; set; } = null!;
}
