using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebDataCrawlerTributacao.Entities;


namespace WebDataCrawlerTributacao.HtmlParser
{
    public class HtmlParserTabelaAliquotasInterestaduaiss
    {
        #region "Propriedades"
        string HtmlIframeTabelaIcmsInterestaduais{get;set;}
        #endregion

        #region "Construtores"
        /// <summary>
        /// Código fonte da página [tab_icms-interestaduais.htm] que contém o código dos estados sendo Origem x Destino x Alíquita.
        /// </summary>
        /// <param name="_htmlIframeTabelaIcmsInterestaduais"></param>
        public HtmlParserTabelaAliquotasInterestaduaiss(string _htmlIframeTabelaIcmsInterestaduais)
        {
            this.HtmlIframeTabelaIcmsInterestaduais = _htmlIframeTabelaIcmsInterestaduais;
        }
        #endregion

        #region "Métodos Públicos"

        public List<AliquotaInterestaduaisOrigemDestino> RetornaAliquotasInterestaduaisOrigemDestino()
        {
            List<AliquotaInterestaduaisOrigemDestino> listaAliquotasInterestaduaisOrigemDestino = new List<AliquotaInterestaduaisOrigemDestino>();
            String[] todasAsTabelasDaPagina = this.HtmlIframeTabelaIcmsInterestaduais.Split(new String[] { "<tbody>", "</tbody>" }, StringSplitOptions.RemoveEmptyEntries);
            String[] todasAsLinhasDaTabelaOriDest = todasAsTabelasDaPagina[3].Split(new String[] { "<tr>"}, StringSplitOptions.RemoveEmptyEntries);
            Regex regexSigla = new Regex("(?<=|^>)[A-Za-z]{2}?(?=<|$)");
            Regex regexPercentuaisAliquota = new Regex("(?<=^|>)(&nbsp;)|[0-9]{2}(?=<|$)");

            
            foreach (string linhaCorrente in todasAsLinhasDaTabelaOriDest.ToList())
            {                
                MatchCollection matchEstadoOrigem = regexSigla.Matches(linhaCorrente);//Recupera a UF do estado de origem através da ER
                
                if (matchEstadoOrigem.Count == 1)//Efetuada essa validação pois na primeira linha(header) aparecerá indevidamente todos os estados de destino
                {
                    MatchCollection matchPercentuaisAliquotaPorDestino = regexPercentuaisAliquota.Matches(linhaCorrente);//recupera todos os valores de percentuais presentes na string da linha corrente
                    int contadorDestino = 0;//zera o contador de destinos a fim de começar de "AC" novamente.
                
                    foreach (Match percentAliquotaCorrente in matchPercentuaisAliquotaPorDestino)
                    {
                        AliquotaInterestaduaisOrigemDestino novoOrigemDestinoAliquota = new AliquotaInterestaduaisOrigemDestino();
                        novoOrigemDestinoAliquota.EstadoOrigem = (UfEnum)Enum.Parse(typeof(UfEnum), matchEstadoOrigem[0].Value);
                        novoOrigemDestinoAliquota.EstadoDestino = (UfEnum)contadorDestino;
                        novoOrigemDestinoAliquota.PercentAliquota = percentAliquotaCorrente.Value != "&nbsp;" ? Convert.ToDecimal(percentAliquotaCorrente.Value) : 0;
                        
                        ////[TO-DO] - Apagar esse trecho usado apenas para validar um ponto de inclusão 
                        //if (novoOrigemDestinoAliquota.EstadoOrigem == UfEnum.TO && novoOrigemDestinoAliquota.EstadoDestino == UfEnum.RO)
                        //{
                        //    string teste = "";
                        //}

                        contadorDestino++;
                        listaAliquotasInterestaduaisOrigemDestino.Add(novoOrigemDestinoAliquota);
                    }
                }
            }    

            return listaAliquotasInterestaduaisOrigemDestino;
        }

        #endregion

        #region "Métodos Privados"

        #endregion 
    }
}
