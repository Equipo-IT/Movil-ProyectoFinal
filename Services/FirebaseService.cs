using Firebase.Database;
using Firebase.Database.Query;
using EcoRetoAdmin2.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EcoRetoAdmin2.Services
{
    public class FirebaseService
    {
        private readonly FirebaseClient firebase;

        public FirebaseService()
        {
            firebase = new FirebaseClient("https://integradora-65813-default-rtdb.firebaseio.com/"); 
        }

        public async Task<bool> CrearComentario(Comentario comentario)
        {
            try
            {
                await firebase
                    .Child("Comentarios")
                    .PostAsync(comentario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ObservableCollection<Comentario>> ObtenerComentarios()
        {
            var comentarios = await firebase
                .Child("Comentarios")
                .OnceAsync<Comentario>();

            var lista = new ObservableCollection<Comentario>();

            foreach (var item in comentarios)
            {
                var c = item.Object;
                c.Id = item.Key;
                lista.Add(c);
            }

            return lista;
        }

        public async Task<bool> ActualizarComentario(Comentario comentario)
        {
            try
            {
                await firebase
                    .Child("Comentarios")
                    .Child(comentario.Id)
                    .PutAsync(comentario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarComentario(string id)
        {
            try
            {
                await firebase
                    .Child("Comentarios")
                    .Child(id)
                    .DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
