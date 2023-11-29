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
   

    public partial class UserSiginIn : UserControl
    {
      
        DataTable ADMIN;
        DataTable CUSTOMER;
        DataTable SELLER;

        public UserSiginIn()
        {
            InitializeComponent();
            adminTableAdapter1.Fill(dataSet11.ADMIN);
            customerTableAdapter1.Fill(dataSet11.CUSTOMER);
            sellerTableAdapter1.Fill(dataSet11.SELLER);

            ADMIN = dataSet11.Tables["ADMIN"];
            CUSTOMER = dataSet11.Tables["CUSTOMER"];
            SELLER = dataSet11.Tables["SELLER"];
        }

        private void label2_Click(object sender, EventArgs e)
        {
            LoginForm parentForm = this.FindParentForm() as LoginForm;
            if (parentForm != null)
            {
                UserSiginUP signUpControl = new UserSiginUP();
                parentForm.panelSign.Controls.Clear();
                parentForm.panelSign.Controls.Add(signUpControl);
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
            // 사용자가 입력한 이메일과 비밀번호를 가져옵니다.
            string email = loginEmailTbox.Text;
            string password = loginPassTbox.Text; // 실제 애플리케이션에서는 비밀번호를 해싱 처리해야 합니다.

            // 이메일과 비밀번호가 데이터베이스에 존재하는지 확인합니다.
            bool isValidUser = ValidateUser(email, password);

            if (isValidUser)
            {
                // 사용자가 유효한 경우, 대시보드를 표시하고 로그인 폼을 숨깁니다.
                loginEmailTbox.Text = "";
                loginPassTbox.Text = "";

                new DashboardForm().Show();
                //this.Hide();
            }
            else
            {
                // 사용자가 유효하지 않은 경우, 오류 메시지를 표시합니다.
                MessageBox.Show("이메일 또는 비밀번호가 잘못되었습니다.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginEmailTbox.Text = "";
                loginPassTbox.Text = "";
            }
        }

        private bool ValidateUser(string email, string password)
        {
            // 사용자 입력에서 공백을 제거하고 단일 인용부호를 이스케이프 처리
            string safeEmail = email.Trim();
            string safePassword = password.Trim();

            // 필터 표현식을 구성
            string filterExpression = $"EMAIL_ID = '{safeEmail}' AND PASSWARD = '{safePassword}'"; // 'PASSWARD'가 'PASSWORD'로 변경되었는지 확인하세요.

            try
            {
                // 관리자를 확인합니다.
                DataRow adminRow = ADMIN.Select(filterExpression).FirstOrDefault();
                if (adminRow != null)
                {
                    UserSession.SetUser(adminRow["EMAIL_ID"].ToString(), "ADMIN");
                    return true;
                }

                // 고객을 확인합니다.
                DataRow customerRow = CUSTOMER.Select(filterExpression).FirstOrDefault();
                if (customerRow != null)
                {
                    UserSession.SetUser(customerRow["EMAIL_ID"].ToString(), "CUSTOMER");
                    return true;
                }

                // 판매자를 확인합니다.
                DataRow sellerRow = SELLER.Select(filterExpression).FirstOrDefault();
                if (sellerRow != null)
                {
                    UserSession.SetUser(sellerRow["EMAIL_ID"].ToString(), "SELLER");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사용자를 검증하는 동안 오류가 발생했습니다: {ex.Message}");
            }

            // 일치하는 항목이 없거나 예외가 발생한 경우 false를 반환
            return false;
        }

        private void loginPassTbox_KeyDown(object sender, KeyEventArgs e)
        {
            // 사용자가 엔터 키를 눌렀는지 확인합니다.
            if (e.KeyCode == Keys.Enter)
            {
                loginBtn_Click(sender, e);
            }
        }
    }
}
