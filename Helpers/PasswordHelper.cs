namespace WebApplication1.Helpers
{
    /// <summary>
    /// Helper reutilizable para manejo de contraseñas
    /// Centraliza la lógica de encriptación para no repetirla
    /// </summary>
    public class PasswordHelper
    {
        /// <summary>
        /// Encripta una contraseña usando BCrypt con salt automático
        /// Nunca guardar contraseñas en texto plano
        /// </summary>
        /// <param name="contrasena">Contraseña en texto plano</param>
        /// <returns>Hash seguro de la contraseña</returns>
        public static string Encriptar(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        /// <summary>
        /// Compara una contraseña en texto plano con su hash
        /// Usado en el login para verificar credenciales
        /// </summary>
        /// <param name="contrasena">Contraseña ingresada por el usuario</param>
        /// <param name="hash">Hash guardado en la BD</param>
        /// <returns>true si coinciden, false si no</returns>
        public static bool Comparar(string contrasena, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, hash);
        }
    }
}
