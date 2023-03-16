namespace UserManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long Phone { get; set; }

    public DateTime Birthdate { get; set; }

    public byte[]? AvatarImg { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
