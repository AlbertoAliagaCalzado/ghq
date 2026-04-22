import React from 'react';
import { useNotifications } from '../contexts/NotificationContext';

export const NotificationList: React.FC = () => {
    const { notifications } = useNotifications();

    if (notifications.length === 0) {
        return (
            <div className="p-4 text-center text-gray-500 text-sm">
                No tienes notificaciones nuevas.
            </div>
        );
    }

    return (
        <div className="max-h-96 overflow-y-auto">
            {notifications.map((notification) => (
                <div
                    key={notification.id}
                    className={`p-4 border-b border-gray-100 transition-colors ${notification.isRead ? 'bg-white' : 'bg-blue-50'}`}
                >
                    <div className="flex justify-between items-start mb-1">
                        <span className="text-xs font-semibold text-blue-600 uppercase tracking-wider">
                            {notification.type}
                        </span>
                        <span className="text-xs text-gray-400">
                            {new Date(notification.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                        </span>
                    </div>
                    <p className="text-sm text-gray-800">
                        {notification.message}
                    </p>
                </div>
            ))}
        </div>
    );
};