namespace portfolio_api.Data
{
    public class Role
    {
        public int roleId { get; set; }
        public string roleName { get; set; }

        public virtual ICollection<HouseholdUser> HouseholdUsers { get; set; }
    }
}
