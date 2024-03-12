using ProyectoProfesor.ConexionFirebase;
using ProyectoProfesor.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProyectoProfesor.MVVM.ViewModel {
    /// <summary> Clase vistaModelo sobre los detalles del usuario </summary>
    /// <remarks>
    /// Clase donde vamos a ser intermediario entre el modelo y su vista.
    /// </remarks>
    public class UsuarioViewModel {
        /// <summary> Atributo de la clase UsuarioViewModel. </summary>
        /// <remarks>Tiene una lista de jornadas.</remarks>
        public ObservableCollection<Jornada> ListaJornadas { get; set; } = new ObservableCollection<Jornada>();
        /// <summary> Atributo de la clase UsuarioViewModel, que realiza la conexión con el servidor. </summary>
        /// <remarks>El atributo instancia la calse Conexión para poder actualizar los datos del usuario.</remarks>
        private Conexion conexion = new Conexion();
        /// <summary> Comando de la clase UsuarioViewModel. </summary>
        /// <remarks>El comando trate de obtener el gmail del usuario.</remarks>
        public ICommand obtenerGmailAlumno { get; set; }
        /// <summary> Atributo de la clase UsuarioViewModel. </summary>
        /// <remarks> El gmail del usuario.</remarks>
        public string gmail { get; set; }
        /// <summary> Atributo de la clase UsuarioViewModel. </summary>
        /// <remarks> El atributo que obtenemos que almacena al usuario y las horas.</remarks>
        public UsuarioEnvoltorio usuarioActual { get; set; }
        /// <summary> Atributo de la clase UsuarioViewModel. </summary>
        /// <remarks> LIsta del usuario envoltorio.</remarks>
        public ObservableCollection<UsuarioEnvoltorio> ListaUsuarios { get; set; } = new ObservableCollection<UsuarioEnvoltorio>();
        /// <summary> Constructor de la clase UsuarioViewModel </summary>
        /// <remarks> Se instancia los atributos necesarios.</remarks>
        public UsuarioViewModel() {
            CargarUsuario();
            ConstruirComandoGmailAlumno();
        }
        /// <summary> Método de la clase UsuarioViewModel </summary>
        /// <remarks> Contruir el comando del gmail</remarks>
        private void ConstruirComandoGmailAlumno() {
            obtenerGmailAlumno = new Command(gmail => {
                ListaJornadas.Clear();
                usuarioActual = ListaUsuarios.Where(t => t.Usuario.Gmail.Equals(gmail)).ToList()[0];
                for (int i = 0; i < usuarioActual.Usuario.Años.Count; i++) {
                    for (int j = 0; j < usuarioActual.Usuario.Años[i].Meses.Count; j++) {
                        for (int k = 0; k < usuarioActual.Usuario.Años[i].Meses[j].Dias.Count; k++) {
                            for (int l = 0; l < usuarioActual.Usuario.Años[i].Meses[j].Dias[k].Jornadas.Count; l++) {
                                ListaJornadas.Add(usuarioActual.Usuario.Años[i].Meses[j].Dias[k].Jornadas[l]);
                            }
                        }
                    }
                }
            });
        }
        /// <summary> Método de la clase UsuarioViewModel </summary>
        /// <remarks> Guarda en la lista de los usuarios</remarks>
        public void CargarUsuario() {
            conexion.GetCliente().Child("Usuario").AsObservable<Usuario>()
                        .Subscribe((user) => {
                            if (user.Object != null) {
                                if (ListaUsuarios.Any(t => t.Usuario.Gmail.ToLower().Equals(user.Object.Gmail.ToLower()))) {
                                    ListaUsuarios.Remove(ListaUsuarios.Where(t => t.Usuario.Gmail.ToLower().Equals(user.Object.Gmail.ToLower())).ToList()[0]); ;
                                    ListaUsuarios.Add(new UsuarioEnvoltorio(user.Object, calcularTiempoJornadas(user.Object)));
                                } else {
                                    ListaUsuarios.Add(new UsuarioEnvoltorio(user.Object, calcularTiempoJornadas(user.Object)));
                                }
                            }
                        });
        }
        /// <summary> Método de la clase UsuarioViewModel </summary>
        /// <remarks> Comprueba si ese mes tiene jornadas</remarks>
        /// <param name="mes"> Mes del año</param>
        /// <returns>true: tiene jornada false:no tiene jornada</returns>
        public bool TieneJornadasElMes(Mes mes) {
            for (int i=0;i< mes.Dias.Count;i++) {
                if(mes.Dias[i].Jornadas.Count>0) {
                    return true;
                }
            }
            return false;
        }
        /// <summary> Método de la clase UsuarioViewModel </summary>
        /// <remarks> Cuenta las horas que tiene en total del usuario</remarks>
        /// <param name="usuario">El usuario</param>
        /// <returns> El numero que horas totales del usuario</returns>
        public int calcularTiempoJornadas(Usuario usuario) {
            int horas = 0;
            for (int i = 0; i < usuario.Años.Count; i++) {
                for (int j = 0; j < usuario.Años[i].Meses.Count; j++) {
                    for (int k = 0; k < usuario.Años[i].Meses[j].Dias.Count; k++) {
                        for (int l = 0; l < usuario.Años[i].Meses[j].Dias[k].Jornadas.Count; l++) {
                            horas = (int)(horas +usuario.Años[i].Meses[j].Dias[k].Jornadas[l].TiempoEmpleado);
                        }
                    }
                }
            }
            return horas;
        }
    }
}
