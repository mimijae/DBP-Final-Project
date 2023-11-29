using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBP_Final_project
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'dataSet1.BADCUSTOMERS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.bADCUSTOMERSTableAdapter.Fill(this.dataSet1.BADCUSTOMERS);
            // TODO: 이 코드는 데이터를 'dataSet1.BADCUSTOMERS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.bADCUSTOMERSTableAdapter.Fill(this.dataSet1.BADCUSTOMERS);
            // TODO: 이 코드는 데이터를 'dataSet1.GOODCUSTOMERS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.gOODCUSTOMERSTableAdapter.Fill(this.dataSet1.GOODCUSTOMERS);
            // TODO: 이 코드는 데이터를 'dataSet1.VW_CUSTOMERCLASSIFICATIONS' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.vW_CUSTOMERCLASSIFICATIONSTableAdapter.Fill(this.dataSet1.VW_CUSTOMERCLASSIFICATIONS);

        }
    }
}
