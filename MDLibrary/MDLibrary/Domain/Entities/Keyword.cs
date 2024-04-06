using System.Collections.Generic;

namespace MDLibrary.Domain.Entities
{
    public class Keyword
    {
        public int KeywordId { get; set; }
        public string Value { get; set; }
        public ICollection<Literature> Literature { get; set; } = [];

		public override string ToString()
		{
			return Value;
		}
	}
}
