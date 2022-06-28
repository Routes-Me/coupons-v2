using System;

namespace CouponService.Models.Dto
{
    public class UserReadDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Application { get; set; }
        public string Description { get; set; }
    }
}
