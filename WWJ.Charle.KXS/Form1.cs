using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WWJ.Charle.KXS.DAL;

namespace WWJ.Charle.KXS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyADOProxy proxy = MyADOProxy.GetInstance(new MySqlServerHelper());

            //proxy.GetDataTable(@"select * from testtran1");
            try
            {
                proxy.BeginCurrentTran();

                proxy.MyExecuteNonQuery(@"insert into testtran1(name)values('wwj1')", null, proxy.MyCurrentTran);

                proxy.MyExecuteNonQuery(@"insert into testtran2(name2)values('wwj2')",null,proxy.MyCurrentTran);
                proxy.CommitCurrentTran();
            }
            catch (Exception ex)
            {
                proxy.RollBackCurrentTran();


            }
            finally
            {
                proxy.Dispose();

            }
         


        }
    }
}
