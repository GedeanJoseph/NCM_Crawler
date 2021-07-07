using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDataCrawlerTributacao.Entities
{
    public class IpiNcmAliquota
    {
        #region "Propriedades"      
        private string ncm;
        private string descricao;
        private decimal aliquota;
        #endregion
        
        #region "Campos"
        public string Ncm
        {
            get { return ncm; }
            set { ncm = value; }
        }
        
        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }
        
        public decimal Aliquota
        {
            get { return aliquota; }
            set { aliquota = value; }
        }

        #endregion
    }
}
