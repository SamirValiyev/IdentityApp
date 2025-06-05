namespace IdentityApp.Dtos;

public record ChangePasswordWithTokenDto
(
    string Email,
    string Token,
    string NewPassword
);
