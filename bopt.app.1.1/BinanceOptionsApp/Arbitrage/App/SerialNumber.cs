using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Arbitrage.App
{
    public static class SerialNumber
    {
        public static string Value;
        private static readonly Regex _rtfRegex = new Regex("\\\\([a-z]{1,32})(-?\\d{1,10})?[ ]?|\\\\'([0-9a-f]{2})|\\\\([^a-z])|([{}])|[\\r\\n]+|(.)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly List<string> destinations = new List<string>()
    {
      "aftncn",
      "aftnsep",
      "aftnsepc",
      "annotation",
      "atnauthor",
      "atndate",
      "atnicn",
      "atnid",
      "atnparent",
      "atnref",
      "atntime",
      "atrfend",
      "atrfstart",
      "author",
      "background",
      "bkmkend",
      "bkmkstart",
      "blipuid",
      "buptim",
      "category",
      "colorschememapping",
      "colortbl",
      "comment",
      "company",
      "creatim",
      "datafield",
      "datastore",
      "defchp",
      "defpap",
      "do",
      "doccomm",
      "docvar",
      "dptxbxtext",
      "ebcend",
      "ebcstart",
      "factoidname",
      "falt",
      "fchars",
      "ffdeftext",
      "ffentrymcr",
      "ffexitmcr",
      "ffformat",
      "ffhelptext",
      "ffl",
      "ffname",
      "ffstattext",
      "field",
      "file",
      "filetbl",
      "fldinst",
      "fldrslt",
      "fldtype",
      "fname",
      "fontemb",
      "fontfile",
      "fonttbl",
      "footer",
      "footerf",
      "footerl",
      "footerr",
      "footnote",
      "formfield",
      "ftncn",
      "ftnsep",
      "ftnsepc",
      "g",
      "generator",
      "gridtbl",
      "header",
      "headerf",
      "headerl",
      "headerr",
      "hl",
      "hlfr",
      "hlinkbase",
      "hlloc",
      "hlsrc",
      "hsv",
      "htmltag",
      "info",
      "keycode",
      "keywords",
      "latentstyles",
      "lchars",
      "levelnumbers",
      "leveltext",
      "lfolevel",
      "linkval",
      "list",
      "listlevel",
      "listname",
      "listoverride",
      "listoverridetable",
      "listpicture",
      "liststylename",
      "listtable",
      "listtext",
      "lsdlockedexcept",
      "macc",
      "maccPr",
      "mailmerge",
      "maln",
      "malnScr",
      "manager",
      "margPr",
      "mbar",
      "mbarPr",
      "mbaseJc",
      "mbegChr",
      "mborderBox",
      "mborderBoxPr",
      "mbox",
      "mboxPr",
      "mchr",
      "mcount",
      "mctrlPr",
      "md",
      "mdeg",
      "mdegHide",
      "mden",
      "mdiff",
      "mdPr",
      "me",
      "mendChr",
      "meqArr",
      "meqArrPr",
      "mf",
      "mfName",
      "mfPr",
      "mfunc",
      "mfuncPr",
      "mgroupChr",
      "mgroupChrPr",
      "mgrow",
      "mhideBot",
      "mhideLeft",
      "mhideRight",
      "mhideTop",
      "mhtmltag",
      "mlim",
      "mlimloc",
      "mlimlow",
      "mlimlowPr",
      "mlimupp",
      "mlimuppPr",
      "mm",
      "mmaddfieldname",
      "mmath",
      "mmathPict",
      "mmathPr",
      "mmaxdist",
      "mmc",
      "mmcJc",
      "mmconnectstr",
      "mmconnectstrdata",
      "mmcPr",
      "mmcs",
      "mmdatasource",
      "mmheadersource",
      "mmmailsubject",
      "mmodso",
      "mmodsofilter",
      "mmodsofldmpdata",
      "mmodsomappedname",
      "mmodsoname",
      "mmodsorecipdata",
      "mmodsosort",
      "mmodsosrc",
      "mmodsotable",
      "mmodsoudl",
      "mmodsoudldata",
      "mmodsouniquetag",
      "mmPr",
      "mmquery",
      "mmr",
      "mnary",
      "mnaryPr",
      "mnoBreak",
      "mnum",
      "mobjDist",
      "moMath",
      "moMathPara",
      "moMathParaPr",
      "mopEmu",
      "mphant",
      "mphantPr",
      "mplcHide",
      "mpos",
      "mr",
      "mrad",
      "mradPr",
      "mrPr",
      "msepChr",
      "mshow",
      "mshp",
      "msPre",
      "msPrePr",
      "msSub",
      "msSubPr",
      "msSubSup",
      "msSubSupPr",
      "msSup",
      "msSupPr",
      "mstrikeBLTR",
      "mstrikeH",
      "mstrikeTLBR",
      "mstrikeV",
      "msub",
      "msubHide",
      "msup",
      "msupHide",
      "mtransp",
      "mtype",
      "mvertJc",
      "mvfmf",
      "mvfml",
      "mvtof",
      "mvtol",
      "mzeroAsc",
      "mzeroDesc",
      "mzeroWid",
      "nesttableprops",
      "nextfile",
      "nonesttables",
      "objalias",
      "objclass",
      "objdata",
      "object",
      "objname",
      "objsect",
      "objtime",
      "oldcprops",
      "oldpprops",
      "oldsprops",
      "oldtprops",
      "oleclsid",
      "operator",
      "panose",
      "password",
      "passwordhash",
      "pgp",
      "pgptbl",
      "picprop",
      "pict",
      "pn",
      "pnseclvl",
      "pntext",
      "pntxta",
      "pntxtb",
      "printim",
      "private",
      "propname",
      "protend",
      "protstart",
      "protusertbl",
      "pxe",
      "result",
      "revtbl",
      "revtim",
      "rsidtbl",
      "rxe",
      "shp",
      "shpgrp",
      "shpinst",
      "shppict",
      "shprslt",
      "shptxt",
      "sn",
      "sp",
      "staticval",
      "stylesheet",
      "subject",
      "sv",
      "svb",
      "tc",
      "template",
      "themedata",
      "title",
      "txe",
      "ud",
      "upr",
      "userprops",
      "wgrffmtfilter",
      "windowcaption",
      "writereservation",
      "writereservhash",
      "xe",
      "xform",
      "xmlattrname",
      "xmlattrvalue",
      "xmlclose",
      "xmlname",
      "xmlnstbl",
      "xmlopen"
    };
        private static readonly Dictionary<string, string> specialCharacters = new Dictionary<string, string>()
    {
      {
        "par",
        "\n"
      },
      {
        "sect",
        "\n\n"
      },
      {
        "page",
        "\n\n"
      },
      {
        "line",
        "\n"
      },
      {
        "tab",
        "\t"
      },
      {
        "emdash",
        "—"
      },
      {
        "endash",
        "–"
      },
      {
        "emspace",
        " "
      },
      {
        "enspace",
        " "
      },
      {
        "qmspace",
        " "
      },
      {
        "bullet",
        "•"
      },
      {
        "lquote",
        "‘"
      },
      {
        "rquote",
        "’"
      },
      {
        "ldblquote",
        "“"
      },
      {
        "rdblquote",
        "”"
      }
    };

        static SerialNumber()
        {
            SerialNumber.Value = "";
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            var obj = executingAssembly.GetName();
            string name = obj.Name;
            using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name + ".Agreement.rtf")) // WesternpipsPrivate7.Agreement.rtf
            {
                byte[] numArray = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(numArray, 0, numArray.Length);
                string str = SerialNumber.StripRichTextFormat(Encoding.ASCII.GetString(numArray));
                int startIndex = str.IndexOf('#') + 1;
                int num = str.IndexOf('#', startIndex);
                SerialNumber.Value = str.Substring(startIndex, num - startIndex);
            }
        }

        private static string StripRichTextFormat(string inputRtf)
        {
            if (inputRtf == null)
                return (string)null;
            Stack<SerialNumber.StackEntry> stackEntryStack = new Stack<SerialNumber.StackEntry>();
            bool ignorable = false;
            int numberOfCharactersToSkip = 1;
            int num = 0;
            List<string> stringList = new List<string>();
            Match match = SerialNumber._rtfRegex.Match(inputRtf);
            if (!match.Success)
                return inputRtf;
            for (; match.Success; match = match.NextMatch())
            {
                string key = match.Groups[1].Value;
                string s1 = match.Groups[2].Value;
                string s2 = match.Groups[3].Value;
                string str1 = match.Groups[4].Value;
                string str2 = match.Groups[5].Value;
                string str3 = match.Groups[6].Value;
                if (!string.IsNullOrEmpty(str2))
                {
                    num = 0;
                    if (str2 == "{")
                        stackEntryStack.Push(new SerialNumber.StackEntry(numberOfCharactersToSkip, ignorable));
                    else if (str2 == "}")
                    {
                        SerialNumber.StackEntry stackEntry = stackEntryStack.Pop();
                        numberOfCharactersToSkip = stackEntry.NumberOfCharactersToSkip;
                        ignorable = stackEntry.Ignorable;
                    }
                }
                else if (!string.IsNullOrEmpty(str1))
                {
                    num = 0;
                    if (str1 == "~")
                    {
                        if (!ignorable)
                            stringList.Add(" ");
                    }
                    else if ("{}\\".Contains(str1))
                    {
                        if (!ignorable)
                            stringList.Add(str1);
                    }
                    else if (str1 == "*")
                        ignorable = true;
                }
                else if (!string.IsNullOrEmpty(key))
                {
                    num = 0;
                    if (SerialNumber.destinations.Contains(key))
                        ignorable = true;
                    else if (!ignorable)
                    {
                        if (SerialNumber.specialCharacters.ContainsKey(key))
                            stringList.Add(SerialNumber.specialCharacters[key]);
                        else if (key == "uc")
                            numberOfCharactersToSkip = int.Parse(s1);
                        else if (key == "u")
                        {
                            int utf32 = int.Parse(s1);
                            if (utf32 < 0)
                                utf32 += 65536;
                            stringList.Add(char.ConvertFromUtf32(utf32));
                            num = numberOfCharactersToSkip;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(s2))
                {
                    if (num > 0)
                        --num;
                    else if (!ignorable)
                    {
                        int utf32 = int.Parse(s2, NumberStyles.HexNumber);
                        stringList.Add(char.ConvertFromUtf32(utf32));
                    }
                }
                else if (!string.IsNullOrEmpty(str3))
                {
                    if (num > 0)
                        --num;
                    else if (!ignorable)
                        stringList.Add(str3);
                }
            }
            return string.Join(string.Empty, stringList.ToArray());
        }

        private class StackEntry
        {
            public int NumberOfCharactersToSkip { get; set; }

            public bool Ignorable { get; set; }

            public StackEntry(int numberOfCharactersToSkip, bool ignorable)
            {
                this.NumberOfCharactersToSkip = numberOfCharactersToSkip;
                this.Ignorable = ignorable;
            }
        }
    }
}
