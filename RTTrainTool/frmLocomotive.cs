﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RTTrainTool
{
	internal partial class frmLocomotive : Form
	{
		private bool lcmode = false;
		private frmEngineCoach frm;

		public frmLocomotive(bool _lcmode = false, frmEngineCoach _frm = null)
		{
			InitializeComponent();

			cbRank.SelectedIndex = 1;

			lcmode = _lcmode;
			frm = _frm;

			if (lcmode)
			{
				btnSave.Text = "적용";
				txtName.Enabled = false;
				nuMaintenance.Enabled = false;
				nuPrice.Enabled = false;
				btnReset.Enabled = false;
				btnBrowse.Enabled = false;
				txtURL.Enabled = false;

				Text = "기관차 설정";
				lbTitle.Text = "기관차 설정";
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (!lcmode)
				{
					if (txtName.Text.Trim() == string.Empty)
					{
						MessageBox.Show("이름을 입력해 주세요.", "RTTrainTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					FolderBrowserDialog fbd = new FolderBrowserDialog();
					fbd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
					fbd.ShowNewFolderButton = true;
					if (fbd.ShowDialog() == DialogResult.OK)
					{
						string path = fbd.SelectedPath + $@"\\{txtName.Text.Trim()}";
						System.IO.Directory.CreateDirectory(path);

						XmlDocument xml = new XmlDocument();

						XmlNode root = xml.CreateElement("locomotive");

						XmlNode name = xml.CreateElement("name");
						name.InnerText = txtName.Text.Trim();
						root.AppendChild(name);

						XmlNode main = xml.CreateElement("maintenance");
						main.InnerText = nuMaintenance.Value.ToString();
						root.AppendChild(main);

						XmlNode speed = xml.CreateElement("speed");
						speed.InnerText = nuSpeed.Value.ToString();
						root.AppendChild(speed);

						XmlNode price = xml.CreateElement("price");
						price.InnerText = nuPrice.Value.ToString();
						root.AppendChild(price);

						XmlNode rank = xml.CreateElement("rank");
						if (cbRank.SelectedIndex == 0)
						{
							rank.InnerText = "high";
						}
						else
						{
							rank.InnerText = "default";
						}
						root.AppendChild(rank);

						XmlNode image = xml.CreateElement("image");
						image.InnerText = "img.png";
						root.AppendChild(image);

						XmlNode carrying = xml.CreateElement("carrying");
						carrying.InnerText = nuCarry.Value.ToString();
						root.AppendChild(carrying);

						xml.AppendChild(root);
						xml.Save(path + @"\data.xml");

						if (txtURL.Text.Trim() == string.Empty)
						{
							Properties.Resources.img_no.Save(path + @"\img.png");
						}
						else
						{
							Image img = Image.FromFile(txtURL.Text);
							img.Save(path + @"\img.png", System.Drawing.Imaging.ImageFormat.Png);
						}

						MessageBox.Show("작업이 완료 되었습니다.", "RTTrainTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
					frm.loc_edit = true;
					frm.loc_speed = Convert.ToDouble(nuSpeed.Value);
					frm.loc_rank = cbRank.SelectedIndex;
					frm.loc_carrying = Convert.ToInt64(nuSpeed.Value);
					Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("오류:\n" + ex.ToString(), "RTTrainTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			txtURL.Text = string.Empty;
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "기관차 이미지";
			ofd.Filter = "이미지 파일|*.png;*.jpg;*.jpge;*.gif;*.bmp|모든 파일|*.*";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				txtURL.Text = ofd.FileName;
			}
		}
	}
}
