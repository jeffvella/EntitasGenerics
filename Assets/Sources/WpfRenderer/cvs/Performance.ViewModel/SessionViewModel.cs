using Performance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.ViewModels
{
    public class SessionViewModel : NotifyBase
    {
        private long _score;
        private long _actions;

        public SessionViewModel()
        { 

        }

        public long Score
        {
            get => _score;
            set => SetField(ref _score, value);
        }

        public long Actions
        {
            get => _actions;
            set => SetField(ref _actions, value);
        }
    }
}

