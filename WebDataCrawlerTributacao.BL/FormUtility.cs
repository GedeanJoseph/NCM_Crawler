using System;
using System.Text;
using System.Web;
using System.IO;
using System.Net;

public class FormUtility
{
    private static Random random = new Random();
    private ASCIIEncoding encoding = new ASCIIEncoding();
    private string boundary;
    private Stream os;
    private bool first;

    static FormUtility()
    {
    }

    public FormUtility(Stream os, string boundary)
    {
        this.os = os;
        this.boundary = boundary;
        this.first = true;
    }

    public static string getBoundary()
    {
        return "---------------------------" + FormUtility.RandomString() + FormUtility.RandomString() + FormUtility.RandomString();
    }

    private static string Encode(string str)
    {
        return HttpUtility.HtmlEncode(str);
    }

    protected static string RandomString()
    {
        return "" + (object)FormUtility.random.NextDouble();
    }

    public void AddFile(string name, string file)
    {
        this.AddFile(name, file, "application/octet-stream");
    }

    public void AddFile(string name, string file, string type)
    {
        if (this.boundary == null)
            return;
        this.Boundary();
        this.WriteName(name);
        this.Write("; filename=\"");
        this.Write(file);
        this.Write("\"");
        this.Newline();
        this.Write("Content-Type: ");
        this.Writeln(type);
        this.Newline();
        byte[] buffer = new byte[8192];
        Stream stream = (Stream)new FileStream(file, FileMode.Open);
        int count;
        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
            this.os.Write(buffer, 0, count);
        this.Newline();
    }

    public void Add(string name, string value)
    {
        if (this.boundary != null)
        {
            this.Boundary();
            this.WriteName(name);
            this.Newline();
            this.Newline();
            this.Writeln(value);
        }
        else
        {
            if (!this.first)
                this.Write("&");
            this.Write(FormUtility.Encode(name));
            this.Write("=");
            this.Write(FormUtility.Encode(value));
        }
        this.first = false;
    }

    public void Complete()
    {
        if (this.boundary == null)
            return;
        this.Boundary();
        this.Writeln("--");
        this.os.Flush();
    }

    private void Boundary()
    {
        this.Write("--");
        this.Write(this.boundary);
    }

    private void Newline()
    {
        this.Write("\r\n");
    }

    private void Write(string str)
    {
        byte[] bytes = this.encoding.GetBytes(str);
        this.os.Write(bytes, 0, bytes.Length);
    }

    private void WriteName(string name)
    {
        this.Newline();
        this.Write("Content-Disposition: form-data; name=\"");
        this.Write(name);
        this.Write("\"");
    }

    protected void Writeln(string str)
    {
        this.Write(str);
        this.Newline();
    }
}