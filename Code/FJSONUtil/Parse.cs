using System;
using System.Collections.Generic;
using System.Linq;

namespace Feng.Json
{
    public class JsonTool
    {
        private SymbolTable symbolTable = null;
        private int position = -1;
        private Token currentToken
        {
            get
            {
                if (position < symbolTable.List.Count)
                {
                    return symbolTable.List[position];
                }
                return null;
            }
        }

        private bool IsEnd()
        {
            return !(position < symbolTable.List.Count);
        }

        private bool HasNext()
        {
            return position < symbolTable.List.Count - 1;
        }

        private Token GetNext()
        {
            return symbolTable.List[position + 1];
        }

        private void Forword()
        {
            position++;
            if (currentToken.Type == TokenType.STRING)
            {
                Console.WriteLine("\"" + currentToken.Value + "\"");
            }
            else
            {
                Console.WriteLine(currentToken.Value);
            }
        }

        private static JsonTool jsontool = null;

        public static JsonObj Parese(string text)
        {
            return Parese(text, null,null);
        }

        public static JsonObj Parese(string text, JsonObjCreateHandler JsonObjCreate,object obj)
        {
            if (jsontool == null)
            {
                jsontool = new JsonTool();
            }
            SymbolTable symbol = Lexer.GetSymbolTable(text);
            jsontool.Init(JsonObjCreate, obj);
            return jsontool.ParseTable(symbol);
        }

        public void Init(JsonObjCreateHandler jsonobjcreate,object obj)
        {
            JsonObjCreate = jsonobjcreate;
            JsonObj = obj;
        }

        private JsonObjCreateHandler JsonObjCreate = null;
        private object JsonObj = null;
        public void OnJsonObjCreate(JsonObj json)
        {
            if (JsonObjCreate != null)
            {
                JsonObjCreate(json, JsonObj);
            } 
        }

        public delegate void JsonObjCreateHandler(JsonObj json, object obj);

        //JSON产生式
        //E=[E,E]|E|{s:E,|s:E}|a
        //    E=[T,T]|T
        //    T={F,F}|a
        //    F=s:E
        Token tokennext = null;
        public JsonObj ParseTable(SymbolTable table)
        {
            symbolTable = table;
            position = -1;
            JsonObj jsonobj = Parse_E_Rule(null);
            return jsonobj;
        }

        public JsonObj Parse_E_Rule(JsonObj pobj)
        {
            if (HasNext())
            {
                Token token = GetNext();
                if (token.Type == TokenType.LBRACKET)
                {
                    List<JsonObj> list = new List<JsonObj>();
                    JsonObj objres = new JsonObj() { Type = JsonValueType.ArrayValue, Value = list, Parent = pobj };
                    Forword();
                    JsonObj obj = Parse_T_Rule(objres);
                    list.Add(obj);
                    if (!HasNext())
                    {
                        Error(1001, "Not Finish");
                    }
                    tokennext = GetNext();
                    while (tokennext.Type == TokenType.COMMA)
                    {
                        Forword();
                        obj = Parse_T_Rule(objres);
                        list.Add(obj);
                        tokennext = GetNext();
                    }
                    if (tokennext.Type != TokenType.RBRACKET)
                    {
                        Error(1002, "Not Finish Symbol Invalid");
                    }
                    Forword();

                    OnJsonObjCreate(objres);
                    return objres;
                }
                else
                {
                    return Parse_T_Rule(pobj);
                }
            }
            return null;
        }

        private JsonObj Parse_T_Rule(JsonObj pobj)
        {
            if (HasNext())
            {
                tokennext = GetNext();
                if (tokennext.Type == TokenType.LBRACE)
                {
                    Forword();
                    List<JsonObjItem> list = new List<JsonObjItem>();
                    JsonObj objres = new JsonObj() { Type = JsonValueType.ArrayItem, Value = list, Parent = pobj };
                    JsonObjItem obj = Parse_F_Rule(objres);
                    list.Add(obj);
                    if (!HasNext())
                    {
                        Error(3001, "Not Finish");
                    }
                    tokennext = GetNext();
                    while (tokennext.Type == TokenType.COMMA)
                    {
                        Forword();
                        obj = Parse_F_Rule(objres);
                        list.Add(obj);
                        tokennext = GetNext();
                    }
                    if (tokennext.Type != TokenType.RBRACE)
                    {
                        Error(3002, "Not Finish");
                    }
                    Forword();
                    tokennext = currentToken;
                    OnJsonObjCreate(objres);
                    return objres;
                }
                else
                {
                    if (!HasNext())
                    {
                        Error(3003, "Not Finish");
                    }
                    Forword();
                    tokennext = currentToken;
                    if (tokennext.Type > 9)
                    {
                        Error(3005, "Not Finish Symbol Invalid");
                    }
                    JsonObj jsonObj = new JsonObj() { Type = JsonValueType.Value, Value = tokennext.ToValue(), Parent = pobj };
                    OnJsonObjCreate(jsonObj);
                    return jsonObj;
                }
            }
            return null;
        }

        private JsonObjItem Parse_F_Rule(JsonObj pobj)
        {
            if (HasNext())
            {
                tokennext = GetNext();
                if (tokennext.Type != TokenType.STRING)
                {
                    Error(5004, "Symbol Invalid");
                }
                Forword();
                tokennext = currentToken;
                string key = tokennext.Value;
                if (!HasNext())
                {
                    Error(5005, "Accident  Finish");
                }
                tokennext = GetNext();
                if (tokennext.Type != TokenType.COLON)
                {
                    Error(5006, "Symbol Invalid");
                }
                Forword();
                tokennext = currentToken;
                JsonObjItem obj = new JsonObjItem();
                obj.Key = key;
                JsonObj value = Parse_E_Rule(pobj);
                obj.Value = value;
                obj.Type = JsonValueType.Object;
                obj.Parent = pobj;
                OnJsonObjCreate(obj);
                return obj;
            }
            return null;
        }

        public void Error(int errorcode, string error)
        {
            throw new Exception("ErrorCode:" + errorcode + "." + error);
        }
    }
}
