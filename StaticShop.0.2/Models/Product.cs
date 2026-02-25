using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StaticShop._0._2.Models;

[Table("products")]
[Index("CategoryId", Name = "category_id")]
public partial class Product
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = null!;

    [Column("price")]
    [Precision(10, 0)]
    public decimal Price { get; set; }

    [Column("quantity", TypeName = "int(11)")]
    public int Quantity { get; set; }

    [Column("category_id", TypeName = "int(11)")]
    public int CategoryId { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
