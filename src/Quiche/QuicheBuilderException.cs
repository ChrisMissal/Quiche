namespace Quiche
{
    using System;

    public class QuicheBuilderException : InvalidOperationException
    {
        private const string IssuesUrl = @"https://github.com/ChrisMissal/Quiche/issues";
        private const char Tab = (char)9;

        internal QuicheBuilderException(Parameter parameter) : base(GetMessage())
        {
        }

        private static string GetMessage()
        {
            return string.Format(@"There was a problem trying to build a URL for your object.{0}{1}> Please report this issue with your object's structure here: {2}{0}",
                Environment.NewLine, Tab, IssuesUrl);
        }
    }
}