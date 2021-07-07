using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDataCrawlerTributacao.Entities
{
    public class ConveniosProtocolosSignatarios
    {
        #region "Propriedades"
        private string conveniosProtocolos;
        private string signatarios;
        #endregion  

        #region "Campos"
        
        public string ConveniosProtocolos
        {
            get { return conveniosProtocolos; }
            set { conveniosProtocolos = value; }
        }
        
        public string Signatarios
        {
            get { return signatarios; }
            set { signatarios = value; }
        }
        #endregion
    }
}
