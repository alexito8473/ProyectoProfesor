using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoProfesor.MVVM.Model {
    /// <summary> Clase modelo envoltorio del Usuario </summary>
    /// <remarks> Clase donde vamos a envolver al Usuario y añadirle más datos revelantes.</remarks>
    public class UsuarioEnvoltorio {
        /// <summary> Atributo de la clase UsuarioEnvoltorio</summary>
        /// <remarks> El atributo tiene almacenado al usuario en específico.</remarks>
        public Usuario Usuario { get; set; }
        /// <summary> Atributo de la clase UsuarioEnvoltorio</summary>
        /// <remarks> El atributo tiene almacenado el tiempo que horas que tiene el usuario.</remarks>
        public int tiempoJornadas { get; set; }
        /// <summary> Constructor de la clase UsuarioEnvoltorio </summary>
        /// <remarks> Se instancia el sus atributos de la clase</remarks>
        /// <param name="Usuario">El usuario en cuestión</param>
        /// <param name="tiempoJornadas">El tiempo que ha trabajado</param>
        public UsuarioEnvoltorio(Usuario Usuario, int tiempoJornadas) {
            this.Usuario = Usuario;
            this.tiempoJornadas = tiempoJornadas;
        }
    }
}
