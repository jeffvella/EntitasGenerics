using Entitas.MatchLine;
using Performance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Performance.ViewModel.Resources
{
    // WPF binding works best with a class with properties and Notifications.
    // Struct members can still be accessed via converters but its a pain.

    [DataContract]
    public class BoardSize : NotifyBase
    {
        private int _x;
        private int _y;

        public BoardSize(int x, int y)
        {
            X = x;
            Y = y;
        }

        [DataMember]
        public int X
        {
            get => _x;
            set => SetField(ref _x, value);
        }

        [DataMember]
        public int Y
        {
            get => _y;
            set => SetField(ref _y, value);
        }

        public static explicit operator BoardSize(GridSize gridSize)
        {
            return new BoardSize(gridSize.x, gridSize.y);
        }

        public static implicit operator GridSize(BoardSize boardSize)
        {
            return new GridSize { x = boardSize.X, y = boardSize.Y };
        }
    }
}


