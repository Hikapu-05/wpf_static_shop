using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StaticShop._0._2.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [StringLength(255)]
    public string Login { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    [StringLength(255)]
    public string Role { get; set; } = null!;

    public double Balance { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("User")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
