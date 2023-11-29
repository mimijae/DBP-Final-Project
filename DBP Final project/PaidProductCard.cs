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
    public class PaidProductCard
    {
        private DataSet1 dataSet11;
        private DataSet1TableAdapters.REFUNDTableAdapter rEFUNDTableAdapter;

        public Panel CardPanel { get; private set; }
        public PictureBox ProductImage { get; private set; }
        public Label ProductName { get; private set; }
        public Label ProductPrice { get; private set; }
        public Label ProductCount { get; private set; }
        public Label ProductDate { get; private set; }
        public Button AddToCartButton { get; private set; }
        public string PurchaseId { get; private set; }
        public string UserId { get; private set; }
        public string ItemId { get; private set; }
        public string ImagePath { get; private set; }
        public string Name { get; private set; }
        public string TotalPrice { get; private set; }
        public int ItemCount { get; private set; }

        public string Date { get; private set; }

        public PaidProductCard(string userId, string purchaseId, string itemId, string imagePath, string name, string totalprice, int itemcount, string date)
        {
            PurchaseId = purchaseId;
            UserId = userId;
            ItemId = itemId;
            ImagePath = imagePath; // 이미지경로
            Name = name; // 물품 이름
            TotalPrice = totalprice; // 물품 가격
            ItemCount = itemcount;
            Date = date;

            dataSet11 = new DataSet1();
            rEFUNDTableAdapter = new DataSet1TableAdapters.REFUNDTableAdapter();

            InitializeCardPanel();
            InitializeProductImage();
            InitializeProductName();
            InitializeProductPrice();
            InitializeProductCount();
            //InitializeProductDate();
            InitializeRefundButton();
            AddControlsToPanel();



        }
        private void InitializeCardPanel()
        {
            CardPanel = new Panel
            {
                Size = new Size(150, 275),
                BorderStyle = BorderStyle.None,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10),
                Margin = new Padding(10)
            };

            CardPanel.Region = CreateRoundedRectangleRegion(CardPanel.Size);
        }

        private void InitializeProductImage()
        {
            ProductImage = new PictureBox
            {
                Size = new Size(130, 130),
                Image = Image.FromFile(ImagePath),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location =  new Point(10, 5)
            };
        }

        private void InitializeProductName()
        {
            ProductName = new Label
            {
                Text = Name,
                Size =  new Size(180, 30),
                Font = new Font("배달의민족 도현", 9, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(10, 150),
                //TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private void InitializeProductPrice()
        {
            ProductPrice = new Label
            {
                Text = "총 가격 :" + "\\" + TotalPrice.ToString() + "원",
                Size = new Size(180, 30),
                Font = new Font("배달의민족 도현", 8, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = new Point(10, 180),
                //TextAlign = ContentAlignment.MiddleCenter
            };
        }
        private void InitializeProductCount()
        {
            ProductCount = new Label
            {
                Text = "구매한 수량 :" + ItemCount.ToString(),
                Size = new Size(180, 30),
                Font = new Font("배달의민족 도현", 8, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = new Point(10, 210),
                //TextAlign = ContentAlignment.MiddleCenter
            };
        }


        private void InitializeProductDate()
        {
            ProductDate = new Label
            {
                Text = "구매한 날짜 : \r\n" + Date.ToString() ,
                Size = new Size(180, 30),
                Font = new Font("배달의민족 도현", 8, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = new Point(10, 240),
                //TextAlign = ContentAlignment.MiddleCenter
            };
        }
        private void InitializeRefundButton()
        {
            // 버튼 생성 및 설정
            rEFUNDTableAdapter.Fill(dataSet11.REFUND);
            DataTable refundTable = dataSet11.Tables["REFUND"];

            // REFUND 테이블에서 현재 사용자 ID, 아이템 ID, 구매 ID가 일치하는 로우를 찾습니다.
            DataRow[] existingRows = refundTable.Select($"CUSTOMER_ID = '{UserId}' AND ITEM_ID = '{ItemId}' AND PURCHASE_ID = '{PurchaseId}'");
            string result = "환불하기";

            // 일치하는 로우가 있고, REFUND_PROCESSING 값이 'false'가 아니라면, 이미 환불 요청이 들어갔다는 것을 확인합니다.
            if (existingRows.Length > 0 && existingRows[0]["REFUND_PROCESSING"].ToString() == "false")
            {
                result = "환불 요청중";
            }else if (existingRows.Length > 0 && existingRows[0]["REFUND_PROCESSING"].ToString() != "false")
            {
                result = "환불 완료";
            }

            AddToCartButton = new Button
            {
                Text = result,
                Size = new Size(130, 30),
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Location = new Point((CardPanel.Width - 130) / 2, 240)
            };

            // 버튼 클릭 이벤트 핸들러
            AddToCartButton.Click += (sender, e) =>
            {
                // 현재 버튼을 인자로 RefundItem 메서드에 전달합니다.
                RefundItem(UserId, ItemId, PurchaseId, (Button)sender);
            };

        }


        private void RefundItem(string userId, string itemId, string purchaseId, Button refundButton)
        {
            rEFUNDTableAdapter.Fill(dataSet11.REFUND);
            DataTable refundTable = dataSet11.Tables["REFUND"];

            // REFUND 테이블에서 현재 사용자 ID, 아이템 ID, 구매 ID가 일치하는 로우를 찾습니다.
            DataRow[] existingRows = refundTable.Select($"CUSTOMER_ID = '{userId}' AND ITEM_ID = '{itemId}' AND PURCHASE_ID = '{purchaseId}'");

            // 일치하는 로우가 있다면, 이미 환불 요청이 들어갔다는 메시지를 표시합니다.
            if (existingRows.Length > 0)
            {
                MessageBox.Show("이미 환불 요청이 처리중입니다.");
            }
            else
            {
                // 환불 요청이 없으므로 새로운 로우를 추가합니다.
                DataRow newRow = refundTable.NewRow();
                newRow["CUSTOMER_ID"] = userId;
                newRow["ITEM_ID"] = itemId;
                newRow["PURCHASE_ID"] = purchaseId;
                newRow["REFUND_PROCESSING"] = "false"; // 환불 처리 상태를 '요청됨'으로 설정합니다.
                refundTable.Rows.Add(newRow);

                // 데이터베이스에 변경 사항을 업데이트하고 결과에 따라 메시지를 표시합니다.
                int numOfRows = rEFUNDTableAdapter.Update(dataSet11.REFUND);
                if (numOfRows < 1)
                {
                    MessageBox.Show("환불 요청 실패하였습니다.");
                }
                else
                {
                    MessageBox.Show("환불 요청이 접수되었습니다.");
                    refundButton.Text = "환불요청중"; // 버튼 텍스트를 변경합니다.
                    refundButton.ForeColor = Color.White;
                   // refundButton.Enabled = false; // 추가적으로, 버튼을 비활성화할 수도 있습니다.
                }
            }
        }




        private void AddControlsToPanel()
        {
            CardPanel.Controls.Add(ProductImage);
            CardPanel.Controls.Add(ProductName);
            CardPanel.Controls.Add(ProductPrice);
            CardPanel.Controls.Add(ProductCount);
            //CardPanel.Controls.Add(ProductDate);

            CardPanel.Controls.Add(AddToCartButton);
        }
        private Region CreateRoundedRectangleRegion(Size size)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, 20, 20, 180, 90);
            path.AddArc(size.Width - 20, 0, 20, 20, 270, 90);
            path.AddArc(size.Width - 20, size.Height - 20, 20, 20, 0, 90);
            path.AddArc(0, size.Height - 20, 20, 20, 90, 90);
            path.CloseAllFigures();

            return new Region(path);
        }
    }
}
