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
    public partial class goodsShowForm : Form
    {
        DataTable mytable1;
        string currentUserId = UserSession.UserId;
        string currentUserType = UserSession.UserType;
        private List<ProductCard> productCards = new List<ProductCard>();
        public string CurrentItemId { get; set; }

        public goodsShowForm()
        {
            InitializeComponent();
        }

        private void goodsShowForm_Load(object sender, EventArgs e)
        {
            listupdate();
        }

        public void listupdate()
        {
            itemTableAdapter1.Fill(dataSet11.ITEM);
            DataTable mytable = dataSet11.Tables["ITEM"];
            var productInfos = new System.Collections.Generic.List<(string UserId, string ItemId, string ImagePath, string Name, string Price, int Count, string Category, DateTime Date, string Content)>();

            foreach (DataRow mydataRow in mytable.Rows)
            {
                productInfos.Add((
                    UserId: currentUserId,
                    ItemId: mydataRow["ITEM_ID"].ToString(),
                    ImagePath: mydataRow["ITEM_IMAGE"].ToString(),
                    Name: mydataRow["ITEM_NAME"].ToString(),
                    Price: "\\" + mydataRow["ITEM_PRICE"].ToString() + "원",
                    Count: int.Parse(mydataRow["ITEM_COUNT"].ToString()),
                    Category: mydataRow["ITEM_CATEGORY"].ToString(),
                    Date: DateTime.Parse(mydataRow["ITEM_DATE"].ToString()),
                    Content: mydataRow["ITEM_CONTENT"].ToString()
                ));
            }

            // ITEM_DATE를 기준으로 내림차순 정렬
            var sortedProductInfos = productInfos.OrderByDescending(info => info.Date).ToList();

            foreach (var info in sortedProductInfos)
            {
                var productCard = new ProductCard(this, fPanelShowDetailGoods, "goodsShowForm", info.UserId, info.ItemId, info.ImagePath, info.Name, info.Price, info.Count, info.Category, info.Date.ToString(), info.Content);
                fPanelShowGoods.Controls.Add(productCard.CardPanel);
                productCards.Add(productCard);
            }
            itemTableAdapter1.Fill(dataSet11.ITEM);
            mytable1 = dataSet11.Tables["ITEM"];
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(CurrentItemId);
            reviewTableAdapter1.Fill(dataSet11.REVIEW);
                DataTable reviewTable = dataSet11.Tables["REVIEW"];


                DataRow newRow = reviewTable.NewRow();
                newRow["CUSTOMER_ID"] = currentUserId;
                newRow["ITEM_ID"] = int.Parse(CurrentItemId);
                newRow["STAR_COUNT"] = Convert.ToInt32(numericUpDown1.Value);

                newRow["CONTENT"] = textBox1.Text.ToString();
                reviewTable.Rows.Add(newRow);

                // 데이터베이스에 변경 사항을 업데이트하고 결과에 따라 메시지를 표시합니다.
                int numOfRows = reviewTableAdapter1.Update(dataSet11.REVIEW);
                if (numOfRows < 1)
                    MessageBox.Show("리뷰를 다는 것을 실패하였습니다.");
                else
            {
                var matchingProductCard = productCards.FirstOrDefault(pc => pc.ItemId == CurrentItemId);

                // 만약 해당 ProductCard가 존재한다면 SetClickEvents를 호출합니다.
                if (matchingProductCard != null)
                {
                    matchingProductCard.ShowDetailInfo(fPanelShowDetailGoods);
                }
                MessageBox.Show($"리뷰를 추가하였습니다.");
                textBox1.Text = "";
            }
                   

        }
    }
}
