namespace UserManagement.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public long Phone { get; set; }

        public DateTime Birthdate { get; set; }

        public byte[]? AvatarImg { get; set; }

        public string AvatarImgBase64 
        {
            get { return this.AvatarImg != null ? Convert.ToBase64String(this.AvatarImg) : string.Empty; }
        }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
