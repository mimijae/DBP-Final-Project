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
    public partial class UserSiginUP : UserControl
    {
        DataTable ADMIN;
        DataTable CUSTOMER;
        DataTable SELLER;

        public UserSiginUP()
        {
            InitializeComponent();
            /*adminTableAdapter1.Fill(dataSet1.ADMIN);
            customerTableAdapter1.Fill(dataSet1.CUSTOMER);
            sellerTableAdapter1.Fill(dataSet1.SELLER);*/

            ADMIN = dataSet1.Tables["ADMIN"];
            CUSTOMER = dataSet1.Tables["CUSTOMER"];
            SELLER = dataSet1.Tables["SELLER"];
        }

        private void label2_Click(object sender, EventArgs e)
        {
            LoginForm parentForm = this.FindParentForm() as LoginForm;
            if (parentForm != null)
            {
                UserSiginIn signInControl = new UserSiginIn();
                parentForm.panelSign.Controls.Clear();
                parentForm.panelSign.Controls.Add(signInControl);
            }
        }

        private Form FindParentForm()
        {
            Control current = this;
            while (current != null && !(current is Form))
            {
                current = current.Parent;
            }
            return current as Form;
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
          // 라디오 버튼들 중에서 선택된 라디오 버튼의 텍스트를 찾습니다.
            string selectedRadioButtonText = GetSelectedRadioButtonText();


            if (selectedRadioButtonText=="고객")
            {
                DataRow mynewDataRow = CUSTOMER.NewRow();
                mynewDataRow["EMAIL_ID"] = loginEmailTbox.Text;
                if(loginPassTbox1.Text.Trim() == loginPassTbox2.Text.Trim())
                {
                    mynewDataRow["PASSWARD"] = loginPassTbox1.Text;
                }
                else
                {
                    MessageBox.Show("비밀번호를 다시 확인해주세요");
                    loginPassTbox1.Text = "";
                    loginPassTbox2.Text = "";
                    return;
                }

                    CUSTOMER.Rows.Add(mynewDataRow);
                int numOfRows = customerTableAdapter1.Update(dataSet1.CUSTOMER);
                if (numOfRows > 0)
                    MessageBox.Show("회원가입 성공!");
                else
                    MessageBox.Show("회원가입 실패");
            }
            else if(selectedRadioButtonText == "판매자")
            {
                DataRow mynewDataRow = SELLER.NewRow();
                mynewDataRow["EMAIL_ID"] = loginEmailTbox.Text;
                mynewDataRow["PASSWARD"] = loginPassTbox1.Text;

                SELLER.Rows.Add(mynewDataRow);
                int numOfRows = sellerTableAdapter1.Update(dataSet1.SELLER);
                if (numOfRows > 0)
                    MessageBox.Show("회원가입 성공!");
                else
                    MessageBox.Show("회원가입 실패");
            }
            else
            {
                DataRow mynewDataRow = ADMIN.NewRow();
                mynewDataRow["EMAIL_ID"] = loginEmailTbox.Text;
                mynewDataRow["PASSWARD"] = loginPassTbox1.Text;

                ADMIN.Rows.Add(mynewDataRow);
                int numOfRows = adminTableAdapter1.Update(dataSet1.ADMIN);
                if (numOfRows > 0)
                    MessageBox.Show("회원가입 성공!");
                else
                    MessageBox.Show("회원가입 실패");
            }
        }

        private string GetSelectedRadioButtonText()
        {
            // 이 메서드는 현재 폼에서 모든 RadioButton 컨트롤을 순회하며
            // 체크된 것을 찾아 그의 텍스트를 반환합니다.
            foreach (Control control in this.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton.Text;
                }
            }
            return null;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
