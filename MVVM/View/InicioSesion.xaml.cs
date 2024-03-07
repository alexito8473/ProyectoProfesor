using ProyectoProfesor.ConexionFirebase;

namespace ProyectoProfesor.MVVM.View;

public partial class InicioSesion : ContentPage
{
    private Conexion conexion = new Conexion();
    public InicioSesion()
	{
		InitializeComponent();
	}

    private async void butInicioSesion_ClickedAsync(object sender, EventArgs e) {
        try {
            await conexion.iniciar_sesion(miEmail.Text, miContraseña.Text);
            await Navigation.PushAsync(new MostrarUsuario());
        } catch {
            await DisplayAlert("Fallo en la autentificación", "El usuario o contraseña son incorrectos", "Vale");
        }
    }
}