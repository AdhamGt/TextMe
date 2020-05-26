using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShareScreen
{
    public class ShareAccount
    {
        private string ShareUsername;
        private string SharePassword;
        private ShareState State;

        public ShareState UserState
        {
            get
            {
                return State;
            }

            set
            {
                State = value;
            }
        }


        public string SharesUsername
        {
            get
            {
                return ShareUsername;
            }

            set
            {
                if (!value.Contains(" ") && Regex.IsMatch(value, @"^[\w]+$"))
                {
                    ShareUsername = value;
                }
            }
        }

        public string SharesPassword
        {
            get
            {
                return SharePassword;
            }

            set
            {
                if (!value.Contains(" ") && Regex.IsMatch(value, @"^[\p{L}\p{N}]+$"))
                {
                    SharePassword = value;
                }
            }
        }

    }
  public enum ShareState
    {
        Disconnected =0,
        Idle = 1,
        Queueing = 2,
        Connecting = 3,
        Connected = 4,
            Hosting = 5
    }
}
