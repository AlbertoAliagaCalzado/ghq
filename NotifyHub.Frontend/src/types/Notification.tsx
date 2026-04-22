export interface Notification {
    id: string;
    recipientId: string;
    actorId: string | null;
    type: string;
    message: string;
    isRead: boolean;
    createdAt: string;
}