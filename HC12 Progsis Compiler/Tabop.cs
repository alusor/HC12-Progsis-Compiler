using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC12_Progsis_Compiler
{
    class Tabop
    {
        public String Codop { get; set; }
        public bool tieneOperando { get; set; }
        public String mDireccionamiento { get; set; }
        public String codigoMaquina { get; set; }
        public int totalBytestCalculado { get; set; }
        public int totalBytesPorCalcular { get; set; }
        public int sumaTotalBytes { get; set; }

        public bool revisarCodop(string a) {
            return a.Equals(Codop);
        }
    }
}
