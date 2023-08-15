using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWO_AutoProfessions
{
    public partial class NWO_ProfessionLeveling_ProfessionSelectConfig : Form
    {
        public NWO_ProfessionLeveling_ProfessionSelectConfig()
        {
            InitializeComponent();

            NWO_Defines.PopulateComboBoxes.Profession(NWO_ProfessionLeveling_ProfessionSelect_Profession_ComboBox);
        }

        public String Profession
        {
            get
            {
                return NWO_ProfessionLeveling_ProfessionSelect_Profession_ComboBox.SelectedItem.ToString();
            }
            set
            {
                int curSel = NWO_ProfessionLeveling_ProfessionSelect_Profession_ComboBox.FindStringExact(value);
                if (curSel < 0)
                    curSel = 0;
                    
                NWO_ProfessionLeveling_ProfessionSelect_Profession_ComboBox.SelectedIndex = curSel;
            }
        }
    }
}
