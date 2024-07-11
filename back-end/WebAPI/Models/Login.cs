namespace WebAPI.Models;

public class Login
{
    public string email { get; set; }
    public string senha { get; set; }
    public string celular { get; set; } = string.Empty;
    public string userName { get; set; } = string.Empty;
    public DateTime nascimento { get; set; } = DateTime.Now;

}
