using ProyectoProfesor.ConexionFirebase;
using ProyectoProfesor.MVVM.ViewModel;

namespace ProyectoProfesor.MVVM.View;

public partial class InicioSesion : ContentPage
{
    private Conexion conexion = new Conexion();
    public InicioSesion()
	{
		InitializeComponent();
	}

    private async void butInicioSesion_ClickedAsync(object sender, EventArgs e) {
        UsuarioViewModel usuario;
        string mensaje = "";
        bool isEmpty = false;
        if (string.IsNullOrEmpty(miEmail.Text)) {
            mensaje = "Introduce un gmail.";
            isEmpty = true;
        }
        if (string.IsNullOrEmpty(miContraseña.Text)) {
            mensaje = mensaje + "Introduce una contraseña.";
            isEmpty = true;
        }
        if (isEmpty) {
            await DisplayAlert("Advertencia", mensaje, "OK");
        } else {
            try {
                await conexion.iniciar_sesion(miEmail.Text, miContraseña.Text);
                usuario = new UsuarioViewModel();
                Thread.Sleep(2000);
                await DisplayAlert("Correcto", "Has iniciado sesión correctamente", "Vale");
                await Navigation.PushAsync(new MostrarUsuario(usuario));
            } catch {
                await DisplayAlert("Fallo en la autentificación", "El usuario o contraseña son incorrectos", "Vale");
            }
        }
    }
}