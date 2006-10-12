using System;
using System.Collections.Generic;
using System.Text;

namespace torrent.libtorrent
{
    public class ByteString
    {
        private byte[] bytes;
        private string stringValue;

        public ByteString(byte[] bytes)
        {
            this.bytes = bytes;
            stringValue = Encoding.Default.GetString(bytes);
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is ByteString))
            {
                return false;
            }
            else
            {
                return stringValue.Equals((obj as ByteString).ToString());
            }
        }

        public ByteString(string value)
        {
            stringValue = value;
            bytes = Encoding.Default.GetBytes(value);
        }

        public override string ToString()
        {
            return stringValue;
        }

        public byte[] ToBytes()
        {
            return bytes;
        }

        public int IndexOf(string searchString)
        {
            return stringValue.IndexOf(searchString);
        }
        
        public int IndexOf(string searchString, int start)
        {
            return stringValue.IndexOf(searchString, start);
        }

        public ByteString SubString(int start, int length)
        {
            return new ByteString(stringValue.Substring(start, length));
        }
        
        public ByteString SubString(int start)
        {
            return SubString(start, stringValue.Length - start);
        }
        
        public ByteString[] Split(params char[] separator)
        {
            return new List<string>(stringValue.Split(separator)).ConvertAll<ByteString>(delegate(string input){ return new ByteString(input);}).ToArray();
        }

        public ByteString Trim()
        {
            return new ByteString(stringValue.Trim());
        }
    }
}
