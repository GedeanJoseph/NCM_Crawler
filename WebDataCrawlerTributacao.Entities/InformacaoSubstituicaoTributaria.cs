using System;
using System.Collections.Generic;

namespace WebDataCrawlerTributacao.Entities
{
    public class InformacaoSubstituicaoTributaria
    {
        #region "Propriedades"              
        private string baseLegalSubstituicaoTributaria;
        private decimal pctIVAST_Original;
        private decimal pctIVAST_Ajustado_12_percent;
        private decimal pctIVAST_Ajustado_4_percent;
        private decimal pctAliquota;
        private string baseLegalIVAST;
        private string baseLegalAliquota;
        private List<ConveniosProtocolosSignatarios> listaConveniosProtocolos;
        private List<IpiNcmAliquota> listaIpiNcmAliquita;
        private List<String> listaBeneficiosFiscais;
        private List<String> listaBeneficiosFiscaisLegislacaoRelacionada;
        private List<String> listaObservacoes;
        private List<String> listaObservacoesLegiscaoRelacionada;
        #endregion

        #region "Campos"
        public string BaseLegalSubstituicaoTributaria
        {
            get { return baseLegalSubstituicaoTributaria; }
            set { baseLegalSubstituicaoTributaria = value; }
        }
        
        public decimal PctIVAST_Original
        {
            get { return pctIVAST_Original; }
            set { pctIVAST_Original = value; }
        }
        
        public decimal PctIVAST_Ajustado_12_percent
        {
            get { return pctIVAST_Ajustado_12_percent; }
            set { pctIVAST_Ajustado_12_percent = value; }
        }
        
        public decimal PctIVAST_Ajustado_4_percent
        {
            get { return pctIVAST_Ajustado_4_percent; }
            set { pctIVAST_Ajustado_4_percent = value; }
        }

        public decimal PctAliquota
        {
            get { return pctAliquota; }
            set { pctAliquota = value; }
        }
        
        public string BaseLegalIVAST
        {
            get { return baseLegalIVAST; }
            set { baseLegalIVAST = value; }
        }

        public string BaseLegalAliquota
        {
            get { return baseLegalAliquota; }
            set { baseLegalAliquota = value; }
        }     

        public List<ConveniosProtocolosSignatarios> ListaConveniosProtocolos
        {
            get { return listaConveniosProtocolos; }
            set { listaConveniosProtocolos = value; }
        }

        public List<IpiNcmAliquota> ListaIpiNcmAliquita
        {
            get { return listaIpiNcmAliquita; }
            set { listaIpiNcmAliquita = value; }
        }

        public List<String> ListaBeneficiosFiscais
        {
            get { return listaBeneficiosFiscais; }
            set { listaBeneficiosFiscais = value; }
        }
        
        public List<String> ListaBeneficiosFiscaisLegislacaoRelacionada
        {
            get { return listaBeneficiosFiscaisLegislacaoRelacionada; }
            set { listaBeneficiosFiscaisLegislacaoRelacionada = value; }
        }

        public List<String> ListaObservacoes
        {
            get { return listaObservacoes; }
            set { listaObservacoes = value; }
        }

        public List<String> ListaObservacoesLegiscaoRelacionada
        {
            get { return listaObservacoesLegiscaoRelacionada; }
            set { listaObservacoesLegiscaoRelacionada = value; }
        }
        #endregion

        #region "Construtores"
        public InformacaoSubstituicaoTributaria()
        {
            this.ListaConveniosProtocolos = new List<ConveniosProtocolosSignatarios>();
            this.ListaBeneficiosFiscais = new List<string>();
            this.ListaBeneficiosFiscaisLegislacaoRelacionada = new List<string>();
            this.ListaObservacoes = new List<string>();
            this.ListaObservacoesLegiscaoRelacionada = new List<string>();
            this.ListaIpiNcmAliquita = new List<IpiNcmAliquota>();
        }
        #endregion
    }
}
