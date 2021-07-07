using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDataCrawlerTributacao.Entities
{
    public class AliquotaInternaIcms
    {
        #region "Propriedades"              
        private UfEnum estado;
        private decimal aliquota;
        private string ncm;
        private string descricao;
        #endregion

        #region "Campos"
        
        public UfEnum Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        
        public decimal Aliquota
        {
            get { return aliquota; }
            set { aliquota = value; }
        }
        
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
        #endregion
    }
}
