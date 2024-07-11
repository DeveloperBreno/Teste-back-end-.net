using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Models;

public class TarefaModel
{
    public int id { get; set; }
    public string titulo { get; set; }
    public string descricao { get; set; }
    public string status { get; set; }
    public string data { get; set; }
    public string idUsuario { get; set; } = string.Empty;

    public void SetarDataParaTela(DateTime dateTime)
    {
        data = dateTime.ToString("yyyy-MM-ddTHH:mm");
    }

}
