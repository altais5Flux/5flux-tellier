using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebservicesSage.Cotnroller;
using WebservicesSage.Singleton;


namespace WebservicesSage
{
    public partial class MappingOrdersStaut : Form
    {
        public MappingOrdersStaut()
        {
            InitializeComponent();
            SingletonUI.Instance.PrestaId1 = PrestaId1;
            SingletonUI.Instance.PrestaId2 = PrestaId2;
            SingletonUI.Instance.PrestaId3 = PrestaId3;

            SingletonUI.Instance.SageDoc1 = SageDoc1;
            SingletonUI.Instance.SageDoc2 = SageDoc2;
            SingletonUI.Instance.SageDoc3 = SageDoc3;

            ControllerConfiguration.LoadAllOrderStatutMappingConfiguration();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            //TODO Save ORDER STatut
            string SavedStatut, valuePresta, valueOrder;
            valuePresta = (SingletonUI.Instance.PrestaId1.selectedIndex+1) + "_" + (SingletonUI.Instance.PrestaId2.selectedIndex + 1) + "_" + (SingletonUI.Instance.PrestaId3.selectedIndex + 1);
            if (Utils.UtilsConfig.MagentoStatutId.TryGetValue("default", out SavedStatut))
            {
                if (!valuePresta.Equals(SavedStatut))
                {
                    Utils.UtilsConfig.UpdateNodeInCustomSection("MagentoStatutId", "default", valuePresta);
                }
            }
            valueOrder = (SingletonUI.Instance.SageDoc1.selectedIndex + 1) + "_" + (SingletonUI.Instance.SageDoc2.selectedIndex + 1) + "_" + (SingletonUI.Instance.SageDoc3.selectedIndex + 1);
            if (Utils.UtilsConfig.OrderMapping.TryGetValue("default", out SavedStatut))
            {
                if (!valueOrder.Equals(SavedStatut))
                {
                    Utils.UtilsConfig.UpdateNodeInCustomSection("OrderMapping", "default", valueOrder);
                }
            }
        }
    }
}
