using System;
using System.Collections.Generic;
using System.Linq; 

namespace Feng.Json
{
    public class Token
    {
        public int ID { get; set;}
        public int Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public Token(int id, int type, int line, int column)
        {
            this.ID = id;
            this.Type = type;
            this.Value = "" + (char)type;
            this.Line = line;
            this.Column = column;
        }
        public Token(int id, string value, int line, int column)
        {
            this.ID = id;
            this.Type = TokenType.STRING;
            this.Value = value;
            this.Line = line;
            this.Column = column;
        }
        public Token(int id, int type, string value, int line, int column)
        {
            this.ID = id;
            this.Type = type;
            this.Value = value;
            this.Line = line;
            this.Column = column;
        }
        public Token(int id, decimal value, int line, int column)
        {
            this.ID = id;
            this.Type = TokenType.NUMBER;
            this.Value = value.ToString();
            this.Line = line;
            this.Column = column;
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", ID, Type, Value, Line, Column);
        }
        public object ToValue()
        {
            if (Type == TokenType.FALSE)
            {
                return false;
            } 
            if (Type == TokenType.TRUE)
            {
                return true;
            }
            if (Type == TokenType.NULL)
            {
                return null;
            }
            if (Type == TokenType.NUMBER)
            {
                return Convert.ToDecimal(Value);
            }
            if (Type == TokenType.STRING)
            {
                return (Value);
            }
            return null;
        }
    }
}
