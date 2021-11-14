namespace Paribu.Net.Enums
{
    public enum TransactionStatus
    {
        WaitingSmsApproval,
        WaitingEmailApproval,
        PendingConfirmation,
        Verified,
        Failed,
        Queued,
        Started
    }
}