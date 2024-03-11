
namespace ProyectoProfesor.MVVM.Model;

public class Semana : ContentPage
{
    public List<Dia> Dias { get; set; }

    public int DiaInicio { get; set; }
    public string Mes { get; set; }
    public int DiaFin { get; set; }
    public Semana(int DiaInicio, int DiaFin, string mes) {
        this.DiaInicio = DiaInicio;
        this.DiaFin = DiaFin;
        Dias = new List<Dia>();
        Mes = mes;
    }
}