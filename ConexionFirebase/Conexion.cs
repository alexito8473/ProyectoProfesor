using Firebase.Auth.Providers;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using ProyectoProfesor.MVVM.Model;
using Firebase.Database.Query;


namespace ProyectoProfesor.ConexionFirebase {
    public class Conexion {
        private FirebaseClient cliente = new FirebaseClient(Constante.Constante.REALMTIME_STORAGE);

        public FirebaseClient GetCliente() {
            return cliente;
        }
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
        public async Task<String> iniciar_sesion(string email, string password) {
            var client = obtenerToken();
            var authResult = await client.SignInWithEmailAndPasswordAsync(email, password);
            return await authResult.User.GetIdTokenAsync();
        }
    }
}
