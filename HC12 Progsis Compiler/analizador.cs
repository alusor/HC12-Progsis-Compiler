using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC12_Progsis_Compiler
{
    class analizador
    {
        Linea linea;
        public analizador() {
            linea = new Linea();            
            
        }
         private void comentario(string coment) {
            string[] temp = coment.Split(';');
            //Console.WriteLine(temp[1]);
            for (int i = 1; i < temp.Length; i++) {
                linea.comentario += temp[i];
            }
           // Console.WriteLine(linea.comentario);
        }
        private string etiqueta(string eti) {
            string aux = eti.Split()[0];
            linea.etiqueta = aux;
            return aux;

        }
        private string codop(string cod) {
            string aux = cod.Trim().Split()[0];
            linea.codop = aux;
            //Console.WriteLine(aux);
            return aux;
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
