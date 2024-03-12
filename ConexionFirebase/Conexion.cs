using Firebase.Auth.Providers;
using Firebase.Auth;
using Firebase.Database;

namespace ProyectoProfesor.ConexionFirebase {
    /// <summary> Clase donde se realiza las distinas conexines. </summary>
    /// <remarks> Esta clase esta conformada para poder realizar distintas conexiones a firebase.</remarks>
    public class Conexion {
        /// <summary> Atributo de la clase Conexión </summary>
        /// <remarks> El atributo nos sirve como conexión a al storage de firebase, que tiene almacenado todos los datos de los usuarios</remarks>
        private FirebaseClient cliente = new FirebaseClient(Constante.Constante.REALMTIME_STORAGE);
        /// <summary> Método para mostrar el al atributo cliente</summary>
        /// <remarks> Con el mostramos al conexion del cliente.</remarks>
        /// <returns> Devuelve la conexion el realtime database</returns>
        public FirebaseClient GetCliente() {
            return cliente;
        }
        /// <summary> Método para mostrar el al atributo cliente</summary>
        /// <remarks> Con el mostramos al conexion del cliente.</remarks>
        /// <returns> Devuelve el token del autentificator</returns>
        private FirebaseAuthClient obtenerToken() {
            var client = new FirebaseAuthClient(new FirebaseAuthConfig() {
                ApiKey = Constante.Constante.API_WEB_PROFESOR,
                AuthDomain = Constante.Constante.AuthDomain_PROFESOR,
                Providers = new FirebaseAuthProvider[]
                {
                new EmailProvider()
                }
            });
            return client;
        }
        /// <summary> Métodos asyncrono para iniciar sesión</summary>
        /// <remarks> El método nos logea con un gmail y contraseña en concreto en la firebase</remarks>
        /// <returns> Devuelve el token del inicio de sesión</returns>
        public async Task<String> iniciar_sesion(string email, string password) {
            var client = obtenerToken();
            var authResult = await client.SignInWithEmailAndPasswordAsync(email, password);
            return await authResult.User.GetIdTokenAsync();
        }

        /// <summary> Métodos asyncrono para registrarte</summary>
        /// <remarks> El método nos registra con un gmail y contraseña en concreto en la firebase</remarks>
        /// <param name="email">El mail del usuario</param>
        /// <param name="password">La contraseña del usuario</param>
        /// <returns> Devuelve el token del registro</returns>
        public async Task<String> registrarse(string email, string password) {
            var client = obtenerToken();
            var authResult = await client.CreateUserWithEmailAndPasswordAsync(email, password);
            return await authResult.User.GetIdTokenAsync();
        }
    }
}
