using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace system_status.App_code
{
    class firewall_info
    {

        Form1 _form = null;
        public bool is_running = false;
        // Protocol
        // From : https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/aa364724(v=vs.85)
        private int NET_FW_IP_PROTOCOL_TCP = 6;
        private int NET_FW_IP_PROTOCOL_UDP = 17;
        private int NET_FW_IP_PROTOCOL_ICMPv4 = 1;
        private int NET_FW_IP_PROTOCOL_ICMPv6 = 58;

        // Direction
        private int NET_FW_RULE_DIR_IN = 1;
        private int NET_FW_RULE_DIR_OUT = 2;

        // Action
        private int NET_FW_ACTION_BLOCK = 0;
        private int NET_FW_ACTION_ALLOW = 1;
        public void init(Form1 theform)
        {

            _form = theform;
            is_running = true;
            _form.firewall_grid.AutoGenerateColumns = false; //這啥
            _form.firewall_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.firewall_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.firewall_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.firewall_grid.AllowDrop = false;
            _form.firewall_grid.ReadOnly = true;
            

            _form.firewall_grid.Columns.Clear();
            string json_columns = @"
[
    {   
        ""firewallID"":{""id"":""firewallID"",""name"":""項次"",""width"":50,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },           
    {   
        ""firewallName"":{""id"":""firewallName"",""name"":""功能名稱"",""width"":300,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
            
    {   
        ""firewallApplicationName"":{""id"":""firewallApplicationName"",""name"":""ApplicationName"",""width"":350,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    }, 
    {   
        ""firewallServiceName"":{""id"":""firewallServiceName"",""name"":""ServiceName"",""width"":150,""display"":true,""headerAlign"":""center"",""cellAlign"":""left""}
    },
    {   
        ""firewallEnabled"":{""id"":""firewallEnabled"",""name"":""Enabled"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""firewallProtocol"":{""id"":""firewallProtocol"",""name"":""Protocol"",""width"":80,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""firewallDirectionInOut"":{""id"":""firewallDirectionInOut"",""name"":""連入/連出"",""width"":120,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""firewallLocalPorts"":{""id"":""firewallLocalPorts"",""name"":""LocalPorts"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""firewallRemotePorts"":{""id"":""firewallRemotePorts"",""name"":""RemotePorts"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    }
]";
            /*
,
    {   
        ""firewallLocalAddresses"":{""id"":""firewallLocalAddresses"",""name"":""LocalAddresses"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {   
        ""firewallRemoteAddresses"":{""id"":""firewallRemoteAddresses"",""name"":""RemoteAddresses"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    }
             */
            //表格初始化
            _form.my.grid_init(_form.firewall_grid, json_columns);


            //allow sorting
            foreach (DataGridViewColumn column in _form.firewall_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }



            run();
        }
        public void run()
        {

            
            int lastId = 0;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            IEnumerable Rules = fwPolicy2.Rules as IEnumerable;
            foreach (dynamic rule in Rules)
            {
                _form.firewall_grid.Rows.Add();
                lastId = _form.firewall_grid.Rows.Count - 1;
                _form.firewall_grid.Rows[lastId].Cells["firewallID"].Value = (lastId + 1);
                _form.firewall_grid.Rows[lastId].Cells["firewallName"].Value = rule.Name;
                _form.firewall_grid.Rows[lastId].Cells["firewallApplicationName"].Value = rule.ApplicationName; 
                _form.firewall_grid.Rows[lastId].Cells["firewallServiceName"].Value = rule.ServiceName; 
                _form.firewall_grid.Rows[lastId].Cells["firewallEnabled"].Value = rule.Enabled; 
                switch(Convert.ToInt32(rule.Protocol))
                {
                    case 6:
                        _form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "TCP";
                        break;
                    case 17:
                        _form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "UDP";
                        break;
                    case 1:
                        _form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "ICPMv4";
                        break;
                    case 58:
                        _form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "ICMPv6";
                        break;
                }
                switch (Convert.ToInt32(rule.Direction))
                {
                    case 1:
                        _form.firewall_grid.Rows[lastId].Cells["firewallDirectionInOut"].Value = "入";
                        break;
                    case 2:
                        _form.firewall_grid.Rows[lastId].Cells["firewallDirectionInOut"].Value = "出";
                        break;
                }
                switch (Convert.ToInt32(rule.Protocol))
                {
                    case 6:
                    case 17:
                    case 1:
                    case 58:
                        //符合 TCP、UDP，要顯示 ports
                        _form.firewall_grid.Rows[lastId].Cells["firewallLocalPorts"].Value = rule.LocalPorts;
                        _form.firewall_grid.Rows[lastId].Cells["firewallRemotePorts"].Value = rule.RemotePorts;
                        //_form.firewall_grid.Rows[lastId].Cells["firewallLocalAddresses"].Value = rule.LocalAddresses;
                        //_form.firewall_grid.Rows[lastId].Cells["firewallRemoteAddresses"].Value = rule.RemoteAddresses;
                        break;
                    default:
                        _form.firewall_grid.Rows[lastId].Cells["firewallLocalPorts"].Value = "";
                        _form.firewall_grid.Rows[lastId].Cells["firewallRemotePorts"].Value = "";
                        //_form.firewall_grid.Rows[lastId].Cells["firewallLocalAddresses"].Value = "";
                        //_form.firewall_grid.Rows[lastId].Cells["firewallRemoteAddresses"].Value = "";
                        break;
                }


                    //if (rule.Name == "My firewall rule")
                    //{

                    //}
                }
            is_running = false;
        }
    }
}