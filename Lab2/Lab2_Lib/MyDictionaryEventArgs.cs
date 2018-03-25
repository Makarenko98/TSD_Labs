using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_Lib
{
    public class MyDictionaryEventArgs <TKey, TValue>: EventArgs
    {
        public KeyValuePair<TKey, TValue> Item { get; set; }

        public MyDictionaryEventArgs(KeyValuePair<TKey, TValue> item)
        {
            Item = item;
        }
    }
}
