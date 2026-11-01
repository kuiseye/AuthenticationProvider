using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationProvider.Domain.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class AppUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id {get;set;}

    [MaxLength(256)]
    [NotNull]
    public string Username {get;set;} = "";
    [EmailAddress]
    [MaxLength(256)]
    [NotNull]
    public string Email {get;set;} = "";
    [MaxLength(256)]
    public string? PasswordHash {get;set;} = "";
    [Precision(0)]
    public DateTimeOffset CreatedAt {get;set;}
    [Precision(0)]
    public DateTimeOffset UpdatedAt {get;set;}

    
    public AppUserRole Role {get;set;}

    private AppUser(){}
    public AppUser(Guid id, string username, string email, string passwordHash,
        DateTimeOffset createdAt, DateTimeOffset updatedAt, AppUserRole role)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
        Role = role;
    }
}

public enum AppUserRole
{
    User,
    Admin
}