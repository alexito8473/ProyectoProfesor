using ProyectoProfesor.ConexionFirebase;

namespace ProyectoProfesor.MVVM.View;
/// <summary> Clase vista donde tenemos la lógica del registrarse </summary>
/// <remarks> Clase donde tenemos todos los controles para realizar el registro.</remarks>
public partial class Registrarse : ContentPage{
    /// <summary> Atributo de la clase Registrarse</summary>
    /// <remarks> El atributo que se realiza la conexion de la firebase.</remarks>
    private Conexion conexion;
    /// <summary> Constructor de la clase Registrarse</summary>
    /// <param name="conexion">La conexion al servidor</param>
    /// <remarks> Se instancia los componentes que tiene el programa. </remarks>
	public Registrarse(Conexion conexion){
		InitializeComponent();
        this.conexion = conexion;
	}
    /// <summary> Botón asyncrono de la clase Principal</summary>
    /// <remarks> Nos realiza las comprobaciones para registrarse el la app </remarks>
    /// <param name="e">Evento del método</param>
    /// <param name="sender"> Objecto del método</param>
    private async void Button_ClickedAsync(object sender, EventArgs e) {
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
                await conexion.registrarse(miEmail.Text, miContraseña.Text);
                await DisplayAlert("Correcto", "Registraso el profesor correctamente", "OK");
                await Navigation.PopAsync();
            } catch {
                await DisplayAlert("Fallo en la autentificación", "No se ha podidio registrar", "OK");
            }
        }
    }
}