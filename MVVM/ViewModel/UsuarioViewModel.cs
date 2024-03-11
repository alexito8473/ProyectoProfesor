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
    public class UsuarioViewModel {
        public ObservableCollection<Jornada> ListaJornadas { get; set; } = new ObservableCollection<Jornada>();
        private Conexion conexion = new Conexion();
        public ICommand obtenerGmailAlumno { get; set; }
        public string gmail { get; set; }
        public Usuario usuarioActual { get; set; }
        public ObservableCollection<Usuario> ListaUsuarios { get; set; } = new ObservableCollection<Usuario>();
        public UsuarioViewModel() {
            CargarUsuario();
            ConstruirComandoGmailAlumno();
        }

        private void ConstruirComandoGmailAlumno() {
            obtenerGmailAlumno = new Command(gmail => {
                ListaJornadas.Clear();
                usuarioActual = ListaUsuarios.Where(t => t.Gmail.Equals(gmail)).ToList()[0];
                for (int i = 0; i < usuarioActual.Años.Count; i++) {
                    for (int j = 0; j < usuarioActual.Años[i].Meses.Count; j++) {
                        for (int k = 0; k < usuarioActual.Años[i].Meses[j].Dias.Count; k++) {
                            for (int l = 0; l < usuarioActual.Años[i].Meses[j].Dias[k].Jornadas.Count; l++) {
                                ListaJornadas.Add(usuarioActual.Años[i].Meses[j].Dias[k].Jornadas[l]);
                            }
                        }
                    }
                }
            });
        }
        public void CargarUsuario() {
            conexion.GetCliente().Child("Usuario").AsObservable<Usuario>()
                        .Subscribe((user) => {
                            if (user.Object != null) {
                                if (ListaUsuarios.Any(t => t.Gmail.ToLower().Equals(user.Object.Gmail.ToLower()))) {
                                    ListaUsuarios.Remove(ListaUsuarios.Where(t => t.Gmail.ToLower().Equals(user.Object.Gmail.ToLower())).ToList()[0]); ;
                                    ListaUsuarios.Add(user.Object);
                                } else {
                                    ListaUsuarios.Add(user.Object);
                                }
                            }
                        });
        }

        public bool TieneJornadasElMes(Mes mes) {
            for (int i=0;i< mes.Dias.Count;i++) {
                if(mes.Dias[i].Jornadas.Count>0) {
                    return true;
                }
            }
            return false;
        }
    }
}
