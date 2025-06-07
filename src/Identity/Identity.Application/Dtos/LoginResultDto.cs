namespace Identity.Application.Dtos
{
    public class LoginResultDto
    {
        public bool Sucesso { get; set; }
        public string Token { get; set; }
        public string Erro { get; set; }
    }
}
