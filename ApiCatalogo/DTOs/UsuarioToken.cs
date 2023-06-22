namespace ApiCatalogo.DTOs;

public class UsuarioToken
{
    public bool Authenticated;
    public DateTime Expiration;
    public string Token;
    public string Message;
}

