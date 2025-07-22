namespace EcoRetoAdmin2.Models
{
    public class Comentario
    {
        public string Id { get; set; } // clave generada por Firebase
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string ComentarioTexto { get; set; }
        public string Estatus { get; set; }
        public string Fecha { get; set; } // puedes usar DateTime si prefieres
    }
}
