import React, { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import * as signalR from '@microsoft/signalr';
import { Notification } from '../types/Notification';

interface NotificationContextType {
    notifications: Notification[];
    isConnected: boolean;
}

const NotificationContext = createContext<NotificationContextType | undefined>(undefined);

const MY_USER_ID = "11111111-1111-1111-1111-111111111111";
const HUB_URL = "http://localhost:5294/hubs/notifications";

export const NotificationProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [notifications, setNotifications] = useState<Notification[]>([]);
    const [isConnected, setIsConnected] = useState(false);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(HUB_URL)
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveNotification", (notification: Notification) => {
            setNotifications(prev => [notification, ...prev]);
        });

        const startConnection = async () => {
            try {
                await connection.start();
                setIsConnected(true);
                await connection.invoke("IdentifyUser", MY_USER_ID);
            } catch (err) {
                console.error("Error al conectar con SignalR:", err);
            }
        };

        startConnection();

        return () => {
            connection.stop();
        };
    }, []);

    return (
        <NotificationContext.Provider value={{ notifications, isConnected }}>
            {children}
        </NotificationContext.Provider>
    );
};

export const useNotifications = () => {
    const context = useContext(NotificationContext);
    if (context === undefined) {
        throw new Error("useNotifications debe usarse dentro de un NotificationProvider");
    }
    return context;
};