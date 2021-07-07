using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using WebDataCrawlerTributacao.Entities;
using WebDataCrawlerTributacao.BL;
using HeatonResearch.Spider.HTML;


namespace Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SubstituicaoTributariaBL substituicaoTributaria = new SubstituicaoTributariaBL();
            InformacaoSubstituicaoTributaria infSubstituicaoTributaria = new InformacaoSubstituicaoTributaria();
            
            infSubstituicaoTributaria = substituicaoTributaria.RetornaSubstituicaoTributaria("Sp", "3305");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AliquotasInterestaduaisBL aliquotaInterestadual = new AliquotasInterestaduaisBL();
            aliquotaInterestadual.RetornaAliquotasInterestaduais();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AliquotasInternasIcmsBL aliquotasInternas = new AliquotasInternasIcmsBL();

            aliquotasInternas.RetornaAliquotasInternasIcms(UfEnum.RR);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ExtractSubPage parse = new ExtractSubPage();
            parse.testeMain();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormPOST formPost = new FormPOST();
            formPost.testeMain();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CookieContainer cookieLogado = EconetLoginBL.LogadoSucesso ? EconetLoginBL.CookiesLogin : EconetLoginBL.LoginEconetEditora();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormGET formGet = new FormGET();
            formGet.testeMain();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            EconetNavegacaoBL navegacaoEconet = new EconetNavegacaoBL();
            navegacaoEconet.VerificaListaMercadoriaSubstituicaoTributaria("3305.10.00", UfEnum.SP);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Uri uriEconet = new Uri("http://www2.econeteditora.com.br/user/ver_log.asp");
            //WebRequest request = HttpWebRequest.Create(uriEconet);
            //HttpWebResponse responseEconet = (HttpWebResponse)request.GetResponse();
            //Stream novoStream = responseEconet.GetResponseStream();
            //String HtmlPagina = new StreamReader(novoStream).ReadToEnd();

            SubstituicaoTributariaBL subs = new SubstituicaoTributariaBL();
            List<MercadoriaSujeitaSubstituicaoTributaria> novaLista = new List<MercadoriaSujeitaSubstituicaoTributaria>();
            novaLista = subs.retornaListaMercadoriasSubstituicao();


        }

    }

    #region Método que extrai conteúdo de uma subpágina
    public class ExtractSubPage
    {

        /// <summary>
        /// This method downloads the specified URL into a C#
        /// String. This is a very simple method, that you can
        /// reused anytime you need to quickly grab all data from
        /// a specific URL.
        /// </summary>
        /// <param name="url">The URL to download.</param>
        /// <returns>The contents of the URL that was downloaded.</returns>
        public String DownloadPage(Uri url)
        {
            WebRequest http = HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.ASCII);

            String result = stream.ReadToEnd();

            //response.Close();
            stream.Close();
            return result;
        }

        /// <summary>
        /// This method is very useful for grabbing information from a
        /// HTML page.  It extracts text from between two tokens, the
        /// tokens need not be case sensitive.
        /// </summary>
        /// <param name="str">The string to extract from.</param>
        /// <param name="token1">The text, or tag, that comes before the desired text</param>
        /// <param name="token2">The text, or tag, that comes after the desired text</param>
        /// <param name="count">Which occurrence of token1 to use, 1 for the first</param>
        /// <returns></returns>
        public String ExtractNoCase(String str, String token1, String token2,
            int count)
        {
            int location1, location2;

            // convert everything to lower case
            String searchStr = str.ToLower();
            token1 = token1.ToLower();
            token2 = token2.ToLower();

            // now search
            location1 = location2 = 0;
            do
            {
                location1 = searchStr.IndexOf(token1, location1 + 1);

                if (location1 == -1)
                    return null;

                count--;
            } while (count > 0);

            // return the result from the original string that has mixed
            // case
            location1 += token1.Length;
            location2 = str.IndexOf(token2, location1 + 1);
            if (location2 == -1)
                return null;

            return str.Substring(location1, location2 - location1);
        }

        /// <summary>
        /// Process each subpage. The subpages are where the data actually is.
        /// </summary>
        /// <param name="u">The URL of the subpage.</param>
        private void ProcessSubPage(Uri u)
        {
            String str = DownloadPage(u);
            String code = ExtractNoCase(str, "Code:<b></td><td>", "</td>", 0);
            if (code != null)
            {
                String capital = ExtractNoCase(str, "Capital:<b></td><td>", "</td>", 0);
                String name = ExtractNoCase(str, "<h1>", "</h1>", 0);
                String flag = ExtractNoCase(str, "<img src=\"", "\" border=\"1\">", 2);
                String site = ExtractNoCase(str, "Official Site:<b></td><td><a href=\"",
                    "\"", 0);

                Uri flagURL = new Uri(u, flag);

                StringBuilder buffer = new StringBuilder();
                buffer.Append("\"");
                buffer.Append(code);
                buffer.Append("\",\"");
                buffer.Append(name);
                buffer.Append("\",\"");
                buffer.Append(capital);
                buffer.Append("\",\"");
                buffer.Append(flagURL.ToString());
                buffer.Append("\",\"");
                buffer.Append(site);
                buffer.Append("\"");
                Console.WriteLine(buffer.ToString());
            }
        }

        /// <summary>
        /// Process the specified URL and extract data from all of the subpages
        /// that this page links to.
        /// </summary>
        /// <param name="url">The URL to process.</param>
        public void Process(Uri url)
        {
            String value = "";
            WebRequest http = HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            Stream istream = response.GetResponseStream();
            ParseHTML parse = new ParseHTML(istream);

            int ch;
            while ((ch = parse.Read()) != -1)
            {
                if (ch == 0)
                {
                    HTMLTag tag = parse.Tag;
                    if (String.Compare(tag.Name, "a", true) == 0)
                    {
                        value = tag["href"];
                        Uri u = new Uri(url, value.ToString());
                        value = u.ToString();
                        ProcessSubPage(u);
                    }
                }
            }
        }

        public void testeMain()
        {
            //Uri u = new Uri("http://www.httprecipes.com/1/6/subpage.php");
            Uri url = new Uri("http://www.httprecipes.com/1/7/post.php");
            ExtractSubPage parse = new ExtractSubPage();
            parse.Process(url);
        }
    }

    #endregion
    
    #region Método que executa o post
     class FormPOST
    {

        /// <summary>
        /// Advance to the specified HTML tag.
        /// </summary>
        /// <param name="parse">The HTML parse object to use.</param>
        /// <param name="tag">The HTML tag.</param>
        /// <param name="count">How many tags like this to find.</param>
        /// <returns>True if found, false otherwise.</returns>
        private bool Advance(ParseHTML parse, String tag, int count)
        {
            int ch;
            while ((ch = parse.Read()) != -1)
            {
                if (ch == 0)
                {
                    if (String.Compare(parse.Tag.Name, tag, true) == 0)
                    {
                        count--;
                        if (count <= 0)
                            return true;
                    }
                }
            }
            return false;
        }

        /*
  * Handle each list item, as it is found.
  */
        private void ProcessItem(String item)
        {
            Console.WriteLine(item.Trim());
        }

        /**
         * Access the website and perform a search for either states or capitals.
         * @param search A search string.
         * @param type What to search for(s=state, c=capital)
         * @throws IOException Thrown if an IO exception occurs.
         */
        public void Process(String search, String type)
        {
            String listType = "ul";
            String listTypeEnd = "/ul";
            StringBuilder buffer = new StringBuilder();
            bool capture = false;

            // Build the URL and POST.
            Uri url = new Uri("http://www.httprecipes.com/1/7/post.php");
            WebRequest http = HttpWebRequest.Create(url);
            http.Timeout = 30000;//retirar um dos zeros que foi add por exemplo
            http.ContentType = "application/x-www-form-urlencoded";
            http.Method = "POST";
            Stream ostream = http.GetRequestStream();

            FormUtility form = new FormUtility(ostream, null);
            form.Add("search", search);
            form.Add("type", type);
            form.Add("action", "Search");
            form.Complete();
            ostream.Close();

            // read the results
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            Stream istream = response.GetResponseStream();
            

            ParseHTML parse = new ParseHTML(istream);

            // parse from the URL

            Advance(parse, listType, 0);

            int ch;
            while ((ch = parse.Read()) != -1)
            {
                if (ch == 0)
                {
                    HTMLTag tag = parse.Tag;
                    if (String.Compare(tag.Name, "li", true) == 0)
                    {
                        if (buffer.Length > 0)
                            ProcessItem(buffer.ToString());
                        buffer.Length = 0;
                        capture = true;
                    }
                    else if (String.Compare(tag.Name, "/li", true) == 0)
                    {
                        ProcessItem(buffer.ToString());
                        buffer.Length = 0;
                        capture = false;
                    }
                    else if (String.Compare(tag.Name, listTypeEnd, true) == 0)
                    {
                        ProcessItem(buffer.ToString());
                        break;
                    }
                }
                else
                {
                    if (capture)
                        buffer.Append((char)ch);
                }
            }
        }

        public void testeMain()
        {
            FormPOST parse = new FormPOST();
            parse.Process("Mi", "s");
        }
    }
    #endregion

     #region Método que executa o get da página
     class FormGET
     {
         /// <summary>
         /// Advance to the specified HTML tag.
         /// </summary>
         /// <param name="parse">The HTML parse object to use.</param>
         /// <param name="tag">The HTML tag.</param>
         /// <param name="count">How many tags like this to find.</param>
         /// <returns>True if found, false otherwise.</returns>
         private bool Advance(ParseHTML parse, String tag, int count)
         {
             int ch;
             while ((ch = parse.Read()) != -1)
             {
                 if (ch == 0)
                 {
                     if (String.Compare(parse.Tag.Name, tag, true) == 0)
                     {
                         count--;
                         if (count <= 0)
                             return true;
                     }
                 }
             }
             return false;
         }

         /// <summary>
         /// Handle each list item, as it is found.
         /// </summary>
         /// <param name="item">The item to be processed.</param>
         private void ProcessItem(String item)
         {
             Console.WriteLine(item.Trim());
         }

         /// <summary>
         /// Access the website and perform a search for either states or capitals.
         /// </summary>
         /// <param name="search">A search string.</param>
         /// <param name="type">What to search for(s=state, c=capital)</param>
         public void Process(String search, String type)
         {
             String listType = "ul";
             String listTypeEnd = "/ul";
             StringBuilder buffer = new StringBuilder();
             bool capture = false;

             // Build the URL.
             MemoryStream mstream = new MemoryStream();
             FormUtility form = new FormUtility(mstream, null);
             form.Add("search", search);
             form.Add("type", type);
             form.Add("action", "Search");
             form.Complete();

             System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

             String str = enc.GetString(mstream.GetBuffer());
             String surl = "http://www.httprecipes.com/1/7/get.php?" + str;
             Uri url = new Uri(surl);
             WebRequest http = HttpWebRequest.Create(url);
             HttpWebResponse response = (HttpWebResponse)http.GetResponse();
             Stream istream = response.GetResponseStream();
             ParseHTML parse = new ParseHTML(istream);

             // Parse from the URL.

             Advance(parse, listType, 0);

             int ch;
             while ((ch = parse.Read()) != -1)
             {
                 if (ch == 0)
                 {
                     HTMLTag tag = parse.Tag;
                     if (String.Compare(tag.Name, "li", true) == 0)
                     {
                         if (buffer.Length > 0)
                             ProcessItem(buffer.ToString());
                         buffer.Length = 0;
                         capture = true;
                     }
                     else if (String.Compare(tag.Name, "/li", true) == 0)
                     {
                         ProcessItem(buffer.ToString());
                         buffer.Length = 0;
                         capture = false;
                     }
                     else if (String.Compare(tag.Name, listTypeEnd, true) == 0)
                     {
                         ProcessItem(buffer.ToString());
                         break;
                     }
                 }
                 else
                 {
                     if (capture)
                         buffer.Append((char)ch);
                 }
             }
         }


         public void testeMain()
         {
             FormGET parse = new FormGET();
             parse.Process("Mi", "s");
         }
     }


     #endregion
}

