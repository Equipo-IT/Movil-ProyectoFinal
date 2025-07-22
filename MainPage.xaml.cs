using EcoRetoAdmin2.Models;
using EcoRetoAdmin2.Services;
using System.Collections.ObjectModel;

namespace EcoRetoAdmin2
{



    public partial class MainPage : ContentPage
    {
        FirebaseService firebaseService;
        ObservableCollection<Comentario> comentarios;
        Comentario comentarioSeleccionado;

        public MainPage()
        {
            InitializeComponent();
            firebaseService = new FirebaseService();
            CargarComentarios();
        }

        private async void CargarComentarios()
        {
            comentarios = await firebaseService.ObtenerComentarios();
            ComentariosCollectionView.ItemsSource = comentarios;
        }

        private void NuevoButton_Clicked(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            comentarioSeleccionado = null;
            NombreEntry.Text = "";
            CorreoEntry.Text = "";
            ComentarioEntry.Text = "";
            EstatusEntry.Text = "";
            FechaEntry.Text = "";
            EliminarButton.IsEnabled = false;
            ComentariosCollectionView.SelectedItem = null;
        }

        private async void GuardarButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreEntry.Text) ||
                string.IsNullOrWhiteSpace(CorreoEntry.Text) ||
                string.IsNullOrWhiteSpace(ComentarioEntry.Text) ||
                string.IsNullOrWhiteSpace(EstatusEntry.Text) ||
                string.IsNullOrWhiteSpace(FechaEntry.Text))
            {
                await DisplayAlert("Error", "Completa todos los campos.", "OK");
                return;
            }

            if (comentarioSeleccionado == null)
            {
                var nuevo = new Comentario
                {
                    Nombre = NombreEntry.Text,
                    Correo = CorreoEntry.Text,
                    ComentarioTexto = ComentarioEntry.Text,
                    Estatus = EstatusEntry.Text,
                    Fecha = FechaEntry.Text
                };

                bool creado = await firebaseService.CrearComentario(nuevo);
                if (creado)
                {
                    await DisplayAlert("Éxito", "Comentario guardado", "OK");
                    LimpiarFormulario();
                    CargarComentarios();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo guardar", "OK");
                }
            }
            else
            {
                comentarioSeleccionado.Nombre = NombreEntry.Text;
                comentarioSeleccionado.Correo = CorreoEntry.Text;
                comentarioSeleccionado.ComentarioTexto = ComentarioEntry.Text;
                comentarioSeleccionado.Estatus = EstatusEntry.Text;
                comentarioSeleccionado.Fecha = FechaEntry.Text;

                bool actualizado = await firebaseService.ActualizarComentario(comentarioSeleccionado);
                if (actualizado)
                {
                    await DisplayAlert("Éxito", "Comentario actualizado", "OK");
                    LimpiarFormulario();
                    CargarComentarios();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo actualizar", "OK");
                }
            }
        }

        private async void EliminarButton_Clicked(object sender, EventArgs e)
        {
            if (comentarioSeleccionado == null) return;

            bool confirm = await DisplayAlert("Confirmar", $"¿Eliminar comentario de {comentarioSeleccionado.Nombre}?", "Sí", "No");
            if (confirm)
            {
                bool eliminado = await firebaseService.EliminarComentario(comentarioSeleccionado.Id);
                if (eliminado)
                {
                    await DisplayAlert("Éxito", "Comentario eliminado", "OK");
                    LimpiarFormulario();
                    CargarComentarios();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar", "OK");
                }
            }
        }

        private void ComentariosCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comentarioSeleccionado = e.CurrentSelection.FirstOrDefault() as Comentario;

            if (comentarioSeleccionado != null)
            {
                NombreEntry.Text = comentarioSeleccionado.Nombre;
                CorreoEntry.Text = comentarioSeleccionado.Correo;
                ComentarioEntry.Text = comentarioSeleccionado.ComentarioTexto;
                EstatusEntry.Text = comentarioSeleccionado.Estatus;
                FechaEntry.Text = comentarioSeleccionado.Fecha;
                EliminarButton.IsEnabled = true;
            }
            else
            {
                LimpiarFormulario();
            }
        }
    }
}
