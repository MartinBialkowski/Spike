namespace EFCoreSpike5.Models
{
    public class UserClaim
    {
        public int UserId { get; set; }
        public int ClaimId { get; set; }
        public User User { get; set; }
        public Claim Claim { get; set; }
    }
}
