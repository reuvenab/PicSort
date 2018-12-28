using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PicSort
{
    public class Filename2Datetime
    {
        #region // 20131109_183003

        private static Regex _mp4_YYYYmmDD_hhMMss = new Regex(@"(\d{4})(\d{2})(\d{2})_(\d{2})(\d{2})(\d{2})", 
                     RegexOptions.Compiled | RegexOptions.IgnoreCase);

         static DateTime _mp4_YYYYmmDD_hhMMss_Match(Match m)
        {
            try
            {
                return new DateTime( 
                    Int32.Parse(m.Groups[1].Value), 
                    Int32.Parse(m.Groups[2].Value), 
                    Int32.Parse(m.Groups[3].Value),
                    Int32.Parse(m.Groups[4].Value), 
                    Int32.Parse(m.Groups[5].Value), 
                    Int32.Parse(m.Groups[6].Value)
                    );    
            }
            catch (System.Exception)
            {
                return DateTime.MinValue;
            }
            
        }    

        #endregion


        #region // WIN_20180701_17_00_05_Pro.jpg
        private static Regex _WIN_YYYYmmDD = new Regex(@"(\d{4})(\d{2})(\d{2})_(\d{2})_(\d{2})_(\d{2})", 
                     RegexOptions.Compiled | RegexOptions.IgnoreCase);

        static DateTime _WIN_YYYYmmDD_Match(Match m)
        {
            try
            {
                return new DateTime( 
                    Int32.Parse(m.Groups[1].Value), 
                    Int32.Parse(m.Groups[2].Value), 
                    Int32.Parse(m.Groups[3].Value),
                    Int32.Parse(m.Groups[4].Value), 
                    Int32.Parse(m.Groups[5].Value), 
                    Int32.Parse(m.Groups[6].Value)
                    );    
            }
            catch (System.Exception)
            {
                return DateTime.MinValue;
            }
            
        }

        #endregion

        #region // 2017:01:17 18:34:50
            
        private static Regex _exifFormat2 = new Regex(@"(\d{4}):(\d{2}):(\d{2}) (\d{2}):(\d{2}):(\d{2})", 
                     RegexOptions.Compiled | RegexOptions.IgnoreCase);

        static DateTime _exifFormat2_Match(Match m)
        {
            try
            {
                return new DateTime( 
                    Int32.Parse(m.Groups[1].Value), 
                    Int32.Parse(m.Groups[2].Value), 
                    Int32.Parse(m.Groups[3].Value),
                    Int32.Parse(m.Groups[4].Value), 
                    Int32.Parse(m.Groups[5].Value), 
                    Int32.Parse(m.Groups[6].Value)
                    );    
            }
            catch (System.Exception)
            {
                return DateTime.MinValue;
            }
            
        }

        #endregion

        
        #region  10/31/2016 12:07
   
        private static Regex _exifFormat1 = new Regex(@"(\d{1,2})/(\d{2})/(\d{4}) (\d{2}):(\d{2})", 
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);


        static DateTime _exifFormat1_Match(Match m)
        {
            try
            {
                return new DateTime( 
                    Int32.Parse(m.Groups[3].Value), 
                    Int32.Parse(m.Groups[1].Value), 
                    Int32.Parse(m.Groups[2].Value),
                    Int32.Parse(m.Groups[4].Value),
                    Int32.Parse(m.Groups[5].Value),
                    0
                    );    
            }
            catch (System.Exception)
            {
                return DateTime.MinValue;
            }
            
        }

        #endregion

        #region // 2018-11-04_12-08-37  
                          
        private static Regex _YYYYmmDD_HHmmSS = new Regex(@"(\d{4})-(\d{2})-(\d{2})_(\d{2})-(\d{2})-(\d{2})", 
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);


        static DateTime _YYYYmmDD_HHmmSS_Match(Match m)
        {
            try
            {
                return new DateTime( 
                    Int32.Parse(m.Groups[1].Value), 
                    Int32.Parse(m.Groups[2].Value), 
                    Int32.Parse(m.Groups[3].Value),
                    Int32.Parse(m.Groups[4].Value), 
                    Int32.Parse(m.Groups[5].Value), 
                    Int32.Parse(m.Groups[6].Value)
                    );    
            }
            catch (System.Exception)
            {
                return DateTime.MinValue;
            }
            
        }

        #endregion

        
        #region  // IMG-20140406-WA0040

        private static Regex _IMG_YYYYmmDD = new Regex(@"IMG-(\d{4})(\d{2})(\d{2})-", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);


        static DateTime _IMG_YYYYmmDD_Match(Match m)
        {
            try
            {
                return new DateTime( 
                    Int32.Parse(m.Groups[1].Value), 
                    Int32.Parse(m.Groups[2].Value), 
                    Int32.Parse(m.Groups[3].Value)
                    );    
            }
            catch (System.Exception)
            {
                return DateTime.MinValue;
            }
            
        }
        
        
        #endregion
        
        delegate DateTime MatchToDateTime(Match m);

        class RegexMatcher : Tuple<Regex, MatchToDateTime>
        {
            public RegexMatcher(Regex regex, MatchToDateTime func) : base(regex, func)
            {
            }
        }

        private static List< RegexMatcher > _matchers = new List<RegexMatcher>() {
            new RegexMatcher(_mp4_YYYYmmDD_hhMMss, _mp4_YYYYmmDD_hhMMss_Match),
            new RegexMatcher(_WIN_YYYYmmDD, _WIN_YYYYmmDD_Match),
            new RegexMatcher(_YYYYmmDD_HHmmSS, _YYYYmmDD_HHmmSS_Match),
            new RegexMatcher(_exifFormat1, _exifFormat1_Match),
            new RegexMatcher(_exifFormat2, _exifFormat2_Match),
            new RegexMatcher(_IMG_YYYYmmDD, _IMG_YYYYmmDD_Match),
        };

        public enum RegexType
        {
            All,
            Exif
        }
        public static DateTime? MakeDatetime(string dateTimeString, RegexType regexType = RegexType.All)
        {

            List< RegexMatcher > matchers = null;
            switch (regexType)
            {
                case RegexType.Exif:
                    matchers = new List<RegexMatcher>() {
                        new RegexMatcher(_exifFormat1, _exifFormat1_Match),
                        new RegexMatcher(_exifFormat2, _exifFormat2_Match),
                    };
                    break;

                default:
                    matchers = _matchers;
                    break;
            }

            foreach (var regexMatcher in matchers)
            {
                var m = regexMatcher.Item1.Match(dateTimeString);
                if (m.Success)
                    return regexMatcher.Item2(m);
            }
            
            return null;
        }


    }
}