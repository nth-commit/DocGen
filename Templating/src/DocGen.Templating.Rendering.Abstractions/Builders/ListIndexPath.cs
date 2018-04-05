using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Rendering.Builders
{
    public class ListIndexPath : IEnumerable<int>
    {
        private readonly IEnumerable<int> _path;

        public ListIndexPath(IEnumerable<int> path)
        {
            if (path.Count() > 5)
            {
                throw new ArgumentOutOfRangeException($"Max list nesting level is 5");
            }

            if (path.Count() == 0)
            {
                throw new ArgumentOutOfRangeException("Cannot have empty path");
            }

            _path = path;
        }

        public string Format()
        {
            string result = null;

            var pathCount = _path.Count();
            if (pathCount == 1 || pathCount == 2)
            {
                result = GetDotSeparatedLabel(_path);
            }
            else if (pathCount == 3)
            {
                result = GetAlphaLabel(_path.Skip(2).First());
            }
            else if (pathCount == 4)
            {
                result = GetRomanLabel(_path.Skip(3).First());
            }
            else if (pathCount == 5)
            {
                result = GetDotSeparatedLabel(_path.Skip(4));
            }

            return result + ".";
        }

        private string GetDotSeparatedLabel(IEnumerable<int> path) => string.Join(".", path.Select(i => i + 1));

        private string GetAlphaLabel(int index) => Encoding.ASCII.GetString(new byte[] { (byte)(97 + (index % 26)) });

        private string GetRomanLabel(int index) => Roman.To(index + 1).ToLower();

        public IEnumerator<int> GetEnumerator() => _path.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static class Roman
        {
            public static readonly Dictionary<char, int> RomanNumberDictionary;
            public static readonly Dictionary<int, string> NumberRomanDictionary;

            static Roman()
            {
                RomanNumberDictionary = new Dictionary<char, int>
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 },
        };

                NumberRomanDictionary = new Dictionary<int, string>
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" },
        };
            }

            public static string To(int number)
            {
                var roman = new StringBuilder();

                foreach (var item in NumberRomanDictionary)
                {
                    while (number >= item.Key)
                    {
                        roman.Append(item.Value);
                        number -= item.Key;
                    }
                }

                return roman.ToString();
            }

            public static int From(string roman)
            {
                int total = 0;

                int current, previous = 0;
                char currentRoman, previousRoman = '\0';

                for (int i = 0; i < roman.Length; i++)
                {
                    currentRoman = roman[i];

                    previous = previousRoman != '\0' ? RomanNumberDictionary[previousRoman] : '\0';
                    current = RomanNumberDictionary[currentRoman];

                    if (previous != 0 && current > previous)
                    {
                        total = total - (2 * previous) + current;
                    }
                    else
                    {
                        total += current;
                    }

                    previousRoman = currentRoman;
                }

                return total;
            }
        }
    }
}
