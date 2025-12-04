namespace Iara.DomainModel.RequestDTOs
{
    public class UpdateUserRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
    }
}