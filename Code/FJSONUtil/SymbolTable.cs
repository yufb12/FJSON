using System;
using System.Collections.Generic;
using System.Linq; 

namespace Feng.Json
{
    public class SymbolTable
    {
        public SymbolTable()
        {
            List = new List<Token>();
        }
        public List<Token> List { get; set; }
    }
}
