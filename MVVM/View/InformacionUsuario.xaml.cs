using ProyectoProfesor.MVVM.ViewModel;

namespace ProyectoProfesor.MVVM.View;

public partial class InformacionUsuario : ContentPage
{
	public InformacionUsuario(UsuarioViewModel usuario)
	{
		InitializeComponent();
        miImagen.Source = usuario.usuarioActual.Imagen;
        miNombre.Text = usuario.usuarioActual.NombreCompleto;
        miGrado.Text = usuario.usuarioActual.Grado;
        miCentroDo.Text = usuario.usuarioActual.CentroDocente;
        miCentroTra.Text = usuario.usuarioActual.CentroTrabajo;
        miTutor.Text = usuario.usuarioActual.TutorTrabajo;
        miProfesor.Text = usuario.usuarioActual.ProfesorResponsable;
        miGmail.Text = usuario.usuarioActual.Gmail;
        miCiclo.Text= usuario.usuarioActual.CicloFormativo;
    }
}