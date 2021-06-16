using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Zuby.ADGV;
using System.IO;
using System.Threading;
using ConnectionWizard;
using PersistenceLayer;
using System.Xml;
using System.Web.Script.Serialization;
using System.Data.OleDb;
using System.IO.Compression;
using System.Data.Common;


namespace JS_Automation
{
    enum CatalogTreeNodeType
    {
        Class, Interface, Attribute, Package
    }
    public partial class Form1 : Form
    {
        
        private DataTable table;
        private string connectionString;
        private string blobConnectionString;
        private TreeviewTypes treeViewSelected;
        private String catalogTreeViewQuery;
        public const string S3DClassConfig_DefaultNodes_Attribute = "Table for Attribute";
        public const string S3DClassConfig_DefaultNodes_Interface = "Table for Interface";

        //string ConnectionString = "Data Source=XENON; Initial Catalog=AST2_CDB_SCHEMA;integrated security=true;";

        private List<AddedInterfaces> AddedInterfaces = new List<AddedInterfaces>();
        private List<String> mAddedAttributes = new List<String>();
        private int aliasingCounter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void showCircularProgress(frmWaitForm fui)
        {
            //fui.StartPosition = FormStartPosition.CenterScreen;
            //fui.Dispose();
            fui.startTimer();
            fui.ShowDialog();
        }

        public void readConnectionString()
        {
            try
            {
                String connectionStringXML = Path.GetDirectoryName(Application.LocalUserAppDataPath)
                + "\\ConnectionStrings.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(connectionStringXML);
                connectionString = doc.DocumentElement.
                    GetElementsByTagName("SqlConnectionString").Item(0).InnerText;
            }
            catch (Exception exception) { }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readConnectionString();
            while (String.IsNullOrEmpty(this.connectionString))
            {
                Form99 frmconn = new Form99();
                frmconn.ShowDialog();
                readConnectionString();
            }

            string[] connectionStringParts = connectionString.Split(';');
            string database = "", serverName = "";
            foreach (String connectionStringPart in connectionStringParts)
            {
                if (connectionStringPart.Contains("Data Source"))
                {
                    serverName = connectionStringPart.Substring(connectionStringPart.LastIndexOf('=') + 1);
                }
                if (connectionStringPart.Contains("Initial Catalog"))
                {
                    database = connectionStringPart.Substring(connectionStringPart.LastIndexOf('=') + 1);
                }
            }

            GetSiteConnect gPlant = new GetSiteConnect();
            gPlant.Server = serverName;
            gPlant.Database = database;
            DialogResult dialogResult = gPlant.ShowDialog();

            if (dialogResult != DialogResult.OK) return;

            frmWaitForm fui = new frmWaitForm();
            Thread t2 = new Thread(() => showCircularProgress(fui));
            t2.Start();

            this.treeViewInstance.Nodes.Clear();
            this.tv_WBS.Nodes.Clear();
            this.tv_DRAWING.Nodes.Clear();
            this.tv_Catalog.Nodes.Clear();

            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.System))
                populateSystemTreeView(connectionString);
            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.WBS))
                populateWBSTreeView(connectionString);
            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.DrawReport))
                populateDrawReportTreeView(connectionString);
            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.WBS))
                populatecataTreeView(connectionString);

            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.Catalog))
            {
                //Populate the catalog treeview
                JSDataRef.J3D J3D = new JSDataRef.J3D();
                J3D.ServerName = ".\\SQLEXPRESS";
                J3D.CatalogSchemaDB = "AST2_CDB_SCHEMA";
                J3D.Connect();

                Dictionary<String, int> counterItem = new Dictionary<string, int>();
                int i = 0, j = 0, k = 0;
                foreach (JSDataRef.J3DClassDef classDef in J3D.CDBClassDefs)
                {
                    j = 0;

                    TreeNode classTreeNode = new TreeNode(classDef.Name);
                    classTreeNode.Name = i.ToString();
                    classTreeNode.Tag = CatalogTreeNodeType.Class;
                    classTreeNode.ImageIndex = 0;
                    classTreeNode.SelectedImageIndex = 0;

                    this.tv_Catalog.Nodes.Add(classTreeNode);

                    foreach (JSDataRef.J3DInterfaceDef interfaceDef in classDef.InterfaceDefs)
                    {
                        k = 0;
                        TreeNode interfaceTreeNode = new TreeNode(interfaceDef.Name);
                        interfaceTreeNode.Name = j.ToString();
                        interfaceTreeNode.Tag = CatalogTreeNodeType.Interface;
                        interfaceTreeNode.ImageIndex = 1;
                        interfaceTreeNode.SelectedImageIndex = 1;

                        this.tv_Catalog.Nodes[i].Nodes.Add(interfaceTreeNode);
                        foreach (JSDataRef.J3DMemberDef memberDef in interfaceDef.MemberDefs)
                        {
                            TreeNode attributeTreeNode = new TreeNode(memberDef.Name);
                            attributeTreeNode.Name = memberDef.Name;
                            attributeTreeNode.ImageIndex = 2;
                            attributeTreeNode.SelectedImageIndex = 2;
                            attributeTreeNode.Tag = CatalogTreeNodeType.Attribute;

                            this.tv_Catalog.Nodes[i].Nodes[j].Nodes.Add(attributeTreeNode);
                            foreach (JSDataRef.J3DPackageDef packageDef in memberDef.PackageDefs)
                            {
                                if (packageDef.Name.Equals("Name"))
                                {
                                    if (memberDef.Name.ToLower().Equals("jnameditem"))
                                    {
                                        //packageDef.Name = "ItemName";
                                    }
                                }
                                TreeNode newPackageTreeNode = new TreeNode(packageDef.Name);
                                newPackageTreeNode.Name = packageDef.Name;
                                newPackageTreeNode.ImageIndex = 3;
                                newPackageTreeNode.SelectedImageIndex = 3;
                                newPackageTreeNode.Tag = CatalogTreeNodeType.Package;

                                this.tv_Catalog.Nodes[i].Nodes[j].Nodes[k].Nodes.Add(newPackageTreeNode);
                            }
                            k++;
                        }
                        j++;
                    }
                    i++;
                }

            }
           // fui.shutDownThread = true;
            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.System)) addImagesToLeafNodes();
            //this.treeViewInstance.ExpandAll();


            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.Catalog))
            {
                //Populate the catalog treeview
                JSDataRef.J3D1 J3D1 = new JSDataRef.J3D1();
                J3D1.ServerName = ".\\SQLEXPRESS";
                J3D1.CatalogSchemaDB = "AST2_CDB_SCHEMA";
                J3D1.Connect();

                Dictionary<String, int> counterItem = new Dictionary<string, int>();
                int i = 0, j = 0, k = 0;
                foreach (JSDataRef.J3DClassDef classDef in J3D1.CDBClassDefs)
                {
                    j = 0;

                    TreeNode classTreeNode = new TreeNode(classDef.Name);
                    classTreeNode.Name = i.ToString();
                    classTreeNode.Tag = CatalogTreeNodeType.Class;
                    classTreeNode.ImageIndex = 0;
                    classTreeNode.SelectedImageIndex = 0;

                    this.tv_Catalog2.Nodes.Add(classTreeNode);

                    foreach (JSDataRef.J3DInterfaceDef interfaceDef in classDef.InterfaceDefs)
                    {
                        k = 0;
                        TreeNode interfaceTreeNode = new TreeNode(interfaceDef.Name);
                        interfaceTreeNode.Name = j.ToString();
                        interfaceTreeNode.Tag = CatalogTreeNodeType.Interface;
                        interfaceTreeNode.ImageIndex = 1;
                        interfaceTreeNode.SelectedImageIndex = 1;

                        this.tv_Catalog2.Nodes[i].Nodes.Add(interfaceTreeNode);
                        foreach (JSDataRef.J3DMemberDef memberDef in interfaceDef.MemberDefs)
                        {
                            TreeNode attributeTreeNode = new TreeNode(memberDef.Name);
                            attributeTreeNode.Name = memberDef.Name;
                            attributeTreeNode.ImageIndex = 2;
                            attributeTreeNode.SelectedImageIndex = 2;
                            attributeTreeNode.Tag = CatalogTreeNodeType.Attribute;

                            this.tv_Catalog2.Nodes[i].Nodes[j].Nodes.Add(attributeTreeNode);
                            foreach (JSDataRef.J3DPackageDef packageDef in memberDef.PackageDefs)
                            {
                                if (packageDef.Name.Equals("Name"))
                                {
                                    if (memberDef.Name.ToLower().Equals("jnameditem"))
                                    {
                                        //packageDef.Name = "ItemName";
                                    }
                                }
                                TreeNode newPackageTreeNode = new TreeNode(packageDef.Name);
                                newPackageTreeNode.Name = packageDef.Name;
                                newPackageTreeNode.ImageIndex = 3;
                                newPackageTreeNode.SelectedImageIndex = 3;
                                newPackageTreeNode.Tag = CatalogTreeNodeType.Package;

                                this.tv_Catalog2.Nodes[i].Nodes[j].Nodes[k].Nodes.Add(newPackageTreeNode);
                            }
                            k++;
                        }
                        j++;
                    }
                    i++;
                }

            }
            fui.shutDownThread = true;
            if (gPlant.SelectedTreeViews.Contains(TreeviewTypes.System)) addImagesToLeafNodes();
        }

        public void populateSystemTreeView(string connectionString)
        {

            DataTable dataTable = (new MainFormDBAccess(connectionString)).getSystemTreeViewData();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    char[] charArr = new char[] { '\\' };
                    string[] arrPath = dataRow[0].ToString().Split(charArr);

                    TreeNode currentNode = null;

                    for (int count = 0; count < arrPath.Length; count++)
                    {
                        if (count == 0)
                        {
                            if (!this.treeViewInstance.Nodes.ContainsKey(arrPath[count]))
                            {
                                TreeNode treeNode = new TreeNode(arrPath[count]);
                                treeNode.Name = arrPath[count];
                                treeNode.ImageIndex = 0;
                                treeNode.SelectedImageIndex = 0;
                                this.treeViewInstance.Nodes.Add(treeNode);

                                currentNode = treeNode;
                            }
                            else
                            {
                                currentNode = this.treeViewInstance.Nodes.Find(arrPath[count], false).FirstOrDefault();
                            }
                        }
                        else if (!currentNode.Nodes.ContainsKey(arrPath[count]))
                        {
                            TreeNode treeNode = new TreeNode(arrPath[count]);
                            treeNode.Name = arrPath[count];
                            currentNode.Nodes.Add(treeNode);
                            treeNode.ImageIndex = regularPatternCheck(treeNode);
                            treeNode.SelectedImageIndex = regularPatternCheck(treeNode);

                            currentNode = treeNode;
                        }
                        else
                        {
                            currentNode = currentNode.Nodes.Find(arrPath[count], false).FirstOrDefault();
                        }
                    }
                }
                catch (Exception exception) { }
            }
        }

        public void populateWBSTreeView(string connectionString)
        {
           // TreeNode rootNode = null;
            try
            {
                DataTable dataTable = (new MainFormDBAccess(connectionString)).getWBSTreeViewData();

                foreach (DataRow dataRow in dataTable.Rows)
                {

                    char[] charArr = new char[] { '\\' };
                    string[] arrPath = dataRow[0].ToString().Split(charArr);

                    TreeNode currentNode = null;

                    for (int count = 0, treeNodeDepth = 0; count < arrPath.Length; count++, treeNodeDepth++)
                    {
                        if (count == 0)
                        {
                            if (!this.tv_WBS.Nodes.ContainsKey(arrPath[count]))
                            {
                                TreeNode treeNode = new TreeNode(arrPath[count]);
                                treeNode.Name = arrPath[count];
                                //Same icon for all the nodes of WBS treeview
                                treeNode.ImageIndex = 8;
                                treeNode.SelectedImageIndex = 8;
                                this.tv_WBS.Nodes.Add(treeNode);

                                currentNode = treeNode;
                            }
                            else
                            {
                                currentNode = this.tv_WBS.Nodes.Find(arrPath[count], false).FirstOrDefault();
                            }
                        }
                        else if (!currentNode.Nodes.ContainsKey(arrPath[count]))
                        {
                            TreeNode treeNode = new TreeNode(arrPath[count]);
                            treeNode.Name = arrPath[count];
                            currentNode.Nodes.Add(treeNode);
                            //Same icon for all the nodes of WBS treeview
                            treeNode.ImageIndex = 8;
                            treeNode.SelectedImageIndex = 8;

                            currentNode = treeNode;
                        }
                        else
                        {
                            currentNode = currentNode.Nodes.Find(arrPath[count], false).FirstOrDefault();
                        }
                    }
                }
                this.tv_WBS.Sort();
            }
            catch (Exception exception) { }
        }

        private void populateDrawReportTreeView(string connectionString)
        {

            TreeNode rootNode = null;

            try
            {
                DataTable dataTable = (new MainFormDBAccess(connectionString)).getDrawReportData();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    char[] charArr = new char[] { '\\' };
                    string[] arrPath = dataRow[0].ToString().Split(charArr);

                    TreeNode currentNode = null;

                    for (int count = 0, treeNodeDepth = 0; count < arrPath.Length; count++, treeNodeDepth++)
                    {
                        if (count == 0)
                        {
                            if (!this.tv_DRAWING.Nodes.ContainsKey(arrPath[count]))
                            {
                                TreeNode treeNode = new TreeNode(arrPath[count]);
                                treeNode.Name = arrPath[count];
                                treeNode.ImageIndex = 0;
                                treeNode.SelectedImageIndex = 0;
                                this.tv_DRAWING.Nodes.Add(treeNode);

                                rootNode = currentNode = treeNode;
                            }
                            else
                            {
                                currentNode = this.tv_DRAWING.Nodes.Find(arrPath[count], false).FirstOrDefault();
                            }
                        }
                        else if (!currentNode.Nodes.ContainsKey(arrPath[count]))
                        {
                            TreeNode treeNode = new TreeNode(arrPath[count]);
                            treeNode.Name = arrPath[count];
                            currentNode.Nodes.Add(treeNode);
                            treeNode.ImageIndex = regularPatternCheck_DrawingReport(treeNode);
                            treeNode.SelectedImageIndex = regularPatternCheck_DrawingReport(treeNode);

                            currentNode = treeNode;
                        }
                        else
                        {
                            currentNode = currentNode.Nodes.Find(arrPath[count], false).FirstOrDefault();
                        }
                    }

                    if (!currentNode.Nodes.ContainsKey(dataRow[1].ToString()))
                    {
                        TreeNode treeNode = new TreeNode(dataRow[1].ToString());
                        treeNode.Name = dataRow[1].ToString();
                        currentNode.Nodes.Add(treeNode);
                        treeNode.ImageIndex = regularPatternCheck_DrawingReport(treeNode);
                        treeNode.SelectedImageIndex = regularPatternCheck_DrawingReport(treeNode);

                        currentNode = treeNode;
                    }
                    else
                    {
                        currentNode = currentNode.Nodes.Find(dataRow[1].ToString(), false).FirstOrDefault();
                    }
                }

                List<TreeNode> childrenNodes = new List<TreeNode>();
                addChildrenNodesRecursive(rootNode, childrenNodes);

                foreach (TreeNode ChildNode in childrenNodes)
                {
                    if (ChildNode.GetNodeCount(true) == 0)
                    {
                        ChildNode.ImageIndex = 7;
                        ChildNode.SelectedImageIndex = 7;
                        ChildNode.Parent.ImageIndex = 5;
                        ChildNode.Parent.SelectedImageIndex = 5;
                    }
                }
            }
            catch { }
        }

        private void populatecataTreeView(string connectionString)
        {
            //TreeNode ParentNode = null; 
            try
            {
                Cata_Tree.Nodes.Clear();
                DataTable dataTable = (new MainFormDBAccess(connectionString)).getCataTreeViewData();
                DataTable dt1 = (new MainFormDBAccess(connectionString)).getCataDeppt();
                DataTable dt2 = (new MainFormDBAccess(connectionString)).getPipeSpecList();
                DataTable dt3 = (new MainFormDBAccess(connectionString)).getSpecItems();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TreeNode nodeRoot = new TreeNode(dataRow[0].ToString());                    
                    Cata_Tree.Nodes.Add(nodeRoot);
                    foreach (DataRow dataRow1 in dt1.Rows)
                    {
                        TreeNode nodechild = new TreeNode(dataRow1[0].ToString());
                        Cata_Tree.Nodes[0].Nodes.Add(nodechild);                        
                    }
                    Cata_Tree.Nodes[0].Nodes[0].Nodes.Add(new TreeNode("Test0"));
                    Cata_Tree.Nodes[0].Nodes[1].Nodes.Add(new TreeNode("Test1"));
                    Cata_Tree.Nodes[0].Nodes[2].Nodes.Add(new TreeNode("Test2"));
                    Cata_Tree.Nodes[0].Nodes[3].Nodes.Add(new TreeNode("Test3"));
                    Cata_Tree.Nodes[0].Nodes[4].Nodes.Add(new TreeNode("Test4"));
                    Cata_Tree.Nodes[0].Nodes[5].Nodes.Add(new TreeNode("Test5"));
                    Cata_Tree.Nodes[0].Nodes[6].Nodes.Add(new TreeNode("Test6"));
                    Cata_Tree.Nodes[0].Nodes[7].Nodes.Add(new TreeNode("Test7"));
                    Cata_Tree.Nodes[0].Nodes[8].Nodes.Add(new TreeNode("Test8"));
                    Cata_Tree.Nodes[0].Nodes[9].Nodes.Add(new TreeNode("Test9"));
                    Cata_Tree.Nodes[0].Nodes[10].Nodes.Add(new TreeNode("Test10"));
                    Cata_Tree.Nodes[0].Nodes[11].Nodes.Add(new TreeNode("Test11"));
                    Cata_Tree.Nodes[0].Nodes[12].Nodes.Add(new TreeNode("Test12"));
                    Cata_Tree.Nodes[0].Nodes[13].Nodes.Add(new TreeNode("Test13"));
                    Cata_Tree.Nodes[0].Nodes[14].Nodes.Add(new TreeNode("Test14"));
                    Cata_Tree.Nodes[0].Nodes[15].Nodes.Add(new TreeNode("Test15"));

                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Pipe Specification"));
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Generic Dimensional Data "));
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Insulation"));
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Bolted Data"));                    
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Parts"));                    
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Spec Item"));
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Instrument"));
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Speciality Items"));
                    Cata_Tree.Nodes[0].Nodes[16].Nodes.Add(new TreeNode("Valve Operator Part"));
                    

                    Cata_Tree.Nodes[0].Nodes[17].Nodes.Add(new TreeNode("Test17"));
                    Cata_Tree.Nodes[0].Nodes[18].Nodes.Add(new TreeNode("Test18"));
                    Cata_Tree.Nodes[0].Nodes[19].Nodes.Add(new TreeNode("Test19"));
                    Cata_Tree.Nodes[0].Nodes[20].Nodes.Add(new TreeNode("Test20"));
                    Cata_Tree.Nodes[0].Nodes[21].Nodes.Add(new TreeNode("Test21"));
                    Cata_Tree.Nodes[0].Nodes[22].Nodes.Add(new TreeNode("Test22"));

                    Cata_Tree.Nodes[0].Nodes[16].Nodes[0].Nodes.Add(new TreeNode("Pipe Material Class"));

                    foreach (DataRow dataRow2 in dt2.Rows)
                    {
                        TreeNode nodePipeSpec = new TreeNode(dataRow2[0].ToString());
                        Cata_Tree.Nodes[0].Nodes[16].Nodes[0].Nodes[0].Nodes.Add(nodePipeSpec);                       
                    }

                    foreach (DataRow dataRow3 in dt3.Rows)
                    {
                        TreeNode nodespecitem = new TreeNode(dataRow3[0].ToString());
                        Cata_Tree.Nodes[0].Nodes[16].Nodes[5].Nodes.Add(nodespecitem);
                    }
                }
                this.Cata_Tree.Sort();


               

            }
            catch (Exception exception) {}


        }


        private int regularPatternCheck(TreeNode treeNode)
        {
            if (treeNode.Level == 1 || treeNode.Level == 2)
                return 1;
            else if (treeNode.Level == 3 || treeNode.Level == 4)
                return 2;
            else return 3;
        }

        private int regularPatternCheck_DrawingReport(TreeNode treeNode)
        {
            if (treeNode.Level == 1)
                return 5;
            else if (treeNode.Level == 2)
                return 2;
            else if (treeNode.Level == 3 || treeNode.Level == 4)
                return 3;
            else if (treeNode.Level == 5)
                return 4;
            else if (treeNode.Level == 6)
                return 5;
            else if (treeNode.Level == 7)
                return 5;
            else return 7;
        }

        private void addImagesToLeafNodes()
        {
            TreeNode rootNode = this.treeViewInstance.TopNode;
            List<TreeNode> childrenNodes = new List<TreeNode>();
            addChildrenNodesRecursive(rootNode, childrenNodes);

            foreach (TreeNode ChildNode in childrenNodes)
            {
                if (ChildNode.GetNodeCount(true) == 0)
                {
                    ChildNode.ImageIndex = 3;
                    ChildNode.SelectedImageIndex = 3;
                }
            }
        }

        private void establishConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form99 frmconn = new Form99();
            frmconn.ShowDialog();
        }

        private void existToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult iExitt;
            iExitt = MessageBox.Show("Do you really want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (iExitt == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pipeLineListView.Show();
            tv_S3DClassconfig.Hide();
            dgv.Show();
            dgv1.Hide();
            tv_Catalog.Show();
            tv_Catalog2.Hide();
            
            axAcroPDF1.Visible = false;
            // piperunDataGridView1.DataSource = GetPiperunList();
        }

        private void showPBSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.showPBSToolStripMenuItem.Checked)
            {
                this.treeViewInstance.Visible = false;
                this.showPBSToolStripMenuItem.Checked = false;
                this.splitContainer1.Panel1.Hide();
                this.splitContainer1.SplitterDistance = 0;
                this.showPBSToolStripMenuItem.Checked = false;
            }
            else
            {
                this.showPBSToolStripMenuItem.Checked = true;
                this.treeViewInstance.Visible = true;
                this.showPBSToolStripMenuItem.Checked = true;
                this.splitContainer1.Panel1.Show();
                this.splitContainer1.SplitterDistance = 175;
            }
        }

        private void showPBSComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.showPBSComponentsToolStripMenuItem.Checked)
            {
                this.pipeLineListView.Visible = false;
                this.showPBSComponentsToolStripMenuItem.Checked = false;
                this.splitContainer2.Panel1.Hide();
                this.splitContainer2.SplitterDistance = 0;
                this.showPBSComponentsToolStripMenuItem.Checked = false;
            }
            else
            {
                this.showPBSComponentsToolStripMenuItem.Checked = true;
                this.pipeLineListView.Visible = true;
                this.showPBSComponentsToolStripMenuItem.Checked = true;
                this.splitContainer2.Panel1.Show();
                this.splitContainer2.SplitterDistance = 125;
            }
        }

        private void byUserORDeveloperNameView_Click(object sender, EventArgs e)
        {
            if (this.byUserORDeveloperNameView.Checked)  // tv_Catalog2
            {
                this.tv_Catalog2.Visible = false;
                this.byUserORDeveloperNameView.Checked = false;
                this.tv_Catalog2.Hide();
                this.byUserORDeveloperNameView.Checked = false;
            }
            else
            {
                this.byUserORDeveloperNameView.Checked = true;
                this.tv_Catalog2.Visible = true;
                this.byUserORDeveloperNameView.Checked = true;
                this.tv_Catalog2.Show(); 
            }
        }

        private void attributeConfigurationToolStripMenuItem_Click(object sender, EventArgs e)  // Show Hide Tree And GridView
        {
            if (this.attributeConfigurationToolStripMenuItem.Checked)
            {
                this.tv_S3DClassconfig.Visible = false;
                this.attributeConfigurationToolStripMenuItem.Checked = false;
                this.tv_S3DClassconfig.Hide();
                this.attributeConfigurationToolStripMenuItem.Checked = false;

                this.attributeConfigurationToolStripMenuItem.Checked = true;
                this.dgv.Visible = true;
                this.attributeConfigurationToolStripMenuItem.Checked = true;
                this.dgv.Show(); 

                this.dgv1.Visible = false;
                this.attributeConfigurationToolStripMenuItem.Checked = false;
                this.dgv1.Hide();
                this.attributeConfigurationToolStripMenuItem.Checked = false;
            }            
            else
            {
                this.attributeConfigurationToolStripMenuItem.Checked = true;
                this.tv_S3DClassconfig.Visible = true;
                this.attributeConfigurationToolStripMenuItem.Checked = true;
                this.tv_S3DClassconfig.Show();

                this.dgv.Visible = false;
                this.attributeConfigurationToolStripMenuItem.Checked = false;
                this.dgv.Hide();
                this.attributeConfigurationToolStripMenuItem.Checked = false;

                this.attributeConfigurationToolStripMenuItem.Checked = true;
                this.dgv1.Visible = true;
                this.attributeConfigurationToolStripMenuItem.Checked = true;
                this.dgv1.Show(); 
            }           
            
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)  // show data into grid view
        {
            try
            {
                pipeLineListView.Items.Clear();

                switch (((MultiSelectTreeview)sender).Name)
                {
                    case "treeViewInstance":
                        this.treeViewSelected = TreeviewTypes.System;
                        break;
                    case "tv_WBS":
                        this.treeViewSelected = TreeviewTypes.WBS;
                        break;
                    case "tv_DRAWING":
                        this.treeViewSelected = TreeviewTypes.DrawReport;
                        break;

                }

                foreach (TreeNode selectedNode in ((MultiSelectTreeview)sender).SelectedNodes)
                {
                    foreach (TreeNode childNode in selectedNode.Nodes)
                    {
                        if (!pipeLineListView.Items.ContainsKey(childNode.Text))
                            pipeLineListView.Items.Add(childNode.Text, childNode.Text, null);
                    }
                    if (selectedNode.Nodes.Count == 0)
                    {
                        if (!pipeLineListView.Items.ContainsKey(selectedNode.Text))
                        {
                            pipeLineListView.Items.Add(selectedNode.Text, selectedNode.Text, null);
                        }

                    }
                }


                //pipeLineListView.Items.Clear();
                //List<TreeNode> childrenNodes = new List<TreeNode>();
                //childrenNodes.Add(e.Node);
                //addChildrenNodesRecursive(e.Node, childrenNodes);

                //foreach (TreeNode ChildNode in childrenNodes)
                //{
                //    if (ChildNode.GetNodeCount(true) == 0)
                //        pipeLineListView.Items.Add(ChildNode.Text);
                //}
            }
            catch (Exception ex)
            {
            }
        }

        public void addChildrenNodesRecursive(TreeNode currentTreeNode, List<TreeNode> childrenNodes)
        {
            foreach (TreeNode childNode in currentTreeNode.Nodes)
            {
                childrenNodes.Add(childNode);
                addChildrenNodesRecursive(childNode, childrenNodes);

            }
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }


        private void dgv_FilterStringChanged(object sender, AdvancedDataGridView.FilterEventArgs e) // Filter Shortning Function
        {
            string filterString = e.FilterString;
            if (e.Cancel || filterString == null)
                return;
            if (filterString.Equals(""))
            {
                (this.dgv.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
            else
            {
                try
                {
                    string str1 = filterString.Substring(1, filterString.IndexOf(']'));
                    string[] strArray = filterString.Substring(str1.Length + 6, filterString.Length - 2 - (str1.Length + 6)).Split(',');
                    string str2 = string.Format(str1 + "= '{0}'", (object)strArray[0].Substring(1, strArray[0].Length - 2));
                    if (strArray.Length > 1)
                    {
                        for (int index = 1; index < strArray.Length; ++index)
                            str2 += string.Format(" OR " + str1 + "= '{0}'", (object)strArray[index].Substring(2, strArray[index].Length - 3));
                    }
                    //(this.dgv.DataSource as DataTable).DefaultView.RowFilter = str2;

                    (this.dgv.DataSource as DataTable).DefaultView.RowFilter = filterString;

                }
                catch (Exception ex)
                {
                    this.dgv.DataSource = (object)this.table; //this.dgv.DataSource = (object) this.table;Text
                }
            }
        }

        private void dgv_SortStringChanged(object sender, AdvancedDataGridView.SortEventArgs e)  // Filter Shortning Function
        {
            DataView dataView = (this.dgv.DataSource as DataTable).AsDataView();
            dataView.Sort = e.SortString.Split(',')[e.SortString.Split(',').Length - 1];
            this.dgv.DataSource = (object)dataView.ToTable();
        }

        private void dgv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)  // Row NO
        {
            using (SolidBrush b = new SolidBrush(dgv.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }


        private void dgv1_FilterStringChanged(object sender, AdvancedDataGridView.FilterEventArgs e) // Filter Shortning Function
        {
            string filterString = e.FilterString;
            if (e.Cancel || filterString == null)
                return;
            if (filterString.Equals(""))
            {
                (this.dgv1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
            else
            {
                try
                {
                    string str1 = filterString.Substring(1, filterString.IndexOf(']'));
                    string[] strArray = filterString.Substring(str1.Length + 6, filterString.Length - 2 - (str1.Length + 6)).Split(',');
                    string str2 = string.Format(str1 + "= '{0}'", (object)strArray[0].Substring(1, strArray[0].Length - 2));
                    if (strArray.Length > 1)
                    {
                        for (int index = 1; index < strArray.Length; ++index)
                            str2 += string.Format(" OR " + str1 + "= '{0}'", (object)strArray[index].Substring(2, strArray[index].Length - 3));
                    }
                    //(this.dgv.DataSource as DataTable).DefaultView.RowFilter = str2;

                    (this.dgv1.DataSource as DataTable).DefaultView.RowFilter = filterString;

                }
                catch (Exception ex)
                {
                    this.dgv1.DataSource = (object)this.table; //this.dgv.DataSource = (object) this.table;Text
                }
            }
        }

        private void dgv1_SortStringChanged(object sender, AdvancedDataGridView.SortEventArgs e)  // Filter Shortning Function
        {
            DataView dataView = (this.dgv1.DataSource as DataTable).AsDataView();
            dataView.Sort = e.SortString.Split(',')[e.SortString.Split(',').Length - 1];
            this.dgv1.DataSource = (object)dataView.ToTable();
        }

        private void dgv1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgv.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }


        private void drwList_SelectedIndexChanged(object sender, EventArgs e) // List View To Data Grid Data Transfer
        {
            frmWaitForm fui = new frmWaitForm();
            Thread t = new Thread(() => showCircularProgress(fui));
            t.Start();
            Thread.Sleep(10);
            //this.statusStrip.Text = "Loading List. Please Wait...";  //Show Messge on Botom
            Cursor.Current = Cursors.WaitCursor;
            this.table = (DataTable)null;
            if (pipeLineListView.SelectedItems.Count > 0)
            {
                List<String> lstSelectedItems = new List<string>();
                foreach (ListViewItem listViewItem in pipeLineListView.SelectedItems)
                {
                    lstSelectedItems.Add(listViewItem.Text);
                }
                string items = "'" + String.Join("','", lstSelectedItems) + "'";

                if (this.treeViewSelected == TreeviewTypes.System)
                    dgv.DataSource = (new MainFormDBAccess(this.connectionString)).populateGVSystem(items);
                else if (this.treeViewSelected == TreeviewTypes.WBS)
                    dgv.DataSource = (new MainFormDBAccess(this.connectionString)).populateGVWBS(items);
                else if (this.treeViewSelected == TreeviewTypes.DrawReport)
                {
                    if (!drawingViewToolStripMenuItem1.Checked)
                    {
                        dgv.DataSource = (new MainFormDBAccess(this.connectionString)).populateGVDrawingRpt(items);
                        //pictureBox1.Visible = false;
                        axAcroPDF1.Visible = false;
                    }
                    else
                    {
                        
                        DataTable dt = (new MainFormDBAccess(this.connectionString)).populateBlobData(items);
                        File.WriteAllBytes("output.zip", (byte[])dt.Rows[0][2]);
                        //pictureBox1.Visible = true;
                        //axAcroPDF1.Visible = true;
                        showFileContents();
                    }
                        
                }
                  

                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;
                if (dgv.Columns.Count > 0)
                    dgv.Columns[0].HeaderCell.Style.BackColor = Color.Yellow;
            }
            fui.shutDownThread = true;
        }

        private void dgv_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.cms_Object.Show(this.dgv, e.Location);
                this.cms_Object.Show(Cursor.Position);

            }
        }

        private void showFileContents()
        {
            try
            {
                using (ZipArchive zipFile = ZipFile.OpenRead("output.zip"))
                {
                    switch (zipFile.Entries.FirstOrDefault().FullName.Split('.').LastOrDefault())
                    {
                        case "dwg":
                            ZipArchiveEntry entry_3 = zipFile.Entries.
                                Where(x => x.FullName.Equals
                                (zipFile.Entries.FirstOrDefault().FullName)).FirstOrDefault();
                            entry_3.ExtractToFile(entry_3.FullName, true);

                            DwgToPdf.Run(entry_3.FullName);
                            bool PDFAvailable = axAcroPDF1.LoadFile("output.pdf");

                            if (PDFAvailable == true)
                            {
                                axAcroPDF1.Visible = true;
                                axAcroPDF1.LoadFile("output.pdf");
                                axAcroPDF1.setShowToolbar(false); //disable pdf toolbar.
                                axAcroPDF1.Enabled = true;
                                dgv.Visible = false;
                                //pictureBox1.Visible = false;
                                axAcroPDF1.Visible = true;


                            }
                            else
                            {
                                MessageBox.Show("Selected PDF Template Is Locked By Another Application.", "Test Application", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            //axAcroPDF1.LoadFile("output.pdf");
                            //axAcroPDF1.src = "output.pdf";
                           
                            break;
                        case "jpg":
                        case "jpeg":

                            ZipArchiveEntry entry = zipFile.Entries.
                                Where(x => x.FullName.Equals(
                                    zipFile.Entries.FirstOrDefault().FullName)).FirstOrDefault();
                            entry.ExtractToFile(entry.FullName, true);

                            Image image = Image.FromStream(entry.Open());
                            //pictureBox1.Image = image;

                            dgv.Visible = false;
                           // pictureBox1.Visible = true;
                            axAcroPDF1.Visible = false;
                            break;

                        case "xls":
                        case "xlsx":

                            ZipArchiveEntry entry_2 = zipFile.Entries.
                                Where(x => x.FullName.Equals(
                                    zipFile.Entries.FirstOrDefault().FullName)).FirstOrDefault();

                            dgv.DataSource = null;
                            entry_2.ExtractToFile(entry_2.FullName, true);

                            //Get All Sheets
                            string strConnString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + entry_2.FullName + ";Extended Properties=Excel 8.0;";

                            DbProviderFactory objDbFactory = DbProviderFactories.GetFactory("System.Data.OleDb");

                            DbDataAdapter objDbAdapter = null;

                            objDbAdapter = objDbFactory.CreateDataAdapter();

                            DbConnection objDbConnection = objDbFactory.CreateConnection();

                            objDbConnection.ConnectionString = strConnString;

                            objDbConnection.Open();

                            DataTable objSheetNames = objDbConnection.GetSchema("Tables");


                            //Show first sheet in the grid view by default
                            String name = "JS Automation Report";
                            String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                            entry_2.FullName +
                                            ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

                            OleDbConnection con = new OleDbConnection(constr);
                            OleDbCommand oconn = new OleDbCommand("Select * From [" + objSheetNames.Rows[0]["TABLE_NAME"] + "]", con);
                            con.Open();

                            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                            DataTable data = new DataTable();
                            sda.Fill(data);
                            dgv.DataSource = data;

                            dgv.Visible = true;
                            //pictureBox1.Visible = false;
                            axAcroPDF1.Visible = false;
                            break;

                    }
                }
            }
            catch (Exception exception)
            {
            }


        }

        private void modify_cms_Pipeline_Click(object sender, EventArgs e)
        {
            Dictionary<string, PersistenceLayer.PropertyUpdateDB.UpdateStatusFields> dctSelectedRows = new
            Dictionary<string, PersistenceLayer.PropertyUpdateDB.UpdateStatusFields>();
            foreach (DataGridViewRow selectedRow in dgv.SelectedRows)
            {
                if (!dctSelectedRows.ContainsKey(selectedRow.Cells[8].Value.ToString()))
                {
                    PersistenceLayer.PropertyUpdateDB.UpdateStatusFields updateStatusFields = new PersistenceLayer.PropertyUpdateDB.UpdateStatusFields();
                    updateStatusFields.PipeLine = selectedRow.Cells[0].Value.ToString();
                    updateStatusFields.PipeRun = selectedRow.Cells[1].Value.ToString();
                    updateStatusFields.PipeLine_OID = selectedRow.Cells[8].Value.ToString();
                    dctSelectedRows.Add(selectedRow.Cells[8].Value.ToString(),
                    updateStatusFields);
                }
            }
            UpdateProperty myForm = new UpdateProperty(dctSelectedRows);
            myForm.ShowDialog();
        }

        private void modify_cms_PipeRun_Click(object sender, EventArgs e)
        {
            Dictionary<string, PersistenceLayer.PropertyUpdateDB.UpdateStatusFields> dctSelectedRows = new
            Dictionary<string, PersistenceLayer.PropertyUpdateDB.UpdateStatusFields>();
            foreach (DataGridViewRow selectedRow in dgv.SelectedRows)
            {
                if (!dctSelectedRows.ContainsKey(selectedRow.Cells[9].Value.ToString()))
                {
                    PersistenceLayer.PropertyUpdateDB.UpdateStatusFields updateStatusFields = new PersistenceLayer.PropertyUpdateDB.UpdateStatusFields();
                    updateStatusFields.PipeLine = selectedRow.Cells[0].Value.ToString();
                    updateStatusFields.PipeRun = selectedRow.Cells[1].Value.ToString();
                    updateStatusFields.PipeLine_OID = selectedRow.Cells[9].Value.ToString();
                    dctSelectedRows.Add(selectedRow.Cells[9].Value.ToString(),
                    updateStatusFields);
                }
            }
            UpdateProperty myForm = new UpdateProperty(dctSelectedRows);
            myForm.ShowDialog();
        }

        public void modify_cms_Status_Click(object sender, EventArgs e)
        {
            Dictionary<string, PersistenceLayer.PropertyUpdateDB.UpdateStatusFields> dctSelectedRows = new
            Dictionary<string, PersistenceLayer.PropertyUpdateDB.UpdateStatusFields>();
            foreach (DataGridViewRow selectedRow in dgv.SelectedRows)
            {
                if (!dctSelectedRows.ContainsKey(selectedRow.Cells[5].Value.ToString()))
                {
                    PersistenceLayer.PropertyUpdateDB.UpdateStatusFields updateStatusFields = new PersistenceLayer.PropertyUpdateDB.UpdateStatusFields();
                    updateStatusFields.PipeLine = selectedRow.Cells[0].Value.ToString();
                    updateStatusFields.PipeRun = selectedRow.Cells[1].Value.ToString();
                    updateStatusFields.PipeLine_OID = selectedRow.Cells[5].Value.ToString();
                    dctSelectedRows.Add(selectedRow.Cells[5].Value.ToString(),
                    updateStatusFields);
                }
            }
            UpdateProperty myForm = new UpdateProperty(dctSelectedRows);
            myForm.ShowDialog();
        }
        //Report Functions and methode 

        private void ToCsV(DataGridView dGV, string filename) // Function For Export Report From DGV
        {
            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";
            // Export data.
            for (int i = 0; i < dGV.RowCount - 1; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine + "\r\n";
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        public void showMyReportToolStripMenuItem_Click(object sender, EventArgs e)  // Button Click Envent For Export Report From DGV
        {
            if (this.attributeConfigurationToolStripMenuItem.Checked)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                sfd.FileName = "JS Automation Report.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //ToCsV(dataGridView1, @"c:\export.xls");
                    ToCsV(dgv1, sfd.FileName);
                }

                MessageBox.Show("Report Successfully Generated");
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                sfd.FileName = "JS Automation Report.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //ToCsV(dataGridView1, @"c:\export.xls");
                    ToCsV(dgv, sfd.FileName);
                }

                MessageBox.Show("Report Successfully Generated");
            } 
            
            
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Excel Documents (*.xls)|*.xls";
            //sfd.FileName = "JS Automation Report.xls";
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    //ToCsV(dataGridView1, @"c:\export.xls");
            //    ToCsV(dgv, sfd.FileName);
            //}

            //MessageBox.Show("Report Successfully Generated");
        }

        private void tDLReport_Click(object sender, EventArgs e)
        {
            frmWaitForm fui = new frmWaitForm();

            DataTable piperunreport = new DataTable();
            piperunreport = DatabaseHelper.lsttosql();
            foreach (DataRow drs in piperunreport.Rows)
            {
                dgv.DataSource = DatabaseHelper.GetPiperunReport();
            }
            for (int i = 0; i > 1; i++)
            {
                fui.ShowDialog();
            }

            fui.Close();
        }

        private void pipeLineStatusReport_Click(object sender, EventArgs e)
        {

        }

        private void pipingMTO_Click(object sender, EventArgs e)
        {

        }

        private void weldReport_Click(object sender, EventArgs e)
        {

        }

        private void clashReport_Click(object sender, EventArgs e)
        {

        }

        private void isometricStatusReport_Click(object sender, EventArgs e)
        {

        }

        private void gAStatusReport_Click(object sender, EventArgs e)
        {

        }

        private void structureMTO_Click(object sender, EventArgs e)
        {

        }

        private void foundationMTO_Click(object sender, EventArgs e)
        {
            frmWaitForm fui = new frmWaitForm();
            fui.ShowDialog();
        }

        private void importExcelReportForLineLockingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportLockData ild = new ImportLockData();
            ild.ShowDialog();
        }

        private void RunReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportSelector reportSelector = new ReportSelector();
            DialogResult dialogResult = reportSelector.ShowDialog(this);
            if (dialogResult != DialogResult.OK) return;

            ReportSelectorDB.ReportSelectorData reportSelectorData =
                (ReportSelectorDB.ReportSelectorData)reportSelector.getComboBox.SelectedItem;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
                "Server=XENON;Integrated Security=SSPI; Initial Catalog=" + reportSelectorData.DBName);
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            Cursor.Current = Cursors.WaitCursor;
            this.table = (DataTable)null;

            DataTable plprdt = new DataTable();
            SqlCommand cmd = new SqlCommand(reportSelectorData.Query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            plprdt.Load(reader);
            dgv.DataSource = plprdt;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;
            dgv.Columns[0].HeaderCell.Style.BackColor = Color.Yellow;

        }

        // START        Drag & Drop Functionality for S3D Class Tree
        private void tv_Catalog_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tv_Catalog.DoDragDrop(e.Item, DragDropEffects.Move);
            string txt;
            txt = null;
            txt += e.Item.ToString();
            pipeLineListView.Text = txt;
        }

        private void tv_S3DClassconfig_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
        }

        private void tv_S3DClassconfig_DragDrop(object sender, DragEventArgs e)
        {
            frmWaitForm fui = new frmWaitForm();
            try
            {
                if (tv_S3DClassconfig.Nodes.Count > 0)
                {
                    TreeNode newNode;
                    if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
                    {
                        Point pt;
                        TreeNode destinationNode;
                        pt = tv_S3DClassconfig.PointToClient(new Point(e.X, e.Y));
                        destinationNode = tv_S3DClassconfig.GetNodeAt(pt);
                        if (destinationNode != null)
                        {
                            newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                            if (newNode.Text.Equals("Name") && newNode.Parent.Text.ToLower().Equals("jnameditem"))
                            {
                                newNode.Text = "ItemName";
                            }
                            TreeNode interfaceParentNode = destinationNode;
                            while (interfaceParentNode.Parent != null)
                            {
                                interfaceParentNode = interfaceParentNode.Parent;
                            }


                            if (((CatalogTreeNodeType)newNode.Tag == CatalogTreeNodeType.Attribute//Interface
                                && interfaceParentNode.Text.Equals(S3DClassConfig_DefaultNodes_Interface)
                                //&& interfaceParentNode.Nodes.Find(newNode.Text, true).Length == 0
                                )
                                ||
                                ((CatalogTreeNodeType)newNode.Tag == CatalogTreeNodeType.Package//Attribute
                                && destinationNode.Text.Equals(S3DClassConfig_DefaultNodes_Attribute)
                                && 
                                (destinationNode.Nodes.Find(newNode.Text, true).Length == 0 || newNode.Text.Equals("ItemName"))
                                
                                ))
                            {

                                if ((CatalogTreeNodeType)newNode.Tag == CatalogTreeNodeType.Attribute) //Interface
                                {
                                    if (!AddedInterfaces.Any(x => x.Name == newNode.Text && x.ParentName == newNode.Parent.Text))
                                    {
                                        AddedInterfaces entity = new AddedInterfaces();
                                        entity.Name = newNode.Text;
                                        entity.ParentName = newNode.Parent.Text;
                                        entity.Aliasing = "x" + ++aliasingCounter;

                                        AddedInterfaces.Add(entity);
                                    }
                                    else // To avoid duplicates of same nodes
                                    {
                                        //return;
                                    }
                                }
                                if ((CatalogTreeNodeType)newNode.Tag == CatalogTreeNodeType.Package) //Attribute
                                {
                                    string parentNodeText = newNode.Parent.Text;
                                    string secondLayerParent = newNode.Parent.Parent.Text;
                                    string aliasedName = "";
                                    if (AddedInterfaces.Any(x => x.Name == parentNodeText && x.ParentName == secondLayerParent))
                                    {
                                        aliasedName = AddedInterfaces.FirstOrDefault(x => x.Name == parentNodeText && x.ParentName == secondLayerParent).Aliasing + "." + newNode.Text;

                                        if (!mAddedAttributes.Any(x => x.Contains(aliasedName)))
                                        {
                                            mAddedAttributes.Add(aliasedName);
                                        }
                                        else // To avoid duplicates of same nodes
                                        {
                                            //return;
                                        }
                                    }

                                }


                                if ((CatalogTreeNodeType)newNode.Tag == CatalogTreeNodeType.Package)//Attribute)
                                {
                                    bool attributeFound = false;

                                    List<TreeNode> lstTreeNodes = new List<TreeNode>();
                                    getNodesRecursive(this.tv_S3DClassconfig.
                                        Nodes[S3DClassConfig_DefaultNodes_Interface], lstTreeNodes);



                                    foreach (TreeNode treeNode in lstTreeNodes)
                                    {
                                        TreeNodeCollection childrenNodes = (TreeNodeCollection)treeNode.Tag;

                                        foreach (TreeNode childNode in childrenNodes)
                                        {
                                            if (childNode.Text.Equals(newNode.Text))
                                            {
                                                attributeFound = true; break;
                                            }
                                        }
                                        if (attributeFound) break;
                                    }

                                    if (!attributeFound) return;
                                }

                                TreeNode addedNode = destinationNode.Nodes.Add(newNode.Text, newNode.Text);
                                if ((CatalogTreeNodeType)newNode.Tag == CatalogTreeNodeType.Attribute)//Interface)
                                {
                                    addedNode.Tag = newNode.Nodes;
                                }
                                if ((CatalogTreeNodeType)newNode.Tag == CatalogTreeNodeType.Package)//Attribute)
                                {
                                    addedNode.Tag = newNode.Parent.Text;
                                }
                                destinationNode.Expand();

                            }
                        }
                    }


                    if (this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface].Nodes.Count > 0)
                    {
                        Dictionary<String, CatalogTreeViewDragDropUtility> dictTreeNodes
                            = new Dictionary<String, CatalogTreeViewDragDropUtility>();

                        getNodesRecursive_2(this.tv_S3DClassconfig.
                            Nodes[S3DClassConfig_DefaultNodes_Interface], dictTreeNodes);

                        String catalogTreeViewQuery_1 = this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface].
                            Nodes[0].Text + " AS V_0 \n";

                        int outerLevelTreeNodeCounter = 1;
                        for (;
                            outerLevelTreeNodeCounter < this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface].Nodes.Count;
                            outerLevelTreeNodeCounter++)
                        {

                            string srcField = dictTreeNodes.
                                ElementAt(0).Value.doesStartWithX ? "OidOrigin" : "Oid";
                            string trgtField = dictTreeNodes.
                                ElementAt(outerLevelTreeNodeCounter).Value.doesStartWithX ? "OidDestination" : "Oid";

                            catalogTreeViewQuery_1 += "JOIN " + this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface]
                                .Nodes[outerLevelTreeNodeCounter].Text + " AS V_" + outerLevelTreeNodeCounter +
                                " ON V_0." + srcField + " = V_" + outerLevelTreeNodeCounter + ".Oid \n";

                            dictTreeNodes[this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface]
                                .Nodes[outerLevelTreeNodeCounter].Text].tableOrder = outerLevelTreeNodeCounter;

                        }

                        int tableCounter = outerLevelTreeNodeCounter;


                        for (int interfaceCounter = 0;
                            interfaceCounter < dictTreeNodes.Count;
                            interfaceCounter++)
                        {
                            if (dictTreeNodes.ElementAt(interfaceCounter).Value.lstTreeNodes == null) continue;

                            foreach (TreeNode tn in dictTreeNodes.ElementAt(interfaceCounter).Value.lstTreeNodes)
                            {

                                string srcField = dictTreeNodes.ElementAt(interfaceCounter).Value.doesStartWithX ? "OidOrigin" : "Oid";

                                string trgtField = tn.Text.ToLower().StartsWith("x") ? "OidDestination" : "Oid";

                                catalogTreeViewQuery_1 += "JOIN " + tn.Text + " AS V_" + tableCounter;

                                catalogTreeViewQuery_1 += " ON V_" + dictTreeNodes.ElementAt(interfaceCounter).Value.tableOrder +
                                    "." + srcField + " = V_" + tableCounter + "." + trgtField + " \n";

                                dictTreeNodes[tn.Text].tableOrder = tableCounter++;
                            }
                        }

                        string attributes = string.Empty;
                        if (this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes.Count > 0)
                        {
                            String[] attributesArray = new String
                                [this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes.Count];
                            for (int count = 0;
                                count < this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes.Count; count++)
                            {

                                if (dictTreeNodes.ContainsKey(this.tv_S3DClassconfig.
                                    Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes[count].Tag.ToString()))
                                {
                                    int index = dictTreeNodes[this.tv_S3DClassconfig.
                                    Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes[count].Tag.ToString()].tableOrder;

                                    attributesArray[count] = "V_" + index + "." + this.tv_S3DClassconfig.
                                        Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes[count].Text;
                                }
                            }
                            attributes = string.Join(",", attributesArray);
                        }
                        else attributes = "*";

                        this.catalogTreeViewQuery = "Select " + attributes + " \nfrom " + catalogTreeViewQuery_1;


                        Thread t = new Thread(() => showCircularProgress(fui));
                        t.Start();
                        Thread.Sleep(10);
                        //this.statusStrip.Text = "Loading List. Please Wait...";  //Show Messge on Botom
                        Cursor.Current = Cursors.WaitCursor;
                        this.table = (DataTable)null;

                        dgv1.DataSource = (new MainFormDBAccess(this.connectionString)).
                            populateCatalogGridView(this.catalogTreeViewQuery);

                        dgv1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;
                        dgv1.Columns[0].HeaderCell.Style.BackColor = Color.Yellow;

                        fui.shutDownThread = true;
                    }
                }
            }
            catch (Exception exception)
            {
                fui.shutDownThread = true;
            }

        }

        private void tv_S3DClassconfig_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                tv_Menu.Show(tv_S3DClassconfig, e.Location);
            }                     
        }

        private void getNodesRecursive(TreeNode oParentNode, List<TreeNode> lstTreeNodes)
        {
            // Start recursion on all subnodes.
            foreach (TreeNode oSubNode in oParentNode.Nodes)
            {
                lstTreeNodes.Add(oSubNode);
                getNodesRecursive(oSubNode, lstTreeNodes);
            }
        }

        private void getNodesRecursive_2(TreeNode oParentNode, Dictionary<String, CatalogTreeViewDragDropUtility> dictTreeNodes)
        {

            List<TreeNode> childrenTreeNodes = new List<TreeNode>();
            // Start recursion on all subnodes.
            foreach (TreeNode oSubNode in oParentNode.Nodes)
            {
                CatalogTreeViewDragDropUtility catalogTreeViewDragDropUtility = new CatalogTreeViewDragDropUtility();
                if (oSubNode.Name.ToLower().StartsWith("x"))
                    catalogTreeViewDragDropUtility.doesStartWithX = true;
                if (oSubNode.Nodes.Count > 0)
                {
                    catalogTreeViewDragDropUtility.lstTreeNodes = oSubNode.Nodes.OfType<TreeNode>().ToList();

                    if (dictTreeNodes.ContainsKey(oSubNode.Name))
                    {

                        catalogTreeViewDragDropUtility.isRepeatedInterface = true;
                        dictTreeNodes.Add(oSubNode.Name + "_x" + dictTreeNodes.Keys.Count, catalogTreeViewDragDropUtility);
                    }
                    else
                        dictTreeNodes.Add(oSubNode.Name, catalogTreeViewDragDropUtility);

                    getNodesRecursive_2(oSubNode, dictTreeNodes);
                }

                else
                {
                    if (dictTreeNodes.ContainsKey(oSubNode.Name))
                        dictTreeNodes.Add(oSubNode.Name + "_x" + dictTreeNodes.Keys.Count, catalogTreeViewDragDropUtility);
                    else
                        dictTreeNodes.Add(oSubNode.Name, catalogTreeViewDragDropUtility);
                }

            }
        }


        private void addTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tv_S3DClassconfig.Nodes.Add(S3DClassConfig_DefaultNodes_Attribute,
                S3DClassConfig_DefaultNodes_Attribute);
            tv_S3DClassconfig.Nodes.Add(S3DClassConfig_DefaultNodes_Interface,
                S3DClassConfig_DefaultNodes_Interface);
        }

        private void removeTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode interfaceParentNode = tv_S3DClassconfig.SelectedNode;


            while (interfaceParentNode.Parent != null)
            {
                interfaceParentNode = interfaceParentNode.Parent;
            }
            if(interfaceParentNode.Text.Equals(S3DClassConfig_DefaultNodes_Interface))
            {
                TreeNodeCollection attributeCollection = (TreeNodeCollection)tv_S3DClassconfig.SelectedNode.Tag;

                foreach(TreeNode treenode in attributeCollection)
                {
                    TreeNode node = this.tv_S3DClassconfig
                        .Nodes[S3DClassConfig_DefaultNodes_Attribute]
                        .Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Name == treenode.Text);
                    if (node != null)
                    {
                        this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes.Remove(node);
                    }
                    
                }
            }
            tv_S3DClassconfig.SelectedNode.Remove();

            frmWaitForm fui = new frmWaitForm();
            try
            {
                if (this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface] != null)
                {
                    if (this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface].Nodes.Count > 0)
                    {
                        Dictionary<String, CatalogTreeViewDragDropUtility> dictTreeNodes
                            = new Dictionary<String, CatalogTreeViewDragDropUtility>();

                        getNodesRecursive_2(this.tv_S3DClassconfig.
                            Nodes[S3DClassConfig_DefaultNodes_Interface], dictTreeNodes);





                        String catalogTreeViewQuery_1 = this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface].
                            Nodes[0].Text + " AS V_0 \n";

                        int outerLevelTreeNodeCounter = 1;
                        for (;
                            outerLevelTreeNodeCounter < this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface].Nodes.Count;
                            outerLevelTreeNodeCounter++)
                        {
                            catalogTreeViewQuery_1 += "JOIN " + this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface]
                                .Nodes[outerLevelTreeNodeCounter].Text + " AS V_" + outerLevelTreeNodeCounter +
                                " ON V_0.Oid = V_" + outerLevelTreeNodeCounter + ".Oid \n";

                            dictTreeNodes[this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Interface]
                                .Nodes[outerLevelTreeNodeCounter].Text].tableOrder = outerLevelTreeNodeCounter;

                        }

                        int tableCounter = outerLevelTreeNodeCounter;


                        for (int interfaceCounter = 0;
                            interfaceCounter < dictTreeNodes.Count;
                            interfaceCounter++)
                        {
                            if (dictTreeNodes.ElementAt(interfaceCounter).Value.lstTreeNodes == null) continue;

                            foreach (TreeNode tn in dictTreeNodes.ElementAt(interfaceCounter).Value.lstTreeNodes)
                            {
                                catalogTreeViewQuery_1 += " JOIN " + tn.Text + " AS V_" + tableCounter;

                                catalogTreeViewQuery_1 += " ON V_" + dictTreeNodes.ElementAt(interfaceCounter).Value.tableOrder +
                                    ".Oid = V_" + tableCounter + ".Oid \n";

                                dictTreeNodes[tn.Text].tableOrder = tableCounter++;
                            }
                        }

                        string attributes = string.Empty;
                        if (this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes.Count > 0)
                        {
                            String[] attributesArray = new String
                                [this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes.Count];
                            for (int count = 0;
                                count < this.tv_S3DClassconfig.Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes.Count; count++)
                            {

                                if (dictTreeNodes.ContainsKey(this.tv_S3DClassconfig.
                                    Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes[count].Tag.ToString()))
                                {
                                    int index = dictTreeNodes[this.tv_S3DClassconfig.
                                    Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes[count].Tag.ToString()].tableOrder;

                                    attributesArray[count] = "V_" + index + "." + this.tv_S3DClassconfig.
                                        Nodes[S3DClassConfig_DefaultNodes_Attribute].Nodes[count].Text;
                                }
                            }
                            attributes = string.Join(",", attributesArray);
                        }
                        else attributes = "*";

                        this.catalogTreeViewQuery = "Select " + attributes + " \n from " + catalogTreeViewQuery_1;




                        Thread t = new Thread(() => showCircularProgress(fui));
                        t.Start();
                        Thread.Sleep(10);
                        //this.statusStrip.Text = "Loading List. Please Wait...";  //Show Messge on Botom
                        Cursor.Current = Cursors.WaitCursor;
                        this.table = (DataTable)null;

                        dgv1.DataSource = (new MainFormDBAccess(this.connectionString)).
                            populateCatalogGridView(this.catalogTreeViewQuery);

                        dgv1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;
                        dgv1.Columns[0].HeaderCell.Style.BackColor = Color.Yellow;

                        fui.shutDownThread = true;
                    }
                    else
                        dgv1.DataSource = null;
                }
                else
                    dgv1.DataSource = null;

            }
            catch (Exception ex)
            {
                fui.shutDownThread = true;
            }
        }

        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tv_S3DClassconfig.Nodes.Clear();
            dgv.DataSource = null;
            dgv1.DataSource = null;
        }

        private void queryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryEditor qryEditor = new QueryEditor(catalogTreeViewQuery);
            DialogResult dialogResult = qryEditor.ShowDialog();
            if (dialogResult == DialogResult.Retry)
            {
                this.catalogTreeViewQuery = qryEditor.getModifiedQuery;

                frmWaitForm fui = new frmWaitForm();
                try
                {
                    Thread t = new Thread(() => showCircularProgress(fui));
                    t.Start();
                    Thread.Sleep(10);
                    //this.statusStrip.Text = "Loading List. Please Wait...";  //Show Messge on Botom
                    Cursor.Current = Cursors.WaitCursor;
                    this.table = (DataTable)null;

                    dgv1.DataSource = (new MainFormDBAccess(this.connectionString)).
                        populateCatalogGridView(this.catalogTreeViewQuery);

                    dgv1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;
                    dgv1.Columns[0].HeaderCell.Style.BackColor = Color.Yellow;
                    fui.shutDownThread = true;
                }
                catch (Exception exception)
                {
                    fui.shutDownThread = true;
                }
            }
            else if (dialogResult == DialogResult.Yes)
            {
                var queryToBeSaved = new QueryToBeSaved { queryID = Guid.NewGuid(), query = this.catalogTreeViewQuery };
                var jsonString = (new JavaScriptSerializer()).Serialize(queryToBeSaved);
                System.IO.File.WriteAllText(
                    Path.GetFullPath(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, @"..\..\savedQuery.json")), jsonString);
            }


        }

        private void Tv_Catalog_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStripActiveRelationship.Show(tv_Catalog, e.Location);
            }
        }


        private void ToolStripMenuItem1_Click(object sender, EventArgs e)   // ReletionShip Tree Nodes
        {
            DataTable dtActiveRelationship = (new MainFormDBAccess(this.connectionString)).populateActiveRelationship();

            foreach (DataRow dataRow in dtActiveRelationship.Rows)
            {
                var parentTreeNode = this.tv_Catalog.Nodes.Cast<TreeNode>().Where(r => r.Text == dataRow[0].ToString()).FirstOrDefault();
                if (parentTreeNode == null)
                {
                    TreeNode attributeTreeNode_0 = parentTreeNode = new TreeNode(dataRow[0].ToString());
                    attributeTreeNode_0.Name = dataRow[0].ToString();
                    attributeTreeNode_0.ImageIndex = 0;
                    attributeTreeNode_0.SelectedImageIndex = 0;
                    attributeTreeNode_0.Tag = CatalogTreeNodeType.Class;
                    attributeTreeNode_0.ForeColor = Color.Red;

                    this.tv_Catalog.Nodes.Add(attributeTreeNode_0);
                }

                if (parentTreeNode.Nodes.Find(dataRow[1].ToString(), false).Count() == 0)
                {
                    TreeNode attributeTreeNode = parentTreeNode
                    .Nodes.Add(dataRow[1].ToString(), dataRow[1].ToString(), 1, 1);
                    attributeTreeNode.ForeColor = Color.Red;
                    attributeTreeNode.Tag = CatalogTreeNodeType.Interface;
                }

                if (parentTreeNode.Nodes[dataRow[1].ToString()].Nodes.Find(dataRow[2].ToString(), false).Count() == 0)
                {
                    TreeNode attributeTreeNode_2 = parentTreeNode
                    .Nodes[dataRow[1].ToString()].Nodes.Add(dataRow[2].ToString(), dataRow[2].ToString(), 2, 2);
                    attributeTreeNode_2.Tag = CatalogTreeNodeType.Attribute;
                    attributeTreeNode_2.ForeColor = Color.Red;
                }
            }
        }


        private void ToolStripMenuItem2_Click(object sender, EventArgs e)    // CodeList Tree Nodes
        {
            DataTable dtCodeListValues = (new MainFormDBAccess(string.Empty)).populateCodeListValues();

            foreach (DataRow dataRow in dtCodeListValues.Rows)
            {
                if (dataRow.ItemArray.ToList().Where(x => String.IsNullOrWhiteSpace(x.ToString())).Any())
                    continue;


                var parentTreeNode = this.tv_Catalog.Nodes.Cast<TreeNode>()
                    .Where(r => r.Text == dataRow[0].ToString()).FirstOrDefault();
                if (parentTreeNode == null)
                {
                    continue;
                    TreeNode attributeTreeNode_0 = parentTreeNode = new TreeNode(dataRow[0].ToString());
                    attributeTreeNode_0.Name = dataRow[0].ToString();
                    attributeTreeNode_0.ImageIndex = 0;
                    attributeTreeNode_0.SelectedImageIndex = 0;
                    attributeTreeNode_0.Tag = CatalogTreeNodeType.Class;
                    attributeTreeNode_0.ForeColor = Color.Blue;

                    this.tv_Catalog.Nodes.Add(attributeTreeNode_0);
                }

                TreeNode node_l1 = parentTreeNode.Nodes.Cast<TreeNode>()
                    .Where(r => r.Text == dataRow[1].ToString()).ToArray().FirstOrDefault();

                if (node_l1 == null) continue;
                //if (parentTreeNode.Nodes.Find(dataRow[1].ToString(), false).Count() == 0)
                //{
                //    TreeNode attributeTreeNode = parentTreeNode
                //    .Nodes.Add(dataRow[1].ToString(), dataRow[1].ToString(), 1, 1);
                //    attributeTreeNode.ForeColor = Color.Blue;
                //    attributeTreeNode.Tag = CatalogTreeNodeType.Interface;
                //}

                TreeNode node_l2 = node_l1.Nodes.Cast<TreeNode>()
                    .Where(r => r.Text == dataRow[2].ToString()).ToArray().FirstOrDefault();

                if (node_l2 == null)                    
                {
                    node_l2 = node_l1
                    .Nodes.Add(dataRow[2].ToString(), dataRow[2].ToString(), 2, 2);
                    node_l2.Tag = CatalogTreeNodeType.Attribute;
                    node_l2.ForeColor = Color.Blue;
                }

                TreeNode node_l3 = node_l2.Nodes.Cast<TreeNode>()
                    .Where(r => r.Text == dataRow[3].ToString()).ToArray().FirstOrDefault();

                if (node_l3 == null)
                {
                    node_l3 = node_l2
                    .Nodes.Add(dataRow[3].ToString(), dataRow[3].ToString(), 3, 3);
                    node_l3.Tag = CatalogTreeNodeType.Package;
                    node_l3.ForeColor = Color.Blue;
                }
            }
        }

        private void DrawingViewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.drawingViewToolStripMenuItem1.Checked )
            {
                //this.pipeLineListView.Visible = false;
                //this.dgv1.Visible = false;
                axAcroPDF1.Visible = true;
                
            }
            else
            {
              // this. pipeLineListView.Visible = true;
                this.dgv1.Visible = true;
            }
        }

        private void PipeLineListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.drawingViewToolStripMenuItem1.Checked)
            {
                this.exportMenuStrip.Show(pipeLineListView, e.Location);
                this.toolStripMenuItem3.Tag = e.Location;
            }
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    Point point = (Point)((ToolStripMenuItem)sender).Tag;
                    ListViewItem item = pipeLineListView.GetItemAt(point.X, point.Y);
                        

                    DataTable dt = (new MainFormDBAccess(this.connectionString)).
                        populateBlobData("'" + item.Text + "'");
                    File.WriteAllBytes(folderBrowserDialog.SelectedPath+"/" + item.Text + ".zip", (byte[])dt.Rows[0][2]);
                }
            }
        }
    }

    public class CatalogTreeViewDragDropUtility
    {
        public List<TreeNode> lstTreeNodes;
        public int tableOrder;
        public Boolean isRepeatedInterface;
        public Boolean doesStartWithX;
    }

    public class AddedInterfaces
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string Aliasing { get; set; }
    }
}



