using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;

namespace JSDataRef
{
    public class J3D1
    {
        private SqlConnection CDB_SchemaConnection;

        public string ServerName { get; set; }

        public string CatalogDB { get; set; }

        public string CatalogSchemaDB { get; set; }

        public string SiteDB { get; set; }

        public string SiteSchemaDB { get; set; }

        public string ModelDB { get; set; }

        public string ReportDB { get; set; }

        public string ReportSchemaDB { get; set; }

        public J3DClassDefs CDBClassDefs { get; set; }

        public void Connect()
        {
            this.CDBSchemaConnection();
            this.CDBClassDef1();
            // this.CDBClassDef1();
        }

        public void DisConnect()
        {
            if (this.CDB_SchemaConnection == null)
                return;
            this.CDB_SchemaConnection.Close();
        }

        private void CDBSchemaConnection()
        {
            string connectionString = "Data Source=" + this.ServerName + "; Initial Catalog=" + this.CatalogSchemaDB + ";integrated security=SSPI;";
            try
            {
                this.CDB_SchemaConnection = new SqlConnection(connectionString);
                this.CDB_SchemaConnection.Open();
            }
            catch
            {
            }
        }

        //private void CDBClassDef()
        //{
        //    Hashtable hashtable1 = new Hashtable();
        //    Hashtable hashtable2 = new Hashtable();
        //    Hashtable hashtable3 = new Hashtable();
        //    J3DInterfaceDefs J3DinterfaceDefs1 = new J3DInterfaceDefs();
        //    int num = -1;
        //    this.CDBClassDefs = new J3DClassDefs();
        //    if (this.CDB_SchemaConnection == null)
        //        return;
        //    SqlDataReader sqlDataReader = new SqlCommand("SELECT Package.oid,PackageName.Username,ClassDef.oid ,ClassName.UserName,x1.oid ,x1.DBViewName, InterfaceMember.oid, MemberName.Name " +
        //            "FROM  IJPackage Package " +
        //            "Inner Join JNamedObjectView PackageName on PackageName.oid=Package.oid " +
        //            "Inner Join JPackage_has_JMembers jpm on jpm.oidOrg=Package.oid " +
        //            "Inner Join dbo.IJClassDef ClassDef on ClassDef.oid=jpm.oidDst " +
        //            "Inner Join dbo.IJNamedObject ClassName on ClassDef.oid = ClassName.oid  " +
        //            "Inner Join dbo.JClass_Implements_JInterfaces CImplIn on ClassDef.oid = CImplIn.oidOrg " +
        //            "Inner Join dbo.IJInterfaceDef InterfaceDef on CImplIn.oidDst = InterfaceDef.oid " +
        //            "Inner Join dbo.IJNamedObject InterfaceName on InterfaceDef.oid = InterfaceName.oid " +
        //            "Inner Join dbo.JInterface_Has_JMembers InHasMem on InterfaceDef.oid = InHasMem.oidOrg  " +
        //            "Inner Join dbo.IJInterfaceMember InterfaceMember on InHasMem.oidDst = InterfaceMember.oid  " +
        //            "Inner Join dbo.IJNamedObject MemberName on InterfaceMember.oid = MemberName.oid " +
        //            "Inner Join dbo.IJDBView x1 on x1.oid=InterfaceDef.oid " +
        //            "Order by PackageName.Username,ClassName.UserName,x1.DBViewName ", this.CDB_SchemaConnection).ExecuteReader();
        //    if (sqlDataReader.HasRows)
        //    {
        //        while (sqlDataReader.Read())
        //        {
        //            try
        //            {
        //                J3DClassDef classDef = new J3DClassDef();
        //                classDef.oid = sqlDataReader.GetValue(0).ToString();
        //                classDef.Name = sqlDataReader.GetValue(1).ToString();

        //                J3DInterfaceDef interfaceDef = new J3DInterfaceDef();
        //                interfaceDef.oid = sqlDataReader.GetValue(2).ToString();
        //                interfaceDef.Name = sqlDataReader.GetValue(3).ToString();

        //                J3DMemberDef memberDef = new J3DMemberDef();
        //                memberDef.oid = sqlDataReader.GetValue(4).ToString();
        //                memberDef.Name = sqlDataReader.GetValue(5).ToString();

        //                J3DPackageDef packageDef = new J3DPackageDef();
        //                packageDef.oid = sqlDataReader.GetValue(6).ToString();
        //                packageDef.Name = sqlDataReader.GetValue(7).ToString();

        //                int index1;
        //                J3DInterfaceDefs J3DinterfaceDefs2;
        //                if (this.CDBClassDefs.Count > 0)
        //                {
        //                    index1 = this.CDBClassDefs.IndexOf(classDef);
        //                    if (index1 < 0)
        //                    {
        //                        index1 = this.CDBClassDefs.Add(classDef);
        //                        hashtable1.Add((object)classDef.oid, (object)index1);
        //                        J3DinterfaceDefs2 = new J3DInterfaceDefs();
        //                    }
        //                    else
        //                        J3DinterfaceDefs2 = this.CDBClassDefs[index1].InterfaceDefs;
        //                }
        //                else
        //                {
        //                    J3DinterfaceDefs2 = new J3DInterfaceDefs();
        //                    index1 = this.CDBClassDefs.Add(classDef);
        //                    hashtable1.Add((object)classDef.oid, (object)index1);
        //                    num = 0;
        //                }

        //                int index2;
        //                J3DMemberDefs J3DmemberDefs;
        //                if (J3DinterfaceDefs2.Count > 0)
        //                {
        //                    LinkedList<string> linkedList = (LinkedList<string>)hashtable2[(object)classDef.oid];
        //                    index2 = J3DinterfaceDefs2.IndexOf(interfaceDef);
        //                    if (index2 < 0)
        //                    {
        //                        J3DmemberDefs = new J3DMemberDefs();
        //                        index2 = J3DinterfaceDefs2.Add(interfaceDef);
        //                    }
        //                    else
        //                        J3DmemberDefs = J3DinterfaceDefs2[index2].MemberDefs;
        //                }
        //                else
        //                {
        //                    J3DmemberDefs = new J3DMemberDefs();
        //                    index2 = J3DinterfaceDefs2.Add(interfaceDef);
        //                }


        //                //from here
        //                int index3;
        //                J3DPackageDefs J3DpackageDefs;
        //                if (J3DmemberDefs.Count > 0)
        //                {
        //                    // LinkedList<string> linkedList = (LinkedList<string>)hashtable2[(object)classDef.oid];
        //                    index3 = J3DmemberDefs.IndexOf(memberDef);
        //                    if (index3 < 0)
        //                    {
        //                        J3DpackageDefs = new J3DPackageDefs();
        //                        index3 = J3DmemberDefs.Add(memberDef);
        //                    }
        //                    else
        //                        J3DpackageDefs = J3DmemberDefs[index3].PackageDefs;
        //                }
        //                else
        //                {
        //                    J3DpackageDefs = new J3DPackageDefs();
        //                    index3 = J3DmemberDefs.Add(memberDef);
        //                }

        //                J3DpackageDefs.Add(packageDef);
        //                //  J3DmemberDefs.Add(memberDef);
        //                interfaceDef.MemberDefs = J3DmemberDefs;
        //                memberDef.PackageDefs = J3DpackageDefs;
        //                J3DinterfaceDefs2[index2] = interfaceDef;
        //                J3DmemberDefs[index3] = memberDef;
        //                classDef.InterfaceDefs = J3DinterfaceDefs2;
        //                this.CDBClassDefs[index1] = classDef;
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    sqlDataReader.Close();
        //}

        private void CDBClassDef1()
        {
            Hashtable hashtable1 = new Hashtable();
            Hashtable hashtable2 = new Hashtable();
            Hashtable hashtable3 = new Hashtable();
            J3DInterfaceDefs J3DinterfaceDefs1 = new J3DInterfaceDefs();
            int num = -1;
            this.CDBClassDefs = new J3DClassDefs();
            if (this.CDB_SchemaConnection == null)
                return;
            SqlDataReader sqlDataReader = new SqlCommand(@"SELECT PackageName.oid,PackageName.name,ClassName.oid,ClassName.Name,InterfaceName.oid,InterfaceName.Name,MemberName.oid,MemberName.Name
                                  FROM  IJPackage Package 
                                  Inner Join JNamedObjectView PackageName on PackageName.oid=Package.oid 
                                  Inner Join JPackage_has_JMembers jpm on jpm.oidOrg=Package.oid 
                                  Inner Join dbo.IJClassDef ClassDef on ClassDef.oid=jpm.oidDst 
                                  Inner Join dbo.IJNamedObject ClassName on ClassDef.oid = ClassName.oid  
                                  Inner Join dbo.JClass_Implements_JInterfaces CImplIn on ClassDef.oid = CImplIn.oidOrg 
                                  Inner Join dbo.IJInterfaceDef InterfaceDef on CImplIn.oidDst = InterfaceDef.oid 
                                  Inner Join dbo.IJNamedObject InterfaceName on InterfaceDef.oid = InterfaceName.oid 
                                  Inner Join dbo.JInterface_Has_JMembers InHasMem on InterfaceDef.oid = InHasMem.oidOrg  
                                  Inner Join dbo.IJInterfaceMember InterfaceMember on InHasMem.oidDst = InterfaceMember.oid 
                                  Inner Join dbo.IJNamedObject MemberName on InterfaceMember.oid = MemberName.oid 
                                  Inner Join dbo.IJDBView x1 on x1.oid=InterfaceDef.oid 
                                  Order by PackageName.name,ClassName.Name,InterfaceName.Name,MemberName.Name ", this.CDB_SchemaConnection).ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    try
                    {
                        J3DClassDef classDef = new J3DClassDef();
                        classDef.oid = sqlDataReader.GetValue(0).ToString();
                        classDef.Name = sqlDataReader.GetValue(1).ToString();

                        J3DInterfaceDef interfaceDef = new J3DInterfaceDef();
                        interfaceDef.oid = sqlDataReader.GetValue(2).ToString();
                        interfaceDef.Name = sqlDataReader.GetValue(3).ToString();

                        J3DMemberDef memberDef = new J3DMemberDef();
                        memberDef.oid = sqlDataReader.GetValue(4).ToString();
                        memberDef.Name = sqlDataReader.GetValue(5).ToString();

                        J3DPackageDef packageDef = new J3DPackageDef();
                        packageDef.oid = sqlDataReader.GetValue(6).ToString();
                        packageDef.Name = sqlDataReader.GetValue(7).ToString();

                        int index1;
                        J3DInterfaceDefs J3DinterfaceDefs2;
                        if (this.CDBClassDefs.Count > 0)
                        {
                            index1 = this.CDBClassDefs.IndexOf(classDef);
                            if (index1 < 0)
                            {
                                index1 = this.CDBClassDefs.Add(classDef);
                                hashtable1.Add((object)classDef.oid, (object)index1);
                                J3DinterfaceDefs2 = new J3DInterfaceDefs();
                            }
                            else
                                J3DinterfaceDefs2 = this.CDBClassDefs[index1].InterfaceDefs;
                        }
                        else
                        {
                            J3DinterfaceDefs2 = new J3DInterfaceDefs();
                            index1 = this.CDBClassDefs.Add(classDef);
                            hashtable1.Add((object)classDef.oid, (object)index1);
                            num = 0;
                        }

                        int index2;
                        J3DMemberDefs J3DmemberDefs;
                        if (J3DinterfaceDefs2.Count > 0)
                        {
                            LinkedList<string> linkedList = (LinkedList<string>)hashtable2[(object)classDef.oid];
                            index2 = J3DinterfaceDefs2.IndexOf(interfaceDef);
                            if (index2 < 0)
                            {
                                J3DmemberDefs = new J3DMemberDefs();
                                index2 = J3DinterfaceDefs2.Add(interfaceDef);
                            }
                            else
                                J3DmemberDefs = J3DinterfaceDefs2[index2].MemberDefs;
                        }
                        else
                        {
                            J3DmemberDefs = new J3DMemberDefs();
                            index2 = J3DinterfaceDefs2.Add(interfaceDef);
                        }


                        //from here
                        int index3;
                        J3DPackageDefs J3DpackageDefs;
                        if (J3DmemberDefs.Count > 0)
                        {
                            // LinkedList<string> linkedList = (LinkedList<string>)hashtable2[(object)classDef.oid];
                            index3 = J3DmemberDefs.IndexOf(memberDef);
                            if (index3 < 0)
                            {
                                J3DpackageDefs = new J3DPackageDefs();
                                index3 = J3DmemberDefs.Add(memberDef);
                            }
                            else
                                J3DpackageDefs = J3DmemberDefs[index3].PackageDefs;
                        }
                        else
                        {
                            J3DpackageDefs = new J3DPackageDefs();
                            index3 = J3DmemberDefs.Add(memberDef);
                        }

                        J3DpackageDefs.Add(packageDef);
                        //  J3DmemberDefs.Add(memberDef);
                        interfaceDef.MemberDefs = J3DmemberDefs;
                        memberDef.PackageDefs = J3DpackageDefs;
                        J3DinterfaceDefs2[index2] = interfaceDef;
                        J3DmemberDefs[index3] = memberDef;
                        classDef.InterfaceDefs = J3DinterfaceDefs2;
                        this.CDBClassDefs[index1] = classDef;
                    }
                    catch
                    {
                    }
                }
            }
            sqlDataReader.Close();
        }

    }
}
