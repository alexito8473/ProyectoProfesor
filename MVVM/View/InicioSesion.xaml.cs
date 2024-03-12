using ProyectoProfesor.ConexionFirebase;
using ProyectoProfesor.MVVM.ViewModel;

namespace ProyectoProfesor.MVVM.View;
/// <summary> Clase vista donde tenemos la lógica del inicio de sesión </summary>
/// <remarks> Clase donde tenemos todos los controles para realizar el inicio de sesion.</remarks>
public partial class InicioSesion : ContentPage{
    /// <summary> Atributo de la clase Principal</summary>
    /// <remarks> El atributo que se realiza la conexion de la firebase.</remarks>
    private Conexion conexion = new Conexion();
    /// <summary> Constructor de la clase InicioSesion</summary>
    /// <remarks> Se instancia los componentes que tiene el programa. </remarks>
    public InicioSesion(){
		InitializeComponent();
	}
    /// <summary> Botón asyncrono de la clase Principal</summary>
    /// <remarks> Nos realiza las comprobaciones para iniciar sesión el la app </remarks>
    /// <param name="e">Evento del método</param>
    /// <param name="sender"> Objecto del método</param>
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

    private void Button_Clicked(object sender, EventArgs e) {
        Navigation.PushAsync(new Registrarse(conexion));
    }
}