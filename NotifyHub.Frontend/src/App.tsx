import { useState } from 'react';
import { NotificationBadge } from './components/NotificationBadge';
import { NotificationList } from './components/NotificationList';
import { NotificationSender } from './components/NotificationSender';

function App() {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  return (
    <div className="min-h-screen bg-gray-50 font-sans">
      <nav className="bg-white shadow-sm px-6 py-4 flex justify-between items-center relative">
        <h1 className="text-xl font-bold text-gray-800">NotifyHub App</h1>

        <div className="relative">
          <NotificationBadge onClick={() => setIsDropdownOpen(!isDropdownOpen)} />

          {isDropdownOpen && (
            <div className="absolute right-0 mt-2 w-80 bg-white rounded-lg shadow-xl border border-gray-100 overflow-hidden z-50">
              <div className="bg-gray-50 px-4 py-3 border-b border-gray-100 flex justify-between items-center">
                <h3 className="font-semibold text-gray-700">Notificaciones</h3>
              </div>
              <NotificationList />
            </div>
          )}
        </div>
      </nav>

      <main className="max-w-xl mx-auto mt-10 p-6">
        <div className="mb-8">
          <h2 className="text-2xl font-bold text-gray-800 mb-2">Panel de Control</h2>
          <p className="text-gray-600">
            Usa este formulario para simular una acción de otro usuario. La notificación viajará a la API REST y volverá instantáneamente a ti vía WebSockets.
          </p>
        </div>

        <NotificationSender />

      </main>
    </div>
  );
}

export default App;