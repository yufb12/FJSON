using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FJsonTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 在解析的同时付值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="obj"></param>
        public void OnJsonObjCreate(Feng.Json.JsonObj json, object obj)
        {
            List<string> keys = obj as List<string>;
            if (keys != null)
            {
                if (json is Feng.Json.JsonObjItem)
                {
                    Feng.Json.JsonObjItem item = json as Feng.Json.JsonObjItem;
                    keys.Add(item.Key);

                    this.richTextBox2.AppendText(item.Key);
                    this.richTextBox2.AppendText(":");
                    if (item.Value == null)
                    {
                        this.richTextBox2.AppendText("null");
                    }
                    else
                    {
                        this.richTextBox2.AppendText(item.Value.ToString());
                    }
                    this.richTextBox2.AppendText("\r\n");
                }
            } 
        }
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> keys = new List<string>();
                this.richTextBox2.Text = string.Empty;
                this.richTextBox2.AppendText("在解析的同时赋值:\r\n");

                Feng.Json.JsonObj obj = Feng.Json.JsonTool.Parese(this.richTextBox1.Text, OnJsonObjCreate, keys);
                TreeNode node = this.treeView1.Nodes.Add(obj.Value.ToString());
                node.Tag = obj;

                if (obj.Type == Feng.Json.JsonValueType.ArrayValue)
                {
                    List<Feng.Json.JsonObj> list = obj.Value as List<Feng.Json.JsonObj>;
                    foreach (Feng.Json.JsonObj item in list)
                    {
                        node = node.Nodes.Add(item.Value.ToString());
                        node.Tag = item;
                    }
                }
                if (obj.Type == Feng.Json.JsonValueType.ArrayItem)
                {
                    List<Feng.Json.JsonObjItem> list = obj.Value as List<Feng.Json.JsonObjItem>;
                    foreach (Feng.Json.JsonObjItem item in list)
                    {
                        node = node.Nodes.Add(item.Key);
                        node.Tag = item;
                    }
                }

                if (obj.Type == Feng.Json.JsonValueType.Object)
                {
                    node = node.Nodes.Add(obj.Value.ToString());
                    node.Tag = obj.Value;
                } 
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                Feng.Json.JsonObj obj = e.Node.Tag as Feng.Json.JsonObj;
                if (obj == null)
                    return;
                e.Node.Nodes.Clear();
                TreeNode node = e.Node;
                if (obj.Type == Feng.Json.JsonValueType.ArrayValue)
                {
                    List<Feng.Json.JsonObj> list = obj.Value as List<Feng.Json.JsonObj>;
                    foreach (Feng.Json.JsonObj item in list)
                    {
                        TreeNode node1 = node.Nodes.Add(item.Value.ToString());
                        node1.Tag = item;
                    }
                }
                if (obj.Type == Feng.Json.JsonValueType.ArrayItem)
                {
                    List<Feng.Json.JsonObjItem> list = obj.Value as List<Feng.Json.JsonObjItem>;
                    foreach (Feng.Json.JsonObjItem item in list)
                    {
                        TreeNode node1 = node.Nodes.Add(item.Key);
                        node1.Tag = item;
                    }
                }

                if (obj.Type == Feng.Json.JsonValueType.Object)
                {
                    Feng.Json.JsonObj jsonObj = obj.Value as Feng.Json.JsonObj;
                    TreeNode node1 = node.Nodes.Add(obj.Value.ToString());
                    node1.Tag = obj.Value;
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
