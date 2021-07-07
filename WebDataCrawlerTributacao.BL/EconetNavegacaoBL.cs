using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using WebDataCrawlerTributacao.Entities;
using WebDataCrawlerTributacao.HtmlParser;

namespace WebDataCrawlerTributacao.BL
{
    public class EconetNavegacaoBL
    {
        #region "Construtores"
        /// <summary>
        /// Construtor que verifica se a Classe Econet já está conectada e executa conexão com usuário e senha padrão do sistema;
        /// </summary>
        public EconetNavegacaoBL()
        {
            if (EconetLoginBL.LogadoSucesso == false)
            {
                EconetLoginBL.LoginEconetEditora();   
            }
        }

        //Construtor que verifica se a Classe Econet já está conectada e executa conexão com usuário e senha fornecidos;
        public EconetNavegacaoBL(String _codUsuario, String _senha)
        {
            if (EconetLoginBL.LogadoSucesso == false)
            {
                EconetLoginBL.LoginEconetEditora(_codUsuario,_senha);
            }
        }

        #endregion

        #region "Métodos"

        /// <summary>
        /// irá retornar todos os itens da tela "Lista de Mercadorias Sujeitas à Substituição Tributária";
        /// </summary>
        /// <param name="_ncm"></param>
        /// <param name="_estado"></param>
        /// <returns></returns>
        public List<MercadoriaSujeitaSubstituicaoTributaria> VerificaListaMercadoriaSubstituicaoTributaria(String _ncm, UfEnum _estado)
        {
            List<MercadoriaSujeitaSubstituicaoTributaria> listaMercadoriasSubstituicaoTributaria = new List<MercadoriaSujeitaSubstituicaoTributaria>();

            #region "Valida o NCM"                    
            Regex ncmPattern = new Regex("[0-9]{4}\\.[0-9]{2}\\.[0-9]{2}(\\.?[0-9]{2})?");//Valida o código NCM.            
            if (!ncmPattern.IsMatch(_ncm))
            {
                return null;
            }
            #endregion

            #region "Cria o GET da página de mercadorias"
            String htmlPaginaMercadorias = "";
            MemoryStream memStream = new MemoryStream();
            FormUtility formTools = new FormUtility(memStream,null);
            formTools.Add("form[uf]", _estado.ToString());
            formTools.Add("form[ncm]", _ncm.Substring(0, 4));
            formTools.Add("form[palavra]", "");
            formTools.Add("acao", "Buscar");
            formTools.Complete();
            
            ASCIIEncoding encoder = new ASCIIEncoding();
            String urlQueryString = encoder.GetString(memStream.GetBuffer());
            String urlParametrizada = "http://www2.econeteditora.com.br/icms_st/index.php?" + urlQueryString;
            Uri urlEconetPaginaListaMercadorias = new Uri(urlParametrizada);
            HttpWebRequest requestPagina = (HttpWebRequest)HttpWebRequest.Create(urlEconetPaginaListaMercadorias);
            requestPagina.CookieContainer = EconetLoginBL.CookiesLogin;//Antes do Get do response, carrega o container de cookies do login
            HttpWebResponse responsePagina = (HttpWebResponse)requestPagina.GetResponse();
            #endregion                      

            #region "Recupera as mercadorias da páginas através do HtmlParser"

            using (StreamReader streamReader = new StreamReader(responsePagina.GetResponseStream(), Encoding.GetEncoding("iso-8859-1")))
            {
                htmlPaginaMercadorias = streamReader.ReadToEnd();
            }

            Regex regexValidaMercadoriasEncontradas = new Regex("Localizados? [0-9]+? itens?");
            if (regexValidaMercadoriasEncontradas.IsMatch(htmlPaginaMercadorias))
            {            
                //Recupera a lista de mercadorias contida na página através do parser.
                HtmlParserMercadoriasSubstTributaria parserMercadorias = new HtmlParserMercadoriasSubstTributaria(htmlPaginaMercadorias);
                listaMercadoriasSubstituicaoTributaria = parserMercadorias.RetornaListaMercadoriasPagina();
            }
            else
            {
                listaMercadoriasSubstituicaoTributaria = null;
            }
            #endregion

            return listaMercadoriasSubstituicaoTributaria;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public InformacaoSubstituicaoTributaria RetornaInformacoesSubstituicaoTributaria(UfEnum _formUf, int _formId)
        {
            InformacaoSubstituicaoTributaria informacoesTributarias = new InformacaoSubstituicaoTributaria();
            String htmlPaginaInformacoesTributacao = "";

            #region "Cria o GET da página de informações de tributação"
            MemoryStream memStream = new MemoryStream();
            FormUtility form = new FormUtility(memStream, null);
            form.Add("form[uf]", _formUf.ToString());
            form.Add("form[id]", _formId.ToString());
            form.Add("acao", "Abrir");
            form.Complete();

            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            String urlQueryString = asciiEncoding.GetString(memStream.GetBuffer());
            String urlParametrizada = "http://www2.econeteditora.com.br/icms_st/index.php?" + urlQueryString;
            Uri uriInformacoesTributacao = new Uri(urlParametrizada);
            HttpWebRequest requestInformacoesTributacao = (HttpWebRequest)HttpWebRequest.Create(uriInformacoesTributacao);
            requestInformacoesTributacao.CookieContainer = EconetLoginBL.CookiesLogin;
            HttpWebResponse responseInformacoesTributacao = (HttpWebResponse)requestInformacoesTributacao.GetResponse();
            #endregion

            #region "Recupera as informações de tributacao através do HtmlParser"

            using (StreamReader reader = new StreamReader(responseInformacoesTributacao.GetResponseStream(), Encoding.GetEncoding("iso-8859-1")))
            {
                htmlPaginaInformacoesTributacao = reader.ReadToEnd();
            }

            HtmlParserSubstituicaoTributaria parserInfSubstituicaoTributaria = new HtmlParserSubstituicaoTributaria(htmlPaginaInformacoesTributacao);
            informacoesTributarias = parserInfSubstituicaoTributaria.RetornaInformacoesSubstituicaoTributaria();

            #endregion

            return informacoesTributarias;

        }


        #endregion  
    }
}
