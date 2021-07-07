using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDataCrawlerTributacao.Entities
{
    /// <summary>
    /// Entidade que representa os ítens na página "Lista de Mercadorias Sujeitas à Substituição Tributária"
    /// </summary>
    public class MercadoriaSujeitaSubstituicaoTributaria
    {
        #region "Propriedades"
        private UfEnum estado;
        private String ncm;
        private String descricaoMercadoria;
        private int formIdMercadoria;
        #endregion

        #region "Campos"        
        public UfEnum Estado
        {
            get { return estado; }
            set { estado = value; }
        }        
        public String Ncm
        {
            get { return ncm; }
            set { ncm = value; }
        }
        public String DescricaoMercadoria
        {
            get { return descricaoMercadoria; }
            set { descricaoMercadoria = value; }
        }
        public int FormIdMercadoria
        {
            get { return formIdMercadoria; }
            set { formIdMercadoria = value; }
        }
        #endregion
    }
}
