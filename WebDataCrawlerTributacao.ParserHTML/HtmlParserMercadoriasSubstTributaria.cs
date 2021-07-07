using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebDataCrawlerTributacao.Entities;


namespace WebDataCrawlerTributacao.HtmlParser
{
    public class HtmlParserMercadoriasSubstTributaria
    {
        #region "Propriedades"
        private String HtmlPaginaListaMercadoriasSubstTributaria{get;set;}
        #endregion

        #region "Construtores"
        public HtmlParserMercadoriasSubstTributaria(String _htmlPaginaListaMercadoriasSubstTributaria)
        {
            this.HtmlPaginaListaMercadoriasSubstTributaria = _htmlPaginaListaMercadoriasSubstTributaria;
        }
        #endregion

        #region "Métodos"
        public List<MercadoriaSujeitaSubstituicaoTributaria> RetornaListaMercadoriasPagina()
        {
            List<MercadoriaSujeitaSubstituicaoTributaria> listaMercadoriasPagina = new List<MercadoriaSujeitaSubstituicaoTributaria>();
            String[] todasAsTabelasDaPagina = this.HtmlPaginaListaMercadoriasSubstTributaria.Split(new String[] { "<tbody>", "</tbody>" }, StringSplitOptions.RemoveEmptyEntries);
            String[] todasAsLinhasMercadoriasPagina = todasAsTabelasDaPagina[5].Split(new String[] { " <form action" }, StringSplitOptions.RemoveEmptyEntries);

            Regex regexTagsValores = new Regex("(?<=^|>)(?!NCM|DESCRIÇÃO|MAIS)[^><\t\n\r]+?(?=<|$)");
            Regex regexFormUF = new Regex("(?<=name=\"form.uf.\" value=\")[a-zA-Z]{2}(?=\">|$)", RegexOptions.Multiline);
            Regex regexFormID = new Regex("(?<=name=\"form.id.\" value=\")[0-9]{0,}?(?=\">)",RegexOptions.Multiline);

            foreach (String linhaCorrente in todasAsLinhasMercadoriasPagina)
            {
                MercadoriaSujeitaSubstituicaoTributaria novaMercadoria = new MercadoriaSujeitaSubstituicaoTributaria();
                MatchCollection matchTagsValores = regexTagsValores.Matches(linhaCorrente);
                

                if (matchTagsValores.Count > 0)
                {
                    novaMercadoria.Ncm = matchTagsValores[0].Value;
                    novaMercadoria.DescricaoMercadoria = matchTagsValores[1].Value;
                    Match matchUf = regexFormUF.Match(linhaCorrente);
                    novaMercadoria.Estado = (UfEnum)Enum.Parse(typeof(UfEnum), matchUf.Value);
                    novaMercadoria.FormIdMercadoria = Convert.ToInt32(regexFormID.Match(linhaCorrente).Value);

                    listaMercadoriasPagina.Add(novaMercadoria);                    
                }
                
            }


            return listaMercadoriasPagina;
        }
        #endregion
    }
}
