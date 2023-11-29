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
    public partial class cartForm : Form
    {
        DataSet1.CARTDataTable cartTable; // CARTDataTable로 타입 변경
        DataSet1.ITEMDataTable itemTable; // ITEMDataTable로 타입 변경
        DataSet1.PURCHASEDataTable purchaseTable; // PURCHASEDataTable로 타입 변경
        DataSet1.PURCHASEDITEMDataTable purchaseitemTable;
        string currentUserId = UserSession.UserId;
        string currentUserType = UserSession.UserType;
        string itemId = string.Empty;
        private List<ProductCard> productCards = new List<ProductCard>();  // ProductCard 인스턴스 목록

        public cartForm()
        {
            InitializeComponent();
            cartTableAdapter1.Fill(dataSet11.CART);
            itemTableAdapter1.Fill(dataSet11.ITEM);
            purchaseTableAdapter1.Fill(dataSet11.PURCHASE);
            purchaseditemTableAdapter1.Fill(dataSet11.PURCHASEDITEM);

            cartTable = dataSet11.CART;
            itemTable = dataSet11.ITEM;
            purchaseTable = dataSet11.PURCHASE;
            purchaseitemTable = dataSet11.PURCHASEDITEM;

            string filterExpression = $"CUSTOMER_ID = '{currentUserId}'";
            DataRow[] foundRows = cartTable.Select(filterExpression);

            var productInfos = new System.Collections.Generic.List<(string UserId, string ItemId, string ImagePath, string Name, string Price, int Count, string Category, string Date, string Content)>();

            foreach (DataRow cartRow in foundRows)
            {
                itemId = cartRow["ITEM_ID"].ToString();
                DataRow[] itemRows = itemTable.Select($"ITEM_ID = '{itemId}'");

                foreach (DataRow itemRow in itemRows)
                {
                    productInfos.Add((
                    UserId: currentUserId,
                    ItemId: itemRow["ITEM_ID"].ToString(),
                    ImagePath: itemRow["ITEM_IMAGE"].ToString(),
                    Name: itemRow["ITEM_NAME"].ToString(),
                    Price: "\\" + itemRow["ITEM_PRICE"].ToString() + "원",
                    Count: int.Parse(itemRow["ITEM_COUNT"].ToString()),
                    Category: itemRow["ITEM_CATEGORY"].ToString(),
                    Date: itemRow["ITEM_DATE"].ToString(),
                    Content: itemRow["ITEM_CONTENT"].ToString()
                ));
                }
            }
            foreach (var info in productInfos)
            {
                var productCard = new ProductCard(null, fPanelShowCart, "cartForm", info.UserId, info.ItemId, info.ImagePath, info.Name, info.Price, info.Count, info.Category, info.Date, info.Content);
                fPanelShowCart.Controls.Add(productCard.CardPanel);
                productCards.Add(productCard);  // 리스트에 추가
            }
        }

        private void purchaseBtn_Click(object sender, EventArgs e)
        {
            var selectedCards = productCards.Where(pc => pc.SelectCheckBox.Checked).ToList();
            if (!selectedCards.Any())
            {
                MessageBox.Show("구매하려는 상품을 하나 이상 선택해 주세요.");
                return;
            }

            bool isPurchasePossible = true;
            // 선택된 아이템들에 대한 재고 확인
            foreach (var card in selectedCards)
            {
                DataRow itemRow = itemTable.Select($"ITEM_ID = '{card.ItemId}'").FirstOrDefault();
                if (itemRow != null)
                {
                    int currentCount = Convert.ToInt32(itemRow["ITEM_COUNT"]);
                    if (currentCount < card.Count)
                    {
                        MessageBox.Show($"{itemRow["ITEM_NAME"]}의 재고가 부족합니다.", "구매 불가");
                        isPurchasePossible = false;
                    }
                }
            }

            if (!isPurchasePossible)
            {
                MessageBox.Show("일부 상품의 구매가 취소되었습니다. 재고를 확인해주세요.");
                return;
            }

            decimal totalPurchasePrice = 0;

            DataRow newPurchaseRow = purchaseTable.NewRow();
            newPurchaseRow["CUSTOMER_ID"] = currentUserId;
            newPurchaseRow["PURCHASE_DATE"] = DateTime.Now;
            purchaseTable.Rows.Add(newPurchaseRow);
            purchaseTableAdapter1.Update(dataSet11.PURCHASE);
            dataSet11.AcceptChanges();

            int newPurchaseId = Convert.ToInt32(newPurchaseRow["PURCHASE_ID"]);

            foreach (var card in selectedCards)
            {
                DataRow[] cartRows = cartTable.Select($"CUSTOMER_ID = '{currentUserId}' AND ITEM_ID = '{card.ItemId}'");
                foreach (DataRow row in cartRows)
                {
                    row.Delete();
                }

                DataRow itemRow = itemTable.Select($"ITEM_ID = '{card.ItemId}'").FirstOrDefault();
                if (itemRow != null)
                {
                    int currentCount = Convert.ToInt32(itemRow["ITEM_COUNT"]);
                    itemRow["ITEM_COUNT"] = currentCount - card.Count;

                    decimal itemPrice = Convert.ToDecimal(itemRow["ITEM_PRICE"]);
                    DataRow newPurchasedItemRow = purchaseitemTable.NewRow();
                    newPurchasedItemRow["PURCHASE_ID"] = newPurchaseId;
                    newPurchasedItemRow["ITEM_ID"] = card.ItemId;
                    newPurchasedItemRow["ITEM_COUNT"] = card.Count;
                    newPurchasedItemRow["TOTAL_PRICE"] = itemPrice * card.Count;
                    purchaseitemTable.Rows.Add(newPurchasedItemRow);
                }
                fPanelShowCart.Controls.Remove(card.CardPanel);
            }

            cartTableAdapter1.Update(dataSet11.CART);
            itemTableAdapter1.Update(dataSet11.ITEM);
            purchaseditemTableAdapter1.Update(dataSet11.PURCHASEDITEM);
            dataSet11.AcceptChanges();

            MessageBox.Show("구매 완료 되었습니다.");
        }



    }

}