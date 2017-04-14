using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester {
    public partial class Form1:Form {
        public Form1() {
            
            InitializeComponent();

            using(OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.ShowDialog();

                if(ofd.FileName != null)
                    txtFile.Text = ofd.FileName;
            }

            List<ID3TagLibV2.Types.ID3Fields> fields = new List<ID3TagLibV2.Types.ID3Fields>();
            foreach(ID3TagLibV2.Types.ID3Fields f in Enum.GetValues(typeof(ID3TagLibV2.Types.ID3Fields))){
                fields.Add(f);
            }

            cmdRead.Focus();
        }

        private void cmdRead_Click(object sender, EventArgs e) {
            ID3TagLibV2.ID3Tag tag = ID3TagLibV2.TagHandler.ReadFileTags(txtFile.Text);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBox1.ImageLocation = ID3TagLibV2.Helpers.CreateImageFile(tag.AlbumArt, "c:\\zim\\");
        }

        private void cmdUpdate_Click(object sender, EventArgs e) {
            //ID3TagLibV2.ID3Tag tag = new ID3TagLibV2.ID3Tag();
            ID3TagLibV2.ID3Tag tag = ID3TagLibV2.TagHandler.ReadFileTags(txtFile.Text);
            ID3TagLibV2.TagHandler.UpdateFileTags(txtFile.Text, tag);
        }

        private void cmdDelete_Click(object sender, EventArgs e) {
            ID3TagLibV2.TagHandler.RemoveAllTags(txtFile.Text);
        }
    }
}
