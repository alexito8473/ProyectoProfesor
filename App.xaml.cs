using ProyectoProfesor.MVVM.View;

namespace ProyectoProfesor {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            MainPage = new NavigationPage(new InicioSesion());
        }
    }
}
