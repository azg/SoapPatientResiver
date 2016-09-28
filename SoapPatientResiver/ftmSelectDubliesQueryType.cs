using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoapPatientResiver
{
    public partial class ftmSelectDubliesQueryType : Form
    {
        private long mquerytype;
        public ftmSelectDubliesQueryType()
        {
            InitializeComponent();
            mquerytype = -1;
        }

        private void MakeDBFClick(object sender, EventArgs e)
        {
            if(rblastName.Checked)
                mquerytype = 1;
            else if(rblastNameBirth.Checked)
                mquerytype = 2;
            else if(rb2NamesBirt.Checked)
                mquerytype = 3;
            else if(rbPussportNumb.Checked)
                mquerytype = 4;
            Close();
        }

        public long getType()
        {
            return mquerytype;
        }
    }
}
