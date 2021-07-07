using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebDataCrawlerTributacao.Entities;

namespace WebDataCrawlerTributacao.HtmlParser
{
    public class HtmlParserAliquotaInternaIcms
    {
        #region "Propriedades"
        string HtmlIframeAliquotasInternasIcms{get;set;}
        #endregion

        #region "Construtores"
        /// <summary>
        /// Código fonte da página [tab_icms-interestaduais.htm] que contém o código dos estados sendo Origem x Destino x Alíquita.
        /// </summary>
        /// <param name="_htmlIframeTabelaIcmsInterestaduais"></param>
        public HtmlParserAliquotaInternaIcms(string _htmlIframeAliquotasInternasIcms)
        {
            this.HtmlIframeAliquotasInternasIcms = _htmlIframeAliquotasInternasIcms;
        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Retorna todas as alíquotas internas para um determinado estado.
        /// </summary>
        /// <param name="_estado">Estado que deverá ser consultado</param>
        /// <returns></returns>
        public List<AliquotaInternaIcms> RetornaAliquitasInternasIcms(UfEnum _estado)
        {
            List<AliquotaInternaIcms> listaAliquotasInternasIcms = new List<AliquotaInternaIcms>();
            String[] todasTabelasDaPagina = this.HtmlIframeAliquotasInternasIcms.Split(new String[] {"</table>"}, StringSplitOptions.RemoveEmptyEntries);
            Regex regexTabelasAliquotas = new Regex("(?<=^|>)(?!Alíquota|NCM|&nbsp;Descrição|  )[^><\t\n]+?(?=<|$)", RegexOptions.Multiline);

            for (int i = 2; i < todasTabelasDaPagina.ToList().Count; i++)
            {
                String linhaCorrente = todasTabelasDaPagina[i];
                MatchCollection matchValoresAliquotas = regexTabelasAliquotas.Matches(linhaCorrente);

                if (matchValoresAliquotas.Count > 0)//apenas se houver sucesso na consulta
                {
                    AliquotaInternaIcms novaAliquotaInternaIcms = new AliquotaInternaIcms();
                    novaAliquotaInternaIcms.Aliquota = Convert.ToDecimal(matchValoresAliquotas[0].Value.Replace(" %", ""));
                    novaAliquotaInternaIcms.Ncm = matchValoresAliquotas[1].Value;
                    novaAliquotaInternaIcms.Descricao = matchValoresAliquotas[2].Value;
                    novaAliquotaInternaIcms.Estado = _estado;

                    listaAliquotasInternasIcms.Add(novaAliquotaInternaIcms);
                }
            }
            
            return listaAliquotasInternasIcms;
        }

        #endregion
    }
}

