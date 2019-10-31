using System;

namespace Test.Messages
{
    public class TextMessage
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text; 
        }
    }
}
