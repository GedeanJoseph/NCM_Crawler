using System;
using System.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDataCrawlerTributacao.Entities
{
    public class AliquotaInterestaduaisOrigemDestino
    {
        #region "Propriedades"                
        private UfEnum estadoOrigem;
        private UfEnum estadoDestino;
        private decimal percentAliquota;
        #endregion

        #region "Campos"
                
        public UfEnum EstadoOrigem
        {
            get { return estadoOrigem; }
            set { estadoOrigem = value; }
        }

        public UfEnum EstadoDestino
        {
            get { return estadoDestino; }
            set { estadoDestino = value; }
        }
        
        public decimal PercentAliquota
        {
            get { return percentAliquota; }
            set { percentAliquota = value; }
        }
        #endregion
    }
}
