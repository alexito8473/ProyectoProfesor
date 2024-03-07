using ProyectoProfesor.MVVM.ViewModel;

namespace ProyectoProfesor.MVVM.View;

public partial class MostrarUsuario : ContentPage
{
	private UsuarioViewModel _viewModel;
	public MostrarUsuario()
	{
		InitializeComponent();
        _viewModel = new UsuarioViewModel();
        BindingContext = _viewModel;
	}

    private void Button_Clicked(object sender, EventArgs e) {
		if (_viewModel.ListaJornadas.Count==0) {
            DisplayAlert("Información", "No tiene jornadas", "ok");
        } else {
            DisplayAlert("Cuidado", _viewModel.ListaJornadas.Count.ToString(), "ok");
        }
    }
}