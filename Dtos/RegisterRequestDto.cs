namespace TodoAppApi.Dtos;

public class RegisterRequestDto : LoginRequestDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
}