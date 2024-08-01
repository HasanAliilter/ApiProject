using Microsoft.AspNetCore.Identity;

namespace Api.Domain.Entities
{
    public class User : IdentityUser<Guid> //Userlerimiz hangi tip bir primary key ile tutmak istiyoruz
    {
        public string FullName { get; set; }
        public string? RefreshToken { get; set; } //kullanıcılar sürekli login sayfasına düşmemesi için 
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
