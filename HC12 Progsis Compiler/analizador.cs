using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace HC12_Progsis_Compiler
{
    class analizador
    {
        Linea linea;
        Regex rex;
        List<Tabop> tabop;
        
        public analizador() {
            linea = new Linea();
            tabop = new List<Tabop>();
            cargarTabop();       
            
        }


        public void cargarTabop()
        {
            System.IO.StreamReader tabop = new System.IO.StreamReader("TABOP.txt");
            String[] temp = tabop.ReadToEnd().Split('\n');
            foreach (String Linea in temp)
            {

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




        private void comentario(string coment) {
            string[] temp = coment.Split(';');
            //Console.WriteLine(temp[1]);
            for (int i = 1; i < temp.Length; i++) {
                linea.comentario += temp[i];
            }
           // Console.WriteLine(linea.comentario);
        }

        public int analizarEtiqueta(string cadena) {

            if (cadena.Length > 8)
                return -1;
            else {
                rex = new Regex(@"^[A-Z,a-z][A-Z,a-z,0-9,_]*");
                if (!rex.IsMatch(cadena))
                    return 1;
            }

            return 0;
        }
        private string etiqueta(string eti) {
            string aux = eti.Split()[0];
            int a = analizarEtiqueta(aux);
            
            switch (a) {
                case 0:
                    linea.etiqueta = aux;
                    break;
                case -1:
                    linea.etiqueta = "error";
                    break;
                case 1:
                    linea.etiqueta = "error1";
                    break;

            }
            return aux;

        }
        public int analizarCodop(string cop) {

            if (cop.Length > 5)
                return -1;
            rex = new Regex(@"^[A-Z,a-z][A-Z,a-z,\.]");
            if (!rex.IsMatch(cop))
                return 1;
            else {
                int numPuntos = cop.ToArray().Where(ir => ir.Equals('.')).Count();
                if (numPuntos > 1)
                {
                    return 1;
                }
            }
            foreach (Tabop l in tabop) {
                if (l.Codop == cop.ToUpper())
                    return 0;
            }
            return 2;
        }
        private string codop(string cod) {
            string aux = cod.Trim().Split()[0];
            int a = analizarCodop(aux);

            switch (a) {
                case -1: linea.codop = "error";
                    break;
                case 0: linea.codop = aux;
                    break;
                case 1: linea.codop = "error1";
                    break;
                case 2:
                    if (!(cod.ToUpper() == "END"))
                        linea.codop = "error3";
                    else
                        linea.codop = aux;
                    break;
            }
            return aux;
        }

        public int analizarOperando(string op) {

            return 0;
        }
        private void operando(string ope) {
            linea.operando = ope;
        }
        public Linea analizar(string lienaCompleta)
        {
            string aux;
            if (lienaCompleta.Contains(';'))
            {
                comentario(lienaCompleta);
                lienaCompleta = lienaCompleta.Split(';')[0];
            }
           if(lienaCompleta.Length>0)
                if (lienaCompleta[0] != '\t' && lienaCompleta[0] != ' ')
                {
                    lienaCompleta = lienaCompleta.Replace( etiqueta(lienaCompleta), string.Empty);
                    if (lienaCompleta.Length > 0) {
                        lienaCompleta = lienaCompleta.Trim();
                        aux = codop(lienaCompleta);
                        if (lienaCompleta == aux)
                        {
                            //Console.WriteLine("No tiene operando");
                        }
                        else {
                            lienaCompleta = lienaCompleta.Replace(aux,string.Empty);
                            lienaCompleta = lienaCompleta.Trim();
                            operando(lienaCompleta);
                        }
                    }
                }
                else {
                    aux = codop(lienaCompleta);
                    lienaCompleta = lienaCompleta.Trim();
                    if (lienaCompleta.Length > 0) {
                        lienaCompleta = lienaCompleta.Replace(aux, string.Empty);
                        lienaCompleta = lienaCompleta.Trim();
                        if (lienaCompleta.Length > 0)
                        {
                            operando(lienaCompleta);
                            //Console.WriteLine(linea.operando);
                        }
                    }
                }
            return linea;
        }
      
    }
}
