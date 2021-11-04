using System;
using System.Collections;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace system_status.App_code
{
    internal class firewall_info
    {
        private Form1 _form = null;
        public bool is_running = false;
        public string last_date = "";
        public DataTable dt = new DataTable();
        private bool isGridInit = false;

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
            if (_form.threads.ContainsKey("firewall_info"))
            {
                _form.threads["firewall_info"].Abort();
                _form.threads["firewall_info"] = null;
            }
            is_running = true;
            last_date = _form.my.time();
            _form.firewall_grid.AutoGenerateColumns = false; //這啥
            _form.firewall_grid.AllowUserToAddRows = false; //不能允許使用者自行調整
            _form.firewall_grid.RowHeadersVisible = false; //左邊空欄移除
            _form.firewall_grid.Dock = DockStyle.Fill; //自動展開到最大
            _form.firewall_grid.AllowDrop = false;
            _form.firewall_grid.ReadOnly = true;

            //_form.firewall_grid.Columns.Clear();
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
        ""firewallAllowBlock"":{""id"":""firewallAllowBlock"",""name"":""允許／阻擋"",""width"":120,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""firewallDirectionInOut"":{""id"":""firewallDirectionInOut"",""name"":""連入/連出"",""width"":120,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""firewallLocalPorts"":{""id"":""firewallLocalPorts"",""name"":""LocalPorts"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""firewallRemotePorts"":{""id"":""firewallRemotePorts"",""name"":""RemotePorts"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""firewallLocalAddresses"":{""id"":""firewallLocalAddresses"",""name"":""LocalAddresses"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    },
    {
        ""firewallRemoteAddresses"":{""id"":""firewallRemoteAddresses"",""name"":""RemoteAddresses"",""width"":160,""display"":true,""headerAlign"":""center"",""cellAlign"":""center""}
    }
]";

            //表格初始化
            if (isGridInit == false)
            {
                _form.my.grid_init(_form.firewall_grid, json_columns);
                isGridInit = true;
            }
            dt = _form.my.datatable_init(json_columns);
            //allow sorting
            foreach (DataGridViewColumn column in _form.firewall_grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            _form.threads["firewall_info"] = new Thread(() => run());
            _form.threads["firewall_info"].Start();

            //run();
        }

        public void run()
        {
            DataRow row = dt.NewRow();
            int step = 0;
            int lastId = 0;
            //_form.UpdateUI_DataGridGrid(_form.firewall_grid, "clear", "", "", -1);
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            IEnumerable Rules = fwPolicy2.Rules as IEnumerable;
            foreach (dynamic rule in Rules)
            {
                row = dt.NewRow();
                //_form.firewall_grid.Rows.Add();
                //_form.UpdateUI_DataGridGrid(_form.firewall_grid, "add", "", "", -1);
                //lastId = _form.firewall_grid.Rows.Count - 1;
                //_form.firewall_grid.Rows[lastId].Cells["firewallID"].Value = (lastId + 1);
                row["firewallID"] = ++step;

                //_form.firewall_grid.Rows[lastId].Cells["firewallName"].Value = rule.Name;
                row["firewallName"] = rule.Name;

                //_form.firewall_grid.Rows[lastId].Cells["firewallApplicationName"].Value = rule.ApplicationName;
                row["firewallApplicationName"] = rule.ApplicationName;

                //_form.firewall_grid.Rows[lastId].Cells["firewallServiceName"].Value = rule.ServiceName;
                row["firewallServiceName"] = rule.ServiceName;

                //_form.firewall_grid.Rows[lastId].Cells["firewallEnabled"].Value = rule.Enabled;
                row["firewallEnabled"] = rule.Enabled;

                switch (Convert.ToInt32(rule.Protocol))
                {
                    case 6:
                        //_form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "TCP";
                        row["firewallProtocol"] = "TCP";
                        break;

                    case 17:
                        //_form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "UDP";
                        row["firewallProtocol"] = "UDP";
                        break;

                    case 1:
                        //_form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "ICPMv4";
                        row["firewallProtocol"] = "ICPMv4";
                        break;

                    case 58:
                        //_form.firewall_grid.Rows[lastId].Cells["firewallProtocol"].Value = "ICMPv6";
                        row["firewallProtocol"] = "ICMPv6";
                        break;
                }
                switch (Convert.ToInt32(rule.Action))
                {
                    case 0: //Block
                        //_form.firewall_grid.Rows[lastId].Cells["firewallAllowBlock"].Value = "阻擋";
                        row["firewallAllowBlock"] = "阻擋";
                        break;

                    case 1: //Allow
                        //_form.firewall_grid.Rows[lastId].Cells["firewallAllowBlock"].Value = "允許";
                        row["firewallAllowBlock"] = "允許";
                        break;
                }
                switch (Convert.ToInt32(rule.Direction))
                {
                    case 1:
                        //_form.firewall_grid.Rows[lastId].Cells["firewallDirectionInOut"].Value = "入";
                        row["firewallDirectionInOut"] = "入";
                        break;

                    case 2:
                        //_form.firewall_grid.Rows[lastId].Cells["firewallDirectionInOut"].Value = "出";
                        row["firewallDirectionInOut"] = "出";
                        break;
                }
                switch (Convert.ToInt32(rule.Protocol))
                {
                    case 6:
                    case 17:
                    case 1:
                    case 58:
                        //符合 TCP、UDP，要顯示 ports
                        //_form.firewall_grid.Rows[lastId].Cells["firewallLocalPorts"].Value = rule.LocalPorts;
                        row["firewallLocalPorts"] = rule.LocalPorts;

                        //_form.firewall_grid.Rows[lastId].Cells["firewallRemotePorts"].Value = rule.RemotePorts;
                        row["firewallRemotePorts"] = rule.RemotePorts;

                        //_form.firewall_grid.Rows[lastId].Cells["firewallLocalAddresses"].Value = rule.LocalAddresses;
                        row["firewallLocalAddresses"] = rule.LocalAddresses;

                        //_form.firewall_grid.Rows[lastId].Cells["firewallRemoteAddresses"].Value = rule.RemoteAddresses;
                        row["firewallRemoteAddresses"] = rule.RemoteAddresses;
                        break;

                    default:
                        //_form.firewall_grid.Rows[lastId].Cells["firewallLocalPorts"].Value = "";
                        row["firewallLocalPorts"] = "";

                        //_form.firewall_grid.Rows[lastId].Cells["firewallRemotePorts"].Value = "";
                        row["firewallRemotePorts"] = "";

                        //_form.firewall_grid.Rows[lastId].Cells["firewallLocalAddresses"].Value = "";
                        row["firewallLocalAddresses"] = "";

                        //_form.firewall_grid.Rows[lastId].Cells["firewallRemoteAddresses"].Value = "";
                        row["firewallRemoteAddresses"] = "";
                        break;
                }

                //if (rule.Name == "My firewall rule")
                //{
                //}
                dt.Rows.Add(row);
            }
            _form.updateDGVUI(_form.firewall_grid, dt);
            _form.setStatusBar("就緒", 0);
            is_running = false;
        }
    }
}