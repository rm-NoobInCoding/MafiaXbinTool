using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaXbinTool
{
    static public class Help
    {
        public static string ReadNullTerminated(this System.IO.BinaryReader rdr)
        {
            var bldr = new System.Text.StringBuilder();
            int nc;
            while ((nc = rdr.Read()) > 0)
                bldr.Append((char)nc);

            return RemoveNewLine(bldr.ToString());
        }
        public static void WriteStr(this System.IO.MemoryStream ms , string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(AddNewLine(str) + "\0");
            ms.Write(buffer, 0, buffer.Length);
        }
        public static string RemoveNewLine(string str)
        {
            string ret = str;
            if (ret == "") ret = "[EmptyLine]";
            ret = ret.Replace("\r\n", "<cf>");
            ret = ret.Replace("\n", "<lf>");
            ret = ret.Replace("\r", "<cr>");
            return ret;
        }
        public static string AddNewLine(string Text)
        {
            if (Text == "[EmptyLine]") Text = "";
            Text = Text.Replace("<cf>", "\r\n");
            Text = Text.Replace("<lf>", "\n");
            Text = Text.Replace("<cr>", "\r");
            return Text;
        }
    }
}
