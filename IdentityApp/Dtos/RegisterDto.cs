namespace IdentityApp.Dtos
{
   public record RegisterDto(
       string Password,
       string Username,
       string FirstName,
       string Email,
       string LastName
       );
}
