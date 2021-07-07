using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WebDataCrawlerTributacao.Entities;
using WebDataCrawlerTributacao.BL;
using WebDataCrawlerTributacao.HtmlParser;

namespace WebDataCrawlerTributacao.BL
{
    public class AliquotasInternasIcmsBL
    {
        /// <summary>
        /// Retorna as Alíquotas internas de ICMS por estado
        /// </summary>
        /// <param name="_estado"></param>
        /// <returns></returns>
        public List<AliquotaInternaIcms> RetornaAliquotasInternasIcms(UfEnum _estado)
        {
            List<AliquotaInternaIcms> listaAloquotasPorEstado = new List<AliquotaInternaIcms>();                       
            String htmlDaPagina = "";

            using (StreamReader reader = new StreamReader(@"C:\Regex\AliquotasEsteaduaisInternas\..   ECONET Editora   ..2.htm", Encoding.GetEncoding("iso-8859-1")))
            {
                htmlDaPagina = reader.ReadToEnd();
            }

            HtmlParserAliquotaInternaIcms parserAliquotas = new HtmlParserAliquotaInternaIcms(htmlDaPagina);
            listaAloquotasPorEstado = parserAliquotas.RetornaAliquitasInternasIcms(_estado);

            return listaAloquotasPorEstado;
        }
    }
}

