namespace IdentityApp.Dtos;

public record LoginDto
(
    string UsernameOrEmail,
    string Password
);