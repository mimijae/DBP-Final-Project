﻿using System;
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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            UserSiginIn userSiginIn = new UserSiginIn();

            // 패널에 컨트롤 추가
            panelSign.Controls.Add(userSiginIn);
        }
    }
}
