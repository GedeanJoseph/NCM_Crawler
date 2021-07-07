using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDataCrawlerTributacao.Entities;
using WebDataCrawlerTributacao.HtmlParser;

namespace WebDataCrawlerTributacao.BL
{
    public class SubstituicaoTributariaBL
    {
        public InformacaoSubstituicaoTributaria RetornaSubstituicaoTributaria(string _estado, string _nCM)
        {
            String htmlPagina = "";

            using (StreamReader reader = new StreamReader(@"C:\Regex\Página que deve ser varrida pelo robo_files\index.htm", Encoding.ASCII))
            {
                htmlPagina = reader.ReadToEnd();
            }

            HtmlParserSubstituicaoTributaria Parser = new HtmlParserSubstituicaoTributaria(htmlPagina);

            return Parser.RetornaInformacoesSubstituicaoTributaria();
        }



        public List<MercadoriaSujeitaSubstituicaoTributaria> retornaListaMercadoriasSubstituicao()
        {
            String HtmlPagina = "";
            List<MercadoriaSujeitaSubstituicaoTributaria> listaMercadorias = new List<MercadoriaSujeitaSubstituicaoTributaria>();

            using(StreamReader reader = new StreamReader(@"D:\Gedean_Arquivos\Desenvimento_Programação\WebDataCrawlerTributacao\Regex\PáginaLista_Itens_NCM\..   ECONET Editora   ...htm",Encoding.GetEncoding("iso-8859-1")))
            {                
                HtmlPagina = reader.ReadToEnd();
            }

            HtmlParserMercadoriasSubstTributaria parser = new HtmlParserMercadoriasSubstTributaria(HtmlPagina);

            listaMercadorias = parser.RetornaListaMercadoriasPagina();

            return listaMercadorias;
        }


    }

}
