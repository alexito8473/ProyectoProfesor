using ProyectoProfesor.ConexionFirebase;
using ProyectoProfesor.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoProfesor.MVVM.ViewModel {
    class UsuarioViewModel {
        public ObservableCollection<Jornada> ListaJornadas { get; set; } = new ObservableCollection<Jornada>();
        private Conexion conexion = new Conexion();
        public ICommand obtenerGmailAlumno { get; set; }
        public string gmail { get; set; }
        public ObservableCollection<Usuario> ListaUsuarios { get; set; } = new ObservableCollection<Usuario>();
        public UsuarioViewModel() {
            CargarUsuario();
            ConstruirComandoGmailAlumno();
        }

        private void ConstruirComandoGmailAlumno() {
            obtenerGmailAlumno = new Command(gmail => {
                ListaJornadas.Clear();
                Usuario usuario = ListaUsuarios.Where(t => t.Gmail.Equals(gmail)).ToList()[0];
                for (int i = 0; i < usuario.Años.Count; i++) {
                    for (int j = 0; j < usuario.Años[i].Meses.Count; j++) {
                        for (int k = 0; k < usuario.Años[i].Meses[j].Dias.Count; k++) {
                            for (int l = 0; l < usuario.Años[i].Meses[j].Dias[k].Jornadas.Count; l++) {
                                ListaJornadas.Add(usuario.Años[i].Meses[j].Dias[k].Jornadas[l]);
                            }
                        }
                    }
                }
            });
        }
        public void CargarUsuario() {
            ListaUsuarios = new ObservableCollection<Usuario>();
            conexion.GetCliente().Child("Usuario").AsObservable<Usuario>()
                        .Subscribe((user) => {
                            if (user.Object != null) {
                                ListaUsuarios.Add(user.Object);
                            }
                        });
        }


    }
}
