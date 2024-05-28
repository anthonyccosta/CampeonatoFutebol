using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampeonatoFutebol
{
    internal class Conexao
    {
         string conectando = "";

        public Conexao()
        {
            conectando = "Data Source=localhost; Initial Catalog=CampeonatoFutebol, User Id=sa; Password=SqlServer2019!"; 
        }

        public string ObterCaminho()
        {
            return conectando;
        }
    }
}
