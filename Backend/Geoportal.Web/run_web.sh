#!/bin/bash
SESSION="geoportal_web"
PORT="80"

echo "🎨 Сборка и запуск WEB-интерфейса..."
screen -S $SESSION -X quit 2>/dev/null

cd ~/Geoportal/Backend/Geoportal.Web
rm -rf bin/ obj/
dotnet build

if [ $? -eq 0 ]; then
    screen -dmS $SESSION dotnet run --urls "http://0.0.0.0:$PORT"
    echo "✅ WEB запущен на порту $PORT (Сессия: $SESSION)"
else
    echo "❌ Ошибка сборки WEB! Проверь верстку."
fi