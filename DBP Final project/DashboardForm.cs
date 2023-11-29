using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace DBP_Final_project
{
    public partial class DashboardForm : Form
    {
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        string currentUserId = UserSession.UserId;
        string currentUserType = UserSession.UserType;
        private ContextMenuStrip contextMenuStrip1;
        private DataSet1TableAdapters.ADMINTableAdapter aDMINTableAdapter;
        private DataSet1TableAdapters.SELLERTableAdapter sELLERTableAdapter;
        private DataSet1TableAdapters.CUSTOMERTableAdapter cUSTOMERTableAdapter;

        public DashboardForm()
        {
            InitializeComponent();

            dataSet11 = new DataSet1();
            aDMINTableAdapter = new DataSet1TableAdapters.ADMINTableAdapter();
            sELLERTableAdapter = new DataSet1TableAdapters.SELLERTableAdapter();
            cUSTOMERTableAdapter = new DataSet1TableAdapters.CUSTOMERTableAdapter();

            aDMINTableAdapter.Fill(dataSet11.ADMIN);
            sELLERTableAdapter.Fill(dataSet11.SELLER);
            cUSTOMERTableAdapter.Fill(dataSet11.CUSTOMER);

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 49);
            panelMenu.Controls.Add(leftBorderBtn);
            userLabel.Text= currentUserId + " 님";
            timenow.Start();
            LoadData();
            marqueeTimer.Start();
            flashTimer.Start();
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            // 먼저 모든 버튼을 숨깁니다.
            iconButton1.Visible = false;
            iconButton2.Visible = false;
            iconButton3.Visible = false;
            iconButton4.Visible = false;
            iconButton5.Visible = false;
            iconButton6.Visible = false;
            iconButton7.Visible = false;

            // 사용자 유형에 따라 버튼을 표시합니다.
            switch (currentUserType)
            {
                case "CUSTOMER":
                    iconButton1.Visible = true;
                    iconButton2.Visible = true;
                    iconButton3.Visible = true;
                    break;
                case "SELLER":
                    iconButton1.Visible = true;
                    iconButton4.Visible = true;
                    iconButton7.Visible = true;
                    break;
                case "ADMIN":
                    iconButton1.Visible = true;
                    iconButton4.Visible = true;
                    iconButton5.Visible = true;
                    iconButton6.Visible = true;
                    iconButton7.Visible = true;
                    break;
            }
        }
        #region Methods
        private struct RGBColors
        {
            public static Color color1 = Color.LightGreen;
        }
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.DarkSeaGreen;
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Current Child Form Icon
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = Color.DarkSeaGreen;
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.DarkSeaGreen;
                currentBtn.ForeColor = Color.White;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.White;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
        private void OpenChildForm(Form childForm, String name)
        {
            //open only form
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            //End
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = name;
        }
        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = Color.DarkSeaGreen;
            lblTitleChildForm.Text = "메인 홈";
        }
        //Events
        //Reset
        #endregion

        private void iconButton1_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new goodsShowForm(), "상품 둘러보기");
        }

        private void iconButton2_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new cartForm(), "장바구니");
        }

        private void iconButton3_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new goodsPaidForm(), "결제한 상품");
        }

        private void iconButton5_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new InventoryManagementForm(),"재고관리");
        }

        private void iconButton4_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new PurchaseDetailsForm(), "구매내역관리");
        }

        private void iconButton6_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new ChartForm(),"판매액 현황");
        }

        private void iconButton7_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new CustomerForm(), "고객관리");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            Reset();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panelTitlebar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            // 사용자 세션 정보를 정리합니다.
            UserSession.ClearUser();

            // LoginForm의 인스턴스를 찾습니다.
            LoginForm loginForm = (LoginForm)Application.OpenForms["LoginForm"];

            if (loginForm == null)
            {
                // LoginForm이 열려있지 않은 경우 새 인스턴스를 생성하고 표시합니다.
                loginForm = new LoginForm();

                loginForm.Show();
            }
            else
            {
                // LoginForm이 이미 열려있다면 그 인스턴스를 활성화합니다.
                loginForm.Show();
            }

            // 대시보드 폼을 숨깁니다.
            this.Hide();

            // 대시보드 폼을 닫습니다.
            this.Close();
        }

        private void iconButton12_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void iconButton13_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }

        }

        private void iconButton14_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void timenow_Tick(object sender, EventArgs e)
        {
            lbtime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public void DeleteUserAccount(string userId, string userTpye)
        {
            try
            {
                // UserType에 따라 적절한 TableAdapter와 DataTable을 선택합니다.
                switch (userTpye)
                {
                    case "SELLER":
                        DataRow deleteRow = dataSet11.SELLER.Rows.Find(userId);
                        if (deleteRow != null)
                        {
                            // 가져온 데이터를 삭제합니다.
                            deleteRow.Delete();

                            // 변경 사항을 데이터베이스에 업데이트합니다.
                            int numOfRows = sELLERTableAdapter.Update(dataSet11.SELLER);
                            if (numOfRows < 1)
                            {
                                MessageBox.Show("회원 삭제 실패");
                            }
                            else
                            {
                                // 성공적으로 삭제되었음을 알립니다.
                                MessageBox.Show("회원 삭제 성공");
                                Application.Exit();
                            }
                        }
                        else
                        {
                            MessageBox.Show("삭제할 회원을 찾을 수 없습니다.");
                        }
                        break;
                    case "CUSTOMER":
                        DataRow deleteRow2 = dataSet11.CUSTOMER.Rows.Find(userId);
                        if (deleteRow2 != null)
                        {
                            // 가져온 데이터를 삭제합니다.
                            deleteRow2.Delete();

                            // 변경 사항을 데이터베이스에 업데이트합니다.
                            int numOfRows = cUSTOMERTableAdapter.Update(dataSet11.CUSTOMER);
                            if (numOfRows < 1)
                            {
                                MessageBox.Show("회원 삭제 실패");
                            }
                            else
                            {
                                // 성공적으로 삭제되었음을 알립니다.
                                MessageBox.Show("회원 삭제 성공");

                                Application.Exit();
                            }
                        }
                        else
                        {
                            MessageBox.Show("삭제할 회원을 찾을 수 없습니다.");
                        }
                        break;
                    case "ADMIN":
                        DataRow deleteRow3 = dataSet11.ADMIN.Rows.Find(userId);
                        if (deleteRow3 != null)
                        {
                            // 가져온 데이터를 삭제합니다.
                            deleteRow3.Delete();

                            // 변경 사항을 데이터베이스에 업데이트합니다.
                            int numOfRows = aDMINTableAdapter.Update(dataSet11.ADMIN);
                            if (numOfRows < 1)
                            {
                                MessageBox.Show("회원 삭제 실패");
                            }
                            else
                            {
                                // 성공적으로 삭제되었음을 알립니다.
                                MessageBox.Show("회원 삭제 성공");
                                Application.Exit();
                            }
                        }
                        else
                        {
                            MessageBox.Show("삭제할 회원을 찾을 수 없습니다.");
                        }
                        break;
                    default:
                        MessageBox.Show("알 수 없는 사용자 유형입니다.");
                        return;
                }
                UserSession.ClearUser();
                MessageBox.Show("회원 탈퇴가 완료되었습니다.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("회원 탈퇴 중 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void marqueeTimer_Tick(object sender, EventArgs e)
        {
            movingLabel.Left -= 1;

            // Reset position if it's completely out of view
            if (movingLabel.Right < 0)
            {
                movingLabel.Left = adPanel.Width;
            }
        }
        private void LoadData()
        {
            itemTableAdapter1.Fill(dataSet11.ITEM);
            DataTable mytable = dataSet11.Tables["ITEM"];

            var productInfos = mytable.AsEnumerable()
                .Select(mydataRow => (
                    UserId: currentUserId,
                    ItemId: mydataRow["ITEM_ID"],
                    ImagePath: mydataRow["ITEM_IMAGE"].ToString(),
                    Name: mydataRow["ITEM_NAME"].ToString(),
                    Price: mydataRow["ITEM_PRICE"].ToString(),
                    Count: Convert.ToInt32(mydataRow["ITEM_COUNT"]),
                    Category: mydataRow["ITEM_CATEGORY"].ToString(),
                    Date: Convert.ToDateTime(mydataRow["ITEM_DATE"]),
                    Content: mydataRow["ITEM_CONTENT"].ToString()
                ))
                .OrderByDescending(info => info.Date)
                .ToList();

            // 가장 최근에 추가된 아이템의 정보를 가져옵니다.
            var mostRecentItem = productInfos.FirstOrDefault();

                movingLabel.Text = $"★신상품 ☆{mostRecentItem.Name}!!☆ 단돈 \\{mostRecentItem.Price}원에 구매하세요!!!★";

        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            // Confirm with the user
            var confirmation = MessageBox.Show("회원 탈퇴하시겠습니까?", "회원 탈퇴 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                // If user chooses 'Yes', execute withdrawal logic
                try
                {
                    // Here you would add your logic to remove the user's data
                    // For example: DeleteUser(UserId);
                    DeleteUserAccount(currentUserId, currentUserType);


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"회원 탈퇴 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void flashTimer_Tick(object sender, EventArgs e)
        {
            if (movingLabel.BackColor == Color.Red)
            {
                movingLabel.BackColor = Color.Yellow;
                movingLabel.ForeColor = Color.Red;
            }
            else
            {
                movingLabel.BackColor = Color.Red;
                movingLabel.ForeColor = Color.Yellow;
            }
        }
    }
}
