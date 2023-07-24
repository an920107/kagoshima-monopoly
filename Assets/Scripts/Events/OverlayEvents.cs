using System;

namespace OverlayEvents {

    public class BuyLandCompleteEventArgs : EventArgs {
        public BuyLandCompleteEventArgs(bool toBuy, BlockController block = null, PlayerController player = null) {
            ToBuy = toBuy;
            Block = block;
            Player = player;
        }

        public bool ToBuy { get; }
        public BlockController Block { get; }
        public PlayerController Player { get; }
    }

    public delegate void BuyLandCompleteEventHandler(object sender, BuyLandCompleteEventArgs e);


    public class BuyFacilityCompleteEventArgs : EventArgs {
        public BuyFacilityCompleteEventArgs(bool toBuy, BlockController block = null, PlayerController player = null) {
            ToBuy = toBuy;
            Block = block;
            Player = player;
        }

        public bool ToBuy { get; }
        public BlockController Block { get; }
        public PlayerController Player { get; }
    }

    public delegate void BuyFacilityCompleteEventHandler(object sender, BuyFacilityCompleteEventArgs e);


    public class NoMoneyCompleteEventArgs : EventArgs {
        public NoMoneyCompleteEventArgs() {

        }
    }

    public delegate void NoMoneyCompleteEventHandler(object sender, NoMoneyCompleteEventArgs e);


    public class PayMoneyCompleteEventArgs : EventArgs {
        public PayMoneyCompleteEventArgs(BlockController block, PlayerController from, PlayerController to) {
            Block = block;
            From = from;
            To = to;
        }

        public BlockController Block { get; }
        public PlayerController From { get; }
        public PlayerController To { get; }
    }

    public delegate void PayMoneyCompleteEventHandler(object sender, PayMoneyCompleteEventArgs e);
}