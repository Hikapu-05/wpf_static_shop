using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StaticShop._0._2.Models;

[Table("favorites")]
[Index("ProductId", Name = "product_id")]
[Index("UserId", Name = "user_id")]
public partial class Favorite
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("product_id", TypeName = "int(11)")]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Favorites")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Favorites")]
    public virtual User User { get; set; } = null!;
}
