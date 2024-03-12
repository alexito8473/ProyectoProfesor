using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ProyectoProfesor.MVVM.Model;
using ProyectoProfesor.MVVM.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoProfesor.MVVM.View;
/// <summary> Clase vista donde tenemos la lógica del mostrarUsuario </summary>
/// <remarks> Clase tendra los métodos crear el pdf, además las distinas navegaciones.</remarks>
public partial class MostrarUsuario : ContentPage{
    /// <summary> Atributo de la clase MostrarUsuario</summary>
    /// <remarks> El atributo es el vide model de la clase.</remarks>
    private UsuarioViewModel _viewModel;
    /// <summary> Constructor de la clase MostrarUsuario</summary>
    /// <remarks> Se instancia los componentes que tiene el programa. </remarks>
    /// <param name="usuario">Modelo vista del usuario</param>
    public MostrarUsuario(UsuarioViewModel usuario){
		InitializeComponent();
        _viewModel = usuario;
        DisplayAlert("Correcto", "Recopilando los datos de los alumnos", "Vale");
        BindingContext = _viewModel;
	}
    /// <summary> Botón asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Nos manda a la informacion del usuario </remarks>
    /// <param name="e">Evento del método</param>
    /// <param name="sender"> Objecto del método</param>
    private void Button_Clicked_1(object sender, EventArgs e) {
        Navigation.PushAsync(new InformacionUsuario(_viewModel.usuarioActual.Usuario));
    }
    /// <summary> Botón asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Nos crea el pdf del usuario. </remarks>
    /// <param name="e">Evento del método</param>
    /// <param name="sender"> Objecto del método</param>
    private void Button_Clicked(object sender, EventArgs e) {
		if (_viewModel.ListaJornadas.Count==0) {
            DisplayAlert("Información", "No tiene jornadas", "ok");
        } else {
            crearPDF2();
            DisplayAlert("Información", "Se ha creado el pdf correctamente", "ok");
        }
    }
    /// <summary> Botón asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Nos crea el pdf del usuario. </remarks>
    private async void crearPDF2() {
        string fileName = "Registro_" + _viewModel.usuarioActual.Usuario.NombreCompleto + DateTime.Now.ToString("dd_MM_yyyy") + ".pdf";

#if ANDROID
        await Permissions.RequestAsync<Permissions.StorageWrite>();
        var filepath = "/sdcard/Download/"+fileName;
#else
        var filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
#endif
        using (PdfWriter writer = new PdfWriter(filepath)) {
            List <Semana> semanas= new List<Semana>();
            Document document = new Document(new PdfDocument(writer));
            for (int i = 0; i < _viewModel.usuarioActual.Usuario.Años.Count; i++) {
                for (int j = 0; j < _viewModel.usuarioActual.Usuario.Años[i].Meses.Count; j++) {
                    if (_viewModel.TieneJornadasElMes(_viewModel.usuarioActual.Usuario.Años[i].Meses[j])) {// Tiene una jornada?
                        semanas = new List<Semana>();
                        semanas = crearListaSemanas( _viewModel.usuarioActual.Usuario.Años[i].Meses[j]);
                        for (int k = 0; k < semanas.Count; k++) {
                            if (JornadaSemana(semanas[k])) {
                                await titularAsync(document, _viewModel.usuarioActual.Usuario.Años[i].fecha, _viewModel.usuarioActual.Usuario.Años[i].Meses[j].Nombre, semanas[k]);
                                ContruirSemana(document, semanas[k]);
                                await FooterAsync(document);
                                document.Add(new AreaBreak());
                            }
                        }
                    }
                }
            }
            document.Close();
        }
    }
    /// <summary> Botón asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Nos devuelve de un mes una lista de semanas. </remarks>
    /// <param name="mes"> El mes de un año</param>
    /// <returns> Devuelve la lista de semanas de un mes</returns>
    private List<Semana> crearListaSemanas( Mes mes) {
        List<Semana> semanas = new List<Semana>();
        List<Dia> dias= new List<Dia>(mes.Dias.OrderBy(t => t.DiaActual).ToList());
        int sumando=0;
        int diasMaximo = mes.diastotales(mes.Nombre);
        for (int k = 0; k < 5; k++) {
            if (sumando+7 > diasMaximo) {
                semanas.Add(new Semana(sumando+1, diasMaximo, mes.Nombre));
                for (int i= sumando; i< dias.Count;i++) {
                    semanas[k].Dias.Add(dias[i]) ;
                }
            } else {
                semanas.Add(new Semana(sumando+1, sumando + 7, mes.Nombre));
                for (int i = sumando; i < sumando+7; i++) {
                    semanas[k].Dias.Add(dias[i]);
                }
            }
            sumando = sumando + 7;
        }
        return semanas;
    }
    /// <summary> Botón asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Nos devuelve de un mes una lista de semanas. </remarks>
    /// <param name="document">El documento donde se trabaja</param>
    /// <param name="semana">Semana donde se trabaja</param>
    /// <param name="año">El año donde se trabaja</param>
    /// <param name="mes">El mes donde se trabaja</param>
    /// <returns> Nos devuelve un Task</returns>
    private async Task titularAsync(Document document, string año, string mes, Semana semana) {
        iText.Layout.Element.Image image = new iText.Layout.Element.Image(
            ImageDataFactory.Create(await conversorAsync(Constante.Constante.HEADER)))
            .SetHorizontalAlignment((iText.Layout.Properties.HorizontalAlignment?)HorizontalAlignment.Center).SetWidth(500);
            document.Add(image);
            Table tablaTitulo2 = new Table(2);
            tablaTitulo2.AddHeaderCell("Semana del "+ semana.DiaInicio+ " al "+ semana.DiaFin +" de " + semana.Mes + " 2024");
            tablaTitulo2.AddHeaderCell(createCellNoBordes2(" "));
            tablaTitulo2.SetWidth(500).SetFontSize(10);
            tablaTitulo2.AddHeaderCell("Centro docente:"+_viewModel.usuarioActual.Usuario.CentroDocente+"\nProfesor/a responsable seguimiento:"+_viewModel.usuarioActual.Usuario.ProfesorResponsable);
            tablaTitulo2.AddHeaderCell("Centro de trabajo coloborador:"+ _viewModel.usuarioActual.Usuario.CentroTrabajo + "\nTutor/a centro de trabajo:"+ _viewModel.usuarioActual.Usuario.TutorTrabajo);
            tablaTitulo2.AddHeaderCell("Alumno/a: "+ _viewModel.usuarioActual.Usuario.NombreCompleto);
            tablaTitulo2.AddHeaderCell("Ciclo formativo: "+ _viewModel.usuarioActual.Usuario.CicloFormativo + "\t\t Grado:"+ _viewModel.usuarioActual.Usuario.Grado);
            document.Add(tablaTitulo2);
    }
    /// <summary> Método de la clase MostrarUsuario</summary>
    /// <remarks> Nos añade un footer al documento </remarks>
    /// <param name="document">El documento donde se trabaja<param>
    /// <returns> Devuelve un task</returns>
    private async Task FooterAsync(Document document) {
        document.Add(new Paragraph(""));
        iText.Layout.Element.Image image = new iText.Layout.Element.Image(
            ImageDataFactory.Create(await conversorAsync(Constante.Constante.FOOTER)))
            .SetHorizontalAlignment((iText.Layout.Properties.HorizontalAlignment?)HorizontalAlignment.Center).SetWidth(500);
        document.Add(image);
    }
    /// <summary> Método de la clase MostrarUsuario</summary>
    /// <remarks> Genera un arry de byte desde una imagen de firebase </remarks>
    /// <param name="url">La url donde esta la imagen de firebase</param>
    /// <returns> Devuelve un array de bytes</returns>
    private async Task<byte[]> conversorAsync(string url) {
        byte[] imageData;
        using (HttpClient client = new HttpClient()) {
            imageData = await client.GetByteArrayAsync(url);
        }
        // Guardar la imagen descargada en un archivo local temporal
        string rutaImagenLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "imagen_temporal.jpg");
        File.WriteAllBytes(rutaImagenLocal, imageData);
        return imageData;
    }
    /// <summary> Método de la clase MostrarUsuario</summary>
    /// <remarks> Nos contruye el formulario  </remarks>
    /// <param name="semana">La semana que se trabaja</param>
    /// <param name="document">El docuemento de que se trabaja</param>
    private void ContruirSemana(Document document, Semana semana) {
        Table table = new Table(4);
        table.SetWidth(470);
        table.SetMarginTop(10);
        table.SetMarginLeft(10);
        table.SetTextAlignment((iText.Layout.Properties.TextAlignment?)TextAlignment.Center);
        table.AddHeaderCell(CreateCell("Dia", true));
        table.AddHeaderCell(CreateCell("Actividad desarrollada/Puesto formativo", true));
        table.AddHeaderCell(CreateCell("Tiempo empleado", true));
        table.AddHeaderCell(CreateCell("Observaciones", true));
        for (int i = 0; i < semana.Dias.Count; i++) {
            for (int j=0;j< semana.Dias[i].Jornadas.Count; j++) {
                table.AddCell(CreateCell(semana.Dias[i].DiaActual.ToString(), false));
                table.AddCell(CreateCell(semana.Dias[i].Jornadas[j].ActividadDesarrollada, false));
                table.AddCell(CreateCell(semana.Dias[i].Jornadas[j].TiempoEmpleado.ToString(), false));
                table.AddCell(CreateCell(semana.Dias[i].Jornadas[j].Observaciones, false));
            }
        }
        document.Add(table);
    }
    /// <summary> Botón asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Crea de un formulario la semana del pdf </remarks>
    /// <param name="semana">Una semana</param>
    /// <returns> true:Si la semana tiene jornadas false:No tiene jornadas</returns>
    private bool JornadaSemana(Semana semana) {
        bool salida = false;
        for (int i = 0; i < semana.Dias.Count&& !salida; i++) {
            for (int j = 0; j < semana.Dias[i].Jornadas.Count&& !salida; j++) {
                salida = true;
            }
        }
        return salida;
    }
    /// <summary> Método asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Nos crea una celda </remarks>
    /// <param name="isHeader">Si es un encabezado o no</param>
    /// <param name="text">Que texto contiene</param>
    /// <returns> Te devuelve una celda</returns>
    private iText.Layout.Element.Cell CreateCell(string text, bool isHeader) {
        iText.Layout.Element.Cell cell = new iText.Layout.Element.Cell();
        cell.Add(new Paragraph(text));
        
        // Establecer el color de fondo de las celdas de encabezado
        if (isHeader) {
            cell.SetBackgroundColor(ColorConstants.GRAY);
            cell.SetFontColor(iText.Kernel.Colors.DeviceRgb.WHITE);
            cell.SetPadding(0);
            cell.SetFontSize(10);
        } else {
            cell.SetBackgroundColor(ColorConstants.WHITE);
            cell.SetFontSize(12);
            cell.SetFontColor(iText.Kernel.Colors.DeviceRgb.BLACK);
        }
        cell.SetPadding(10);
        return cell;
    }
    /// <summary> Método asyncrono de la clase MostrarUsuario</summary>
    /// <remarks> Nos crea una celda, pero sin bordes</remarks>
    /// <param name="content">Texto para la celdaparam>
    /// <returns> Te devuelve una celda</returns>
    private static iText.Layout.Element.Cell createCellNoBordes2(string content) {
        iText.Layout.Element.Cell cell = new iText.Layout.Element.Cell();
        cell.Add(new Paragraph(content));
        cell.SetFontSize(8);
        cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);
        cell.SetBorder(iText.Layout.Borders.Border.NO_BORDER); // Quitar el borde de la celda
        return cell;
    }
}