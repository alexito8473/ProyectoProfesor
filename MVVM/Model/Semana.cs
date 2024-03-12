
namespace ProyectoProfesor.MVVM.Model;
/// <summary> Clase modelo sobre los detalles de un Semana </summary>
/// <remarks>
/// Clase donde vamos a usar como molde para poder almacenar las 
/// semanas donde tendremos el dia que empieza y que termina.
/// </remarks>
public class Semana : ContentPage{
    /// <summary> Atributo de la clase Semana </summary>
    /// <remarks> El atributo nos almacena un conjunto de dias, que pertenecen a esa semana</remarks>
    public List<Dia> Dias { get; set; }
    /// <summary> Atributo de la clase Semana </summary>
    /// <remarks> El atributo el dia donde se inicia la semana</remarks>
    public int DiaInicio { get; set; }
    /// <summary> Atributo de la clase Semana </summary>
    /// <remarks> El atributo el mes de la semana</remarks>
    public string Mes { get; set; }
    /// <summary> Atributo de la clase Semana </summary>
    /// <remarks> El atributo el dia donde se acaba la semana</remarks>
    public int DiaFin { get; set; }
    /// <summary> Constructor de la clase Semana </summary>
    /// <remarks> Se instancia el sus atributos y se instancia la lista de dias.</remarks>
    /// <param name="DiaInicio">El dia de inicio</param>
    /// <param name="DiaFin">El dia de fin</param>
    /// <param name="mes">Ell mes de la semana</param>
    public Semana(int DiaInicio, int DiaFin, string mes) {
        this.DiaInicio = DiaInicio;
        this.DiaFin = DiaFin;
        Dias = new List<Dia>();
        Mes = mes;
    }
}