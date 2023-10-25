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


    public class StatusCompleteEventArgs : EventArgs {
        public StatusCompleteEventArgs() { }
    }

    public delegate void StatusCompleteEventHandler(object sender, StatusCompleteEventArgs e);


    public class SalaryCompleteEventArgs : EventArgs {
        public SalaryCompleteEventArgs(BlockController startBlock, PlayerController player) {
            StartBlock = startBlock;
            Player = player;
        }

        public BlockController StartBlock { get; }
        public PlayerController Player { get; }
    }

    public delegate void SalaryCompleteEventHandler(object sender, SalaryCompleteEventArgs e);


    public class JailCompleteEventArgs : EventArgs {
        public JailCompleteEventArgs(PlayerController player) {
            Player = player;
        }

        public PlayerController Player { get; }
    }

    public delegate void JailCompleteEventHandler(object sender, JailCompleteEventArgs e);
}