using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HC12_Progsis_Compiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Tabop> tabop;
        List<Linea> lineas;
        analizador ana;
        bool a = false;
        public MainWindow()
        {
            InitializeComponent();
            lineas = new List<Linea>();
            tabop = new List<Tabop>();
            ana = new analizador();
            cargarTabop();  
        }
        public void cargarTabop() {
            System.IO.StreamReader tabop = new System.IO.StreamReader("TABOP.txt");
            String[] temp = tabop.ReadToEnd().Split('\n');
            foreach (String Linea in temp) {
                
                String[] aux = Linea.Split('|');
                Tabop tmp = new Tabop();
                tmp.Codop = aux[0];
                if (aux[1] != "NO")
                {
                    tmp.tieneOperando = true;
                }
                else tmp.tieneOperando = false;
                tmp.mDireccionamiento = aux[2];
                tmp.codigoMaquina = aux[3];
                tmp.totalBytestCalculado = int.Parse(aux[4]);
                tmp.totalBytesPorCalcular = int.Parse(aux[5]);
                tmp.sumaTotalBytes = int.Parse(aux[6]);
                this.tabop.Add(tmp); 
            }
        }

        string verificarCodop(string codop) {
            string temp= "";
            for (int i = 0; i < tabop.Count; i++) {
                if (codop.ToUpper() == tabop[i].Codop) {
                    temp += tabop[i].Codop+ " " + tabop[i].tieneOperando+ " " + tabop[i].mDireccionamiento+ " " + tabop[i].totalBytestCalculado+ " " + tabop[i].totalBytesPorCalcular+" "+ tabop[i].sumaTotalBytes+"\n";
                    
                }
                    

            }
            return temp;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) {
                source.Text = "";
                string filename = dlg.FileName;
                name.Text = filename;
                {
                    string temp;
                    System.IO.StreamReader archivo = new System.IO.StreamReader(filename);
                    while ((temp = archivo.ReadLine()) != null) {
                        source.Text += temp + "\n";
                    }
                    //source.Text = archivo.ReadToEnd();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Linea aux;
            String[] lineas = source.Text.Split('\n');
            salida.Text = "";
            foreach (string linea in lineas) {
                if (linea.Length > 0) {
                    aux = ana.analizar(linea);
                    this.lineas.Add(aux);
                    if (linea.Contains("END") || linea.Contains("end") || linea.Contains("enD") || linea.Contains("EnD") || linea.Contains("ENd") || linea.Contains("eNd")){
                        a = true;
                    }
                }
                ana = new analizador();
            }       
            foreach (Linea i in this.lineas) {
                if (i.comentario != null) {
                    salida.Text +=("Comentario"+'\n');
                }
                if (i.etiqueta != null) {
                    if (i.etiqueta != "error")
                        if (i.etiqueta != "error1")
                            salida.Text +=("Etiqueta:" + i.etiqueta + '\n');
                        else {
                            salida.Text +=("Error: Formato de etiqueta no valido." + '\n');
                            //break;
                        }
                    else {      
                        salida.Text +=("Error: Excedido el tamaño de la etiqueta." + '\n');
                        //break;
                    }

                }
                else
                {
                    salida.Text +=("Etiqueta: null" + '\n');
                }
                if (i.codop != null)
                {
                    if (i.codop == "error" || i.codop == "error1")
                    {
                        salida.Text += ("Error: Formato de CODOP no valido." + '\n');
                        //break;
                    }
                    else {
                        salida.Text += ("CODOP: " + i.codop + '\n');
                        string a = verificarCodop(i.codop);
                        if (a != "") {
                            salida.Text += a;
                        }
                    }

                }
                else {
                    salida.Text +=("CODOP: null" + '\n');
                }
                if (i.operando != null)
                {
                    salida.Text +=("Operando: " + i.operando + '\n');
                }
                else {
                    salida.Text +=("Operando: null" + '\n');
                }
                salida.Text +=('\n');
            }
            if ((this.lineas.Last().codop != "end")&& (this.lineas.Last().codop != "END") && (this.lineas.Last().codop != "End") && (this.lineas.Last().codop != "ENd") && (this.lineas.Last().codop != "eND") ) {
                salida.Text += "No se encontro End";
            }

        }
    }
}
