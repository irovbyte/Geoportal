#!/bin/bash
SESSION="geoportal_api"
PORT="5001"

echo "🛠 Сборка и запуск API..."
screen -S $SESSION -X quit 2>/dev/null # Убиваем только старое API

# Чистим только папку API, чтобы не трогать Web
cd ~/Geoportal/Backend/Geoportal.Api
rm -rf bin/ obj/
dotnet build

if [ $? -eq 0 ]; then
    screen -dmS $SESSION dotnet run --urls "http://0.0.0.0:$PORT"
    echo "✅ API запущено на порту $PORT (Сессия: $SESSION)"
else
    echo "❌ Ошибка сборки API! Проверь код."
fi