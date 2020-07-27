using System;
using System.Collections.Generic;

namespace Feng.Json
{
    public class JsonObj
    {
        public JsonObj()
        {

        }
        public object Value { get; set; }
        public short Type { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Value);
        }
 
        public JsonObj this[int index]
        {
            get
            {
                if (this.Type == JsonValueType.ArrayValue)
                {
                    List<JsonObj> list = this.Value as List<JsonObj>;
                    return list[index];
                }
                return null;
            }
        }
        public JsonObj this[string key]
        {
            get
            {
                if (this.Type == JsonValueType.ArrayItem)
                {
                    List<JsonObjItem> list = this.Value as List<JsonObjItem>;
                    foreach (JsonObjItem item in list)
                    {
                        if (item.Key == key)
                        {
                            return item.Value as JsonObj;
                        }
                    }
                }
                return null;
            }
        }

        public void ToJson(System.Text.StringBuilder sb)
        {
            switch (this.Type)
            {
                case JsonValueType.ArrayItem:
                    sb.Append("{");
                    List<JsonObjItem> list = Value as List<JsonObjItem>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        JsonObjItem obj = list[i];
                        obj.ToJson(sb);
                        if (i != list.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    sb.Append("}");
                    break;
                case JsonValueType.ArrayValue:
                    sb.Append("[");
                    List<JsonObj> list2 = Value as List<JsonObj>;
                    for (int i = 0; i < list2.Count; i++)
                    {
                        JsonObj obj = list2[i];
                        obj.ToJson(sb);
                        if (i != list2.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    sb.Append("]");
                    break;
                case JsonValueType.Object:
                    JsonObjItem item = this as JsonObjItem;
                    sb.Append("\"");
                    sb.Append(item.Key);
                    sb.Append("\"");
                    sb.Append(":");
                    JsonObj obj2 = item.Value as JsonObj;
                    obj2.ToJson(sb);
                    break;
                case JsonValueType.Value:
                    if (Value == null)
                    {
                        sb.Append("null");
                    }
                    else if (Value.Equals(true))
                    {
                        sb.Append("true");
                    }
                    else if (Value.Equals(false))
                    {
                        sb.Append("false");
                    }
                    else if (Value is string)
                    {
                        sb.Append("\"");
                        sb.Append(Value.ToString());
                        sb.Append("\"");
                    }
                    else if (Value is decimal)
                    {
                        sb.Append(Value.ToString());
                    }
                    else
                    {
                        throw new Exception("type error");
                    }
                    break;
                default:
                    throw new Exception("type is error");
            }
        }

        public void GetJsonObjItems(List<JsonObjItem> buf)
        {
            switch (this.Type)
            {
                case JsonValueType.ArrayItem:
                    List<JsonObjItem> list = Value as List<JsonObjItem>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        JsonObjItem obj = list[i];
                        obj.GetJsonObjItems(buf); 
                    }
                    break;
                case JsonValueType.ArrayValue:
                    List<JsonObj> list2 = Value as List<JsonObj>;
                    for (int i = 0; i < list2.Count; i++)
                    {
                        JsonObj obj = list2[i];
                        obj.GetJsonObjItems(buf); 
                    }
                    break;
                case JsonValueType.Object:
                    JsonObjItem item = this as JsonObjItem;
                    JsonObj obj2 = item.Value as JsonObj;
                    obj2.GetJsonObjItems(buf);
                    //if (obj2.Type == JsonValueType.Value)
                    //{
                        buf.Add(item);
                    //}
                    break;
                case JsonValueType.Value: 
                    break;
                default:
                    break;
            }
        }
        public void GetJsonObjList(List<JsonObj> buf)
        {
            switch (this.Type)
            {
                case JsonValueType.ArrayItem: 
                    List<JsonObjItem> list = Value as List<JsonObjItem>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        JsonObjItem obj = list[i];
                        obj.GetJsonObjList(buf);
                        buf.Add(obj);
                    }
                    break;
                case JsonValueType.ArrayValue: 
                    List<JsonObj> list2 = Value as List<JsonObj>;
                    for (int i = 0; i < list2.Count; i++)
                    {
                        JsonObj obj = list2[i];
                        obj.GetJsonObjList(buf);
                        buf.Add(obj);
                    } 
                    break;
                case JsonValueType.Object:
                    JsonObjItem item = this as JsonObjItem;
                    buf.Add(item);
                    JsonObj obj2 = item.Value as JsonObj;
                    obj2.GetJsonObjList(buf);
                    break;
                case JsonValueType.Value:
                    buf.Add(this);
                    break;
                default:
                    throw new Exception("type is error");
            }
        }

        public JsonObj Parent { get; set; }
    }
    public class JsonObjItem : JsonObj
    {
        public JsonObjItem()
        {

        }
        public string Key { get; set; }
        public override string ToString()
        {
            return string.Format("Key:{0} Value={1}", Key, Value);
        }

         
    }

    public class JsonValueType
    {
        public const short Value = 1;
        public const short Object = 2;
        public const short ArrayItem = 3;
        public const short ArrayValue = 4;
    }
}
