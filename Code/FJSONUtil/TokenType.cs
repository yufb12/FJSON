using System;
using System.Collections.Generic;
using System.Linq; 

namespace Feng.Json
{
    public class TokenType
    {
        public static int STRING = 1;       //VALUE
        public static int NUMBER = 2;       //124.34,0x124
        public static int TRUE = 6;         //true
        public static int FALSE = 7;        //false
        public static int NULL = 8;         //null 
        /// <summary>
        /// '('
        /// </summary>
        public static int LPAREN = '(';      // ("("), 
        /// <summary>
        /// ')'
        /// </summary>
        public static int RPAREN = ')';      // (")"), 
        /// <summary>
        /// '{'
        /// </summary>
        public static int LBRACE = '{';      // ("{"), 
        /// <summary>
        /// '}'
        /// </summary>
        public static int RBRACE = '}';      // ("}"), 
        /// <summary>
        /// '['
        /// </summary>
        public static int LBRACKET = '[';    // ("["), 
        /// <summary>
        /// ']'
        /// </summary>
        public static int RBRACKET = ']';    // ("]"), 
        /// <summary>
        /// ','
        /// </summary>
        public static int COMMA = ',';       // (","), 
        /// <summary>
        /// ':'
        /// </summary>
        public static int COLON = ':';       // (":"),
    }
}
