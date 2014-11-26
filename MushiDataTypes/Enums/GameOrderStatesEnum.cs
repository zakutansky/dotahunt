using MushiDataTypes.Attributes;

namespace MushiDataTypes.Enums
{
    public enum GameOrderStatesEnum
    {
        NotSelected,

        [AssociatedStatus(PlayerStatus.PendingInvitation)]
        PendingInvitation,

        [AssociatedStatus(PlayerStatus.InvitationAccept)]
        InvitationAccept,

        [AssociatedStatus(PlayerStatus.PendingPayment)]
        PendingPayment,

        [AssociatedStatus(PlayerStatus.InGame)]
        InGame,

        PaymentDeclined,

        Refund,

        PaymentFailed,

        OrderInProgress
    }
}