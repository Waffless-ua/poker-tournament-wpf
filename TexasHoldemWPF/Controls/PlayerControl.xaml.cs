using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using TexasHoldemWPF.Models.Entities;

namespace TexasHoldemWPF.Controls
{
    public partial class PlayerControl : UserControl
    {
        #region Attached properties area
        public Player Player
        {
            get { return (Player)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }
        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("Player", typeof(Player), typeof(PlayerControl), new PropertyMetadata(null));

        public IEnumerable<Card> Cards
        {
            get { return (IEnumerable<Card>)GetValue(CardsProperty); }
            set { SetValue(CardsProperty, value); }
        }
        public static readonly DependencyProperty CardsProperty =
            DependencyProperty.Register("Cards", typeof(IEnumerable<Card>), typeof(PlayerControl), new PropertyMetadata(null));

        public bool IsDealer
        {
            get { return (bool)GetValue(IsDealerProperty); }
            set { SetValue(IsDealerProperty, value); }
        }
        public static readonly DependencyProperty IsDealerProperty =
            DependencyProperty.Register("IsDealer", typeof(bool), typeof(PlayerControl), new PropertyMetadata(false));

        public bool IsSmallBlind
        {
            get { return (bool)GetValue(IsSmallBlindProperty); }
            set { SetValue(IsSmallBlindProperty, value); }
        }
        public static readonly DependencyProperty IsSmallBlindProperty =
            DependencyProperty.Register("IsSmallBlind", typeof(bool), typeof(PlayerControl), new PropertyMetadata(false));

        public bool IsBigBlind
        {
            get { return (bool)GetValue(IsBigBlindProperty); }
            set { SetValue(IsBigBlindProperty, value); }
        }
        public static readonly DependencyProperty IsBigBlindProperty =
            DependencyProperty.Register("IsBigBlind", typeof(bool), typeof(PlayerControl), new PropertyMetadata(false));
        #endregion
        public bool HighlightBestHand
        {
            get { return (bool)GetValue(HighlightBestHandProperty); }
            set { SetValue(HighlightBestHandProperty, value); }
        }

        public static readonly DependencyProperty HighlightBestHandProperty =
            DependencyProperty.Register("HighlightBestHand", typeof(bool), typeof(PlayerControl), new PropertyMetadata(false));
        public bool ShowCards
        {
            get { return (bool)GetValue(ShowCardsProperty); }
            set { SetValue(ShowCardsProperty, value); }
        }
        public static readonly DependencyProperty ShowCardsProperty =
            DependencyProperty.Register("ShowCards", typeof(bool), typeof(PlayerControl), new PropertyMetadata(false));
        public PlayerControl()
        {
            InitializeComponent();
        }
    }
}