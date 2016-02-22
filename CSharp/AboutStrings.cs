using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace DotNetKoans.CSharp
{
    public class AboutStrings : Koan
    {
        //Note: This is one of the longest katas and, perhaps, one
        //of the most important. String behavior in .NET is not
        //always what you expect it to be, especially when it comes
        //to concatenation and newlines, and is one of the biggest
        //causes of memory leaks in .NET applications

        [Koan(1)]
        public void DoubleQuotedStringsAreStrings()
        {
            var str = "Hello, World";
            Assert.Equal(typeof(string), str.GetType());
            //"" means string.
        }

        [Koan(2)]
        public void SingleQuotedStringsAreNotStrings()
        {
            var str = 'H';
			Assert.Equal(typeof(char), str.GetType());
            // '' means char
        }

        [Koan(3)]
        public void CreateAStringWhichContainsDoubleQuotes()
        {
            var str = "Hello, \"World\"";
            Assert.Equal(14, str.Length);
            //the \ escapes the very next character, which is how you can use "" within a string without breaking it.
        }

        [Koan(4)]
        public void AnotherWayToCreateAStringWhichContainsDoubleQuotes()
        {
            //The @ symbol creates a 'verbatim string literal'. 
            //Here's one thing you can do with it:
            var str = @"Hello, ""World""";
            Assert.Equal(14, str.Length);
            //this is how you have to do "" inside "" @(""""), because verbatim strings don't give a fuck about \.
        }

        [Koan(5)]
        public void VerbatimStringsCanHandleFlexibleQuoting()
        {
            var strA = @"Verbatim Strings can handle both ' and "" characters (when escaped)";
            //^^ this is a verbatim string. notice that it psuedo-"escapes" the " character with another " (resulting in "")?
            var strB = "Verbatim Strings can handle both ' and \" characters (when escaped)";
            //this is a literal string. notice the \"?
            Assert.Equal(true, strA.Equals(strB));
            //resulting in them both saying the same thing.
        }

        [Koan(6)]
        public void VerbatimStringsCanHandleMultipleLinesToo()
        {
            //Tip: What you create for the literal string will have to 
            //escape the newline characters. For Windows, that would be
            // \r\n. If you are on non-Windows, that would just be \n.
            //We'll show a different way next.
            var verbatimString = @"I
am a
broken line";
            Assert.Equal(20, verbatimString.Length);
            var literalString = "I\r\nam a\r\nbroken line";
            //verbatim strings are really verbatim, which means returns as well. the \r\n is how we make the literal string match up.
            Assert.Equal(literalString, verbatimString);
        }

        [Koan(7)]
        public void ACrossPlatformWayToHandleLineEndings()
        {
            //Since line endings are different on different platforms
            //(\r\n for Windows, \n for Linux) you shouldn't just type in
            //the hardcoded escape sequence. A much better way
            //(We'll handle concatenation and better ways of that in a bit)
            var literalString = "I" + System.Environment.NewLine + "am a" + System.Environment.NewLine + "broken line";
            var vebatimString = @"I" + System.Environment.NewLine + "am a" + System.Environment.NewLine + "broken line";
            //Syste=m.Environment.NewLine (and other methods I suppose) don't give a damn about windows, linux, or mac bullshit.
            Assert.Equal(literalString, vebatimString);
        }

        [Koan(8)]
        public void PlusWillConcatenateTwoStrings()
        {
            var str = "Hello, " + "World";
            Assert.Equal("Hello, World", str);
            // just like javascript
        }

        [Koan(9)]
        public void PlusConcatenationWillNotModifyOriginalStrings()
        {
            var strA = "Hello, ";
            var strB = "World";
            var fullString = strA + strB;
            Assert.Equal("Hello, ", strA);
            Assert.Equal("World", strB);

            //the two strings will still be in memory. use += to minimize memory leaks where need be.
        }

        [Koan(10)]
        public void PlusEqualsWillModifyTheTargetString()
        {
            var strA = "Hello, ";
            var strB = "World";
            strA += strB;
            Assert.Equal("Hello, World", strA);
            Assert.Equal("World", strB);
            //see? originl strings (strB) will still be in memory, but += did modify strA.
        }

        [Koan(11)]
        public void StringsAreReallyImmutable()
        {
            //So here's the thing. Concatenating strings is cool
            //and all. But if you think you are modifying the original
            //string, you'd be wrong. 

            var strA = "Hello, ";
            var originalString = strA;
            var strB = "World";
            strA += strB;
            Assert.Equal("Hello, ", originalString);

            Assert.Equal("Hello, World", strA);
            //but it does modify strA, otherwise this^^ would get an exception.

            //What just happened? Well, the string concatenation actually
            //takes strA and strB and creates a *new* string in memory
            //that has the new value. It does *not* modify the original
            //string. This is a very important point - if you do this kind
            //of string concatenation in a tight loop, you'll use a lot of memory
            //because the original string will hang around in memory until the
            //garbage collector picks it up. Let's look at a better way
            //when dealing with lots of concatenation
        }

		[Koan(12)]
		public void YouDoNotNeedConcatenationToInsertVariablesInAString()
		{
			var world = "World";
			var str = String.Format("Hello, {0}", world);
			Assert.Equal("Hello, World", str);
            //{0} is code for index zero of the variables, parameter two of String.Format is the variable to include.
            //any extra variables would have to correspond with more {numbers}, such as:
            var pumpkin = "pumpkin";
            var newstr  = String.Format("Hello, {0} {1}", world, pumpkin);
            //{0} is world, {1} is pumpkin.
            Assert.Equal("Hello, World pumpkin", newstr);
		}

		[Koan(13)]
		public void AnyExpressionCanBeUsedInFormatString()
		{
			var str = String.Format("The square root of 9 is {0}", Math.Sqrt(9));
			Assert.Equal("The square root of 9 is 3", str);
            //fuck yeah format string
		}

		[Koan(14)]
		public void StringsCanBePaddedToTheLeft()
		{
			//You can modify the value inserted into the result
			var str = string.Format("{0,3:}", "x");
            //the 3 in {0,3:} says where the second parameter will be placed. in this case, string "x"
            //is placed at the third position (note, this position is not zero indexed, but like .length, starting with 1
			Assert.Equal("  x", str);
            //count the spaces
		}

		[Koan(15)]
		public void StringsCanBePaddedToTheRight()
		{
			var str = string.Format("{0,-3:}", "x");
            // the -3 in {0,-3:} is where the second parameter will be placed. like above, this position is NOT zero indexed.
			Assert.Equal("x  ", str);
            //See? count the spaces.
		}

		[Koan(16)]
		public void SeperatorsCanBeAdded()
		{
			var str = string.Format("{0:n}", 123456);
            //I guess the :n is the designator for numbers seperator?
			Assert.Equal("123,456.00", str);
		}

		[Koan(17)]
		public void CurrencyDesignatorsCanBeAdded()
		{

            //or they can not be added?
			var str = string.Format("{0:n}", 123456);
			Assert.Equal("123,456.00", str);
            //isn't this the same as above?
		}

		[Koan(18)]
		public void NumberOfDisplayedDecimalsCanBeControled()
		{
			var str = string.Format("{0:.##}", 12.3456);
			Assert.Equal("12.35", str);
            //the number of # is the number of places., but it rounds. therefore the above works.
		}

		[Koan(19)]
		public void MinimumNumberOfDisplayedDecimalsCanBeControled()
		{
			var str = string.Format("{0:.00}", 12.3);
			Assert.Equal("12.30", str);
            //i guess the zeros are different? or maybe the ##s mean if they're needed, and the 00s mean they are required?
		}

		[Koan(20)]
		public void BuiltInDateFormaters()
		{
			var str = string.Format("{0:t}", DateTime.Parse("12/16/2011 2:35:02 PM"));
			Assert.Equal("2:35 PM", str);
            //:t gets the time and excludes the date.
		}

		[Koan(21)]
		public void CustomeDateFormaters()
		{
			var str = string.Format("{0:t m}", DateTime.Parse("12/16/2011 2:35:02 PM"));
            Assert.Equal("P 35", str);
            //:t just gets the time, but m grabs only the minute. ick why P is there or in front.
            //readup on DateTime.Parse and String.Format
		}
		//These are just a few of the formatters available. Dig some and you may find what you need.

        [Koan(22)]
        public void ABetterWayToConcatenateLotsOfStrings()
        {
            //Concatenating lots of strings is a Bad Idea(tm). If you need to do that, then consider StringBuilder.
            var strBuilder = new System.Text.StringBuilder();
			strBuilder.Append("The ");
			strBuilder.Append("quick ");
			strBuilder.Append("brown ");
			strBuilder.Append("fox ");
			strBuilder.Append("jumped ");
			strBuilder.Append("over ");
			strBuilder.Append("the ");
			strBuilder.Append("lazy ");
			strBuilder.Append("dog.");
            var str = strBuilder.ToString();
            Assert.Equal("The quick brown fox jumped over the lazy dog.", str);

            //String.Format and StringBuilder will be more efficent that concatenation. Prefer them.
        }

		[Koan(22)]
		public void StringBuilderCanUseFormatAsWell()
		{
			var strBuilder = new System.Text.StringBuilder();
			strBuilder.AppendFormat("{0} {1} {2}", "The", "quick", "brown");
			strBuilder.AppendFormat("{0} {1} {2}", "jumped", "over", "the");
			strBuilder.AppendFormat("{0} {1}.", "lazy", "dog");
			var str = strBuilder.ToString();
			Assert.Equal("The quick brownjumped over thelazy dog.", str);
            //notice no spaces added after the {2} positions, and the period added in the thir strBuilder.AppendFormat
		}
		
        [Koan(23)]
        public void LiteralStringsInterpretsEscapeCharacters()
        {
            var str = "\n";
            Assert.Equal(1, str.Length);
            //in literal strings, "\" is not a character because it is escaped.
        }

        [Koan(24)]
        public void VerbatimStringsDoNotInterpretEscapeCharacters()
        {
            var str = @"\n";
            Assert.Equal(2, str.Length);
            //since verbatim strings are, well, verbatim, "\" is a character because escaping is not verbatim.
        }

        [Koan(25)]
        public void VerbatimStringsStillDoNotInterpretEscapeCharacters()
        {
            var str = @"\\\";
            Assert.Equal(3, str.Length);
            //no really, verbatim doesn't escape at all.
        }

        [Koan(28)]
        public void YouCanGetASubstringFromAString()
        {
            var str = "Bacon, lettuce and tomato";
            Assert.Equal("tomato", str.Substring(19));
            //this works because the substring with one parameter starts the substring and continues it
            //until the end of the string
            Assert.Equal("let", str.Substring(7, 3));
            //with two parameters, it starts the substring at the index of parameter 1, and continues
            //for the second number of offsets (in this case, three. Notice, we count the first one too, unlike zero indexes)
        }

        [Koan(29)]
        public void YouCanGetASingleCharacterFromAString()
        {
            var str = "Bacon, lettuce and tomato";
            Assert.Equal('B', str[0]);
            //remember, chars don't like "", but they do like ''.
        }

        [Koan(30)]
        public void SingleCharactersAreRepresentedByIntegers()
        {
            Assert.Equal(97, 'a');
            Assert.Equal(98, 'b');
            //these are 'assigning' their first parameter values to their second parameter chars.
            Assert.Equal(true, 'b' == ('a' + 1));
            /*in this case, since 98 == (97+1), it evals to true. But this will get a throw :
            Assert.Equal(false, 'b' == ('a' + 1));
            As will this:
            Assert.Equal(true, 'b' == ('a' + 2));
            */

        }

        [Koan(31)]
        public void StringsCanBeSplit()
        {
            var str = "Sausage Egg Cheese";
            string[] words = str.Split();
            //this splits it on spaces by default?
            Assert.Equal(new[] { "Sausage", "Egg", "Cheese" }, words);
        }

        [Koan(32)]
        public void StringsCanBeSplitUsingCharacters()
        {
            var str = "the:rain:in:spain";
            string[] words = str.Split(':');
            //this splits it on the :. notice the '' telling the .Split that ':' is a char.
            Assert.Equal(new[] { "the", "rain", "in", "spain" }, words);
        }

        [Koan(33)]
        public void StringsCanBeSplitUsingRegularExpressions()
        {
            var str = "the:rain:in:spain";
            var regex = new System.Text.RegularExpressions.Regex(":");
            string[] words = regex.Split(str);
            Assert.Equal(new[] { "the", "rain", "in", "spain" }, words);

            //I for real don't know what this is teaching me. Why not use str.Split(':')?

            //A full treatment of regular expressions is beyond the scope
            //of this tutorial. The book "Mastering Regular Expressions"
            //is highly recommended to be on your bookshelf
        }
    }
}
