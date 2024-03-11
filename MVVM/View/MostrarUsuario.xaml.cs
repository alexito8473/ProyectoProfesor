using Firebase.Storage;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using ProyectoProfesor.MVVM.Model;
using ProyectoProfesor.MVVM.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace ProyectoProfesor.MVVM.View;

public partial class MostrarUsuario : ContentPage
{
	private UsuarioViewModel _viewModel;
	public MostrarUsuario(UsuarioViewModel usuario)
	{
		InitializeComponent();
        _viewModel = usuario;
        DisplayAlert("Correcto", "Recopilando los datos de los alumnos", "Vale");
        BindingContext = _viewModel;
	}

    private void Button_Clicked_1(object sender, EventArgs e) {
        Navigation.PushAsync(new InformacionUsuario(_viewModel));
    }
    private void Button_Clicked(object sender, EventArgs e) {
		if (_viewModel.ListaJornadas.Count==0) {
            DisplayAlert("Información", "No tiene jornadas", "ok");
        } else {
            crearPDF2();
            DisplayAlert("Información", "Se ha creado el pdf correctamente", "ok");
        }
    }
    private async void crearPDF2() {
        string fileName = "Registro_" + _viewModel.usuarioActual.NombreCompleto + DateTime.Now.ToString("dd_MM_yyyy") + ".pdf";

#if ANDROID
        await Permissions.RequestAsync<Permissions.StorageWrite>();
    var docsDirectory = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
    var filepath= Path.Combine(docsDirectory.AbsoluteFile.Path,fileName);
#else
        var filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
#endif
        using (PdfWriter writer = new PdfWriter(filepath)) {
            List <Semana> semanas= new List<Semana>();
            Document document = new Document(new PdfDocument(writer));
            for (int i = 0; i < _viewModel.usuarioActual.Años.Count; i++) {
                for (int j = 0; j < _viewModel.usuarioActual.Años[i].Meses.Count; j++) {
                    if (_viewModel.TieneJornadasElMes(_viewModel.usuarioActual.Años[i].Meses[j])) {// Tiene una jornada?
                        semanas = new List<Semana>();
                        semanas = crearListaSemanas( _viewModel.usuarioActual.Años[i].Meses[j]);
                        for (int k = 0; k < semanas.Count; k++) {
                            if (JornadaSemana(semanas[k])) {
                                titular(document, _viewModel.usuarioActual.Años[i].fecha, _viewModel.usuarioActual.Años[i].Meses[j].Nombre, semanas[k]);
                                ContruirSemana(document, semanas[k]);
                                Footer(document);
                                document.Add(new AreaBreak());
                            }
                        }
                    }
                }
            }
            document.Close();
        }
    }

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
    private void titular(Document document, string año, string mes, Semana semana) {
        document.Add(new Paragraph("JUNTA DE ANDALUCIA\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tCONSEJERÍA DE EDUCACIÓN").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(10));
            document.Add(new Paragraph("FORMACION DE CENTRO DE TRABAJO. FICHA SEMANAL DEL ALUMNO/ALUMNA").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(10));
            Table tablaTitulo = new Table(1);
            tablaTitulo.SetWidth(259).SetFontSize(10);
            tablaTitulo.AddCell("Semana del "+ semana.DiaInicio+ " al "+ semana.DiaFin +" de " + semana.Mes + " 2024");
            tablaTitulo.SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER);
            document.Add(tablaTitulo);
            Table tablaTitulo2 = new Table(2);
            tablaTitulo2.SetWidth(500).SetFontSize(10);
            tablaTitulo2.AddHeaderCell("Centro docente:"+_viewModel.usuarioActual.CentroDocente+"\nProfesor/a responsable seguimiento:"+_viewModel.usuarioActual.ProfesorResponsable);
            tablaTitulo2.AddHeaderCell("Centro de trabajo coloborador:"+ _viewModel.usuarioActual.CentroTrabajo + "\nTutor/a centro de trabajo:"+ _viewModel.usuarioActual.TutorTrabajo);
            tablaTitulo2.AddHeaderCell("Alumno/a: "+ _viewModel.usuarioActual.NombreCompleto);
            tablaTitulo2.AddHeaderCell("Ciclo formativo: "+ _viewModel.usuarioActual.CicloFormativo + "\t\t Grado:"+ _viewModel.usuarioActual.Grado);
            document.Add(tablaTitulo2);
    }
    private void Footer(Document document) {
        document.Add(new Paragraph(""));
        Table tablaTitulo = new Table(3);
        tablaTitulo.SetWidth(330);
        tablaTitulo.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        tablaTitulo.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
        tablaTitulo.SetStrokeColor(ColorConstants.WHITE);
        tablaTitulo.SetBackgroundColor(ColorConstants.WHITE);   
        tablaTitulo.AddCell(createCellNoBordes("El/La alumno"));
        tablaTitulo.AddCell(createCellNoBordes("VºB El/A PROFESOR/A RESPONSABLE DEL SEGUIMIENTO"));
        tablaTitulo.AddCell(createCellNoBordes("VºB El/LA TUTOR DEL CENTRO DE TRABAJO"));
        tablaTitulo.AddCell(createCellNoBordes2("Fdo:"));
        tablaTitulo.AddCell(createCellNoBordes2("Fdo:"));
        tablaTitulo.AddCell(createCellNoBordes2("Fdo:"));
        document.Add(tablaTitulo);
    }
    private async Task<byte[]> conversorAsync() {
        byte[] imageData;
        using (HttpClient client = new HttpClient()) {
            imageData = await client.GetByteArrayAsync(_viewModel.usuarioActual.Imagen);
        }
        // Guardar la imagen descargada en un archivo local temporal
        string rutaImagenLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "imagen_temporal.jpg");
        File.WriteAllBytes(rutaImagenLocal, imageData);
        return imageData;
    }

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

    private bool JornadaSemana(Semana semana) {
        bool salida = false;
        for (int i = 0; i < semana.Dias.Count&& !salida; i++) {
            for (int j = 0; j < semana.Dias[i].Jornadas.Count&& !salida; j++) {
                salida = true;
            }
        }
        return salida;
    }
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
    private static iText.Layout.Element.Cell createCellNoBordes(string content) {
        iText.Layout.Element.Cell cell = new iText.Layout.Element.Cell();
        cell.Add(new Paragraph(content));
        cell.SetFontSize(8);
        cell.SetBorder(iText.Layout.Borders.Border.NO_BORDER); // Quitar el borde de la celda
        return cell;
    }
    private static iText.Layout.Element.Cell createCellNoBordes2(string content) {
        iText.Layout.Element.Cell cell = new iText.Layout.Element.Cell();
        cell.Add(new Paragraph(content));
        cell.SetFontSize(8);
        cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);
        cell.SetBorder(iText.Layout.Borders.Border.NO_BORDER); // Quitar el borde de la celda
        return cell;
    }
    private async Task<byte[]>  convertirImagen(string name) {
        using var ms= new MemoryStream();
        using (var steam= await FileSystem.OpenAppPackageFileAsync(name))
            await steam.CopyToAsync(ms);
        return ms.ToArray();
    }


}