namespace AuthenticationProvider.Application.Dtos.AppUserDtos;

public record AppUserDto(string Username);
public record RegisterAppUserForm(string Username, string Email, string Password,string VerifyPassword){}
public record LoginAppUserForm(string Username, string Password){}