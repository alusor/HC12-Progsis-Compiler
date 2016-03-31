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
        //bool a = false;
        public MainWindow()
        {
            InitializeComponent();
            tabop = new List<Tabop>();
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

        string verificarCodop(string codop, string operando) {
            string temp= "";
            string aux = "";
            bool found =false;
            for (int i = 0; i < tabop.Count; i++) {
                if (codop.ToUpper() == tabop[i].Codop) {
                    if(!found)
                        if (tabop[i].tieneOperando)
                        {
                            if (operando == null)
                            {
                                aux = ("El CODOP DEBE TENER OPERANDO\n");
                            }
                            else {
                            
                               found = analizarOperando(operando,tabop[i].mDireccionamiento);
                                if (found)
                                {
                                    switch (tabop[i].mDireccionamiento)
                                    {
                                        case "IMM":
                                            aux = "Inmediato " + tabop[i].sumaTotalBytes.ToString() + " bytes \n";
                                            break;
                                        case "DIR":
                                            aux = "Directo " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        case "EXT":
                                            aux = "Extendido " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        case "REL":
                                            if (tabop[i].sumaTotalBytes <= 3)
                                                aux = "Relativo 8 bits,  " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            else
                                                aux = "Relativo 16 bits,  " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        case "IDX":
                                            aux = "Indizado de 5 bits " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        case "IDX1":
                                            aux = "Indizado de 9 bits " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        case "IDX2":
                                            aux = "Indizado de 16 bits " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        case "[D,IDX]":
                                            aux = "Indi zado Indirecto de Acumulador “D” " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        case "[IDX2]":
                                            aux = "Indizado Indirecto de 16 bits " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else aux = "Operando incorrecto\n";
                            }


                        }
                        else {
                            if (tabop[i].mDireccionamiento == "INH" && operando == null)
                            {
                                aux = "Inherente " + tabop[i].sumaTotalBytes.ToString() + " bytes\n";
                            }else 
                            {
                                aux =("EL CODOP NO DEBE TENER OPERANDO\n" );
                            }
                        }
                    temp += tabop[i].Codop+ " " + tabop[i].tieneOperando+ " " + tabop[i].mDireccionamiento+ " "+tabop[i].codigoMaquina+"" + tabop[i].totalBytestCalculado+ " " + tabop[i].totalBytesPorCalcular+" "+ tabop[i].sumaTotalBytes+"\n";
                    
                }
                    

            }
            if (temp == "") {
                return "No se encuentra en el TABOP\n";
            }
            return temp + aux ;
        }
        bool verificarXYPCS(String x)
        {
            if (x.Contains("-"))
            {
                x = x.Replace("-", string.Empty);
            }
            else if(x.Contains("+")) {
                x = x.Replace("+",string.Empty);
            }
            if (x.Contains("]"))
                x = x.Replace("]",string.Empty);
           // Console.WriteLine(x);
            if (x.ToUpper()=="X"|| x.ToUpper() == "Y"|| x.ToUpper() == "PC"|| x.ToUpper() == "SP")
                return true;


            return false;
        }
        bool analizarOperando(string operando, string modo) {
            bool temp = false;
            long op;
            string xp = "";


            if (operando.Contains(",")&&modo!="IMM")
            {
                string x = operando.Split(',')[1];
                xp = operando.Split(',')[0];
                switch (modo) {
                    case "IDX":
                        if (xp == "")
                        {
                            return true && verificarXYPCS(x);
                        }
                        else {
                            Console.WriteLine(xp);
                            if(xp[0]!='['&& xp[0] != 'A' && xp[0] != 'B' && xp[0] != 'D' && xp[0] != 'a' && xp[0] != 'b' && xp[0] != 'd')
                                op = int.Parse(xp);
                            
                            return verificarXYPCS(operando.Split(',')[1]);
                        }

                       // Console.WriteLine(xp);
                        //Console.WriteLine(operando.Split(',')[0]  + "    " + operando);
                        break;
                    case "IDX1":
                        break;
                    case "IDX2":
                        break;
                    case "[IDX2]":
                        if (operando[0] == '[')
                            return true;
                        break;
                    case "[D,IDX]":
                        break;
                }
                /*if (operando[0] == '[') {
                    if (modo[1] == 'D') {
                        if (modo == "[D,IDX]") {
                            Console.WriteLine(modo + " " + operando);
                            return true;
                        }
                    }
                    Console.WriteLine("Indirecto");   
                    temp = true;
                }
                else if(modo!="IMM"&&modo!="DIR"&&modo!="EXT"){
                    if ((operando.Split(',')[0][0] >= '0' && operando.Split(',')[0][0] <= '9')|| operando.Split(',')[0][0] == '-') {
                        op = long.Parse(operando.Split(',')[0]);
                        xp = operando.Split(',')[1];
                        Console.WriteLine(modo);
                        temp = true;
                    }
                    //Console.WriteLine("op: " + operando.Split(',')[0]);
                    
                }
                */
               // Console.WriteLine(xp);
               // Console.WriteLine("El operando tiene coma.");
            }
            else {

                
                if (operando[0] == '#')
                {
                    //Console.WriteLine("OPERANDO DE MODO IMM");
                    temp = true;
                }
                else {
                    if ((operando[0] == '$' || operando[0] == '%' || operando[0] == '@' || (operando[0] >= 48 && operando[0] <= 57)) && modo != "IMM")
                    {
                        int x = 0;
                        switch (operando[0])
                        {

                            case '%':
                                operando = operando.Substring(1, operando.Length - 1);
                                x = BinarioEntero(operando);
                                //Console.WriteLine("El numero binario " + x );

                                break;
                            case '@':
                                operando = operando.Substring(1, operando.Length - 1);
                                x = OctalEntero(operando);
                                //Console.WriteLine("El numero ocal es: " + x);
                                break;
                            case '$':
                                operando = operando.Substring(1, operando.Length - 1);
                                x = HexaEntero(operando);
                                //Console.WriteLine("El numero hexa es: " + x);
                                break;
                            default:
                                x = Convert.ToInt32(operando);

                                break;

                        }
                       // Console.WriteLine(x);
                        if (x <= 255 && modo == "DIR")
                        {
                            temp = true;
                           // Console.WriteLine("OPERANDO MODO DIRECTO");
                        }
                        else if (x >= 256 && x <= 65535 && modo == "EXT")
                        {
                            temp = true;
                          //  Console.WriteLine("OPERANDO MODO EXTENDIDO");
                        }


                    }//Recordar hacer validaciones de la etiqueta.
                    else if (modo == "REL")
                    {
                        temp = true;

                    }
                    else if (modo == "EXT") {
                        temp = true;
                    }
                }
            }
            //Console.WriteLine(operando + " " + modo);
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
        int BinarioEntero(string bin)
        {
            return Convert.ToInt32(bin, 2);
        }
        int OctalEntero(string octa) {

            return Convert.ToInt32(octa, 8);
        }
        int HexaEntero(string hexa) {
            return Convert.ToInt32(hexa,16);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.lineas = new List<Linea>();
            
            ana = new analizador();
            salida.Text = "";
            Linea aux;
            String[] lineas = source.Text.Split('\n');
            
            foreach (string linea in lineas) {
                if (linea.Length > 0) {
                    aux = ana.analizar(linea);
                    this.lineas.Add(aux);
                }
                ana = new analizador();
            }       
            foreach (Linea i in this.lineas) {
                if (i.codop != null) {
                    if (i.codop.ToUpper() != "END") {
                        if (i.comentario != null)
                        {
                            salida.Text += ("Comentario" + '\n');
                        }
                        else{
                            if (i.etiqueta != null)
                            {
                                if (i.etiqueta != "error")
                                    if (i.etiqueta != "error1")
                                        salida.Text += ("Etiqueta:" + i.etiqueta + '\n');
                                    else {
                                        salida.Text += ("Error: Formato de etiqueta no valido." + '\n');
                                        //break;
                                    }
                                else {
                                    salida.Text += ("Error: Excedido el tamaño de la etiqueta." + '\n');
                                    //break;
                                }

                            }
                            else
                            {
                                salida.Text += ("Etiqueta: null" + '\n');
                            }
                            if (i.codop != null)
                            {
                                if (i.codop == "error" || i.codop == "error1")
                                {
                                    salida.Text += ("Error: Formato de CODOP no valido." + '\n');
                                    //break;
                                }
                                else {
                                    if (i.codop == "error3")
                                        salida.Text += "Error: No se encuentra en el TABOP\n";
                                    else {
                                        salida.Text += ("CODOP: " + i.codop + '\n');
                                        //Console.WriteLine("1");
                                    }
                                    
                                   string a = verificarCodop(i.codop, i.operando);
                                    if (a != "")
                                    {
                                        salida.Text += a;
                                    }
                                }

                            }
                            else {
                                salida.Text += ("CODOP: null" + '\n');
                            }
                            if (i.operando != null)
                            {
                                salida.Text += ("Operando: " + i.operando + '\n');
                            }
                            else {
                                salida.Text += ("Operando: null" + '\n');
                            }
                            salida.Text += ('\n');
                        }

                        
                    }
                }
                
            }

            if (this.lineas.Last().codop.ToUpper() != "END") {
                salida.Text += "No se encontro End";
            }
            control.SelectedIndex = 1;

        }
    }

}
