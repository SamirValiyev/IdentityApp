namespace IdentityApp.Dtos;

public record ChangePasswordDto(
    string CurrentPassword,
    string NewPassword,
    Guid Id
    );

