using ProyectoProfesor.MVVM.Model;
using ProyectoProfesor.MVVM.ViewModel;

namespace ProyectoProfesor.MVVM.View;
/// <summary> Clase vista donde podemos ver la informacion del alumno </summary>
/// <remarks> Clase donde vamos a usar para ver todos los datos del alumno.</remarks>
public partial class InformacionUsuario : ContentPage{
    /// <summary> Constructor de la clase InformacionUsuario</summary>
    /// <remarks> Se instancia los componentes que tiene el programa, como los datos del usuario</remarks>
    /// <param name="usuario">Nos pasamos el el videmodel que controla al usuario que hemos escogido</param>
    public InformacionUsuario(Usuario usuario){
		InitializeComponent();
        BindingContext = usuario;
    }
}