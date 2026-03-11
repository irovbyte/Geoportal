#!/bin/bash

# Очистка экрана
clear

echo "======================================="
echo "    🚀 GEOPORTAL RUN CONTROL v1.0"
echo "======================================="
echo "1) Собрать и запустить API (Port: 5001)"
echo "2) Собрать и запустить WEB (Port: 80)"
echo "3) Перезапустить ВСЁ (API + WEB)"
echo "4) Остановить всё (Kill Sessions)"
echo "---------------------------------------"
read -p "Выберите действие (1-4): " choice

# Глобальные настройки
API_SESSION="geoportal_api"
WEB_SESSION="geoportal_web"
API_PORT="5001"
WEB_PORT="80"

# Функция для запуска API
run_api() {
    echo "🛠 Сборка и запуск API..."
    screen -S $API_SESSION -X quit 2>/dev/null
    
    sync && echo 3 > /proc/sys/vm/drop_caches # Чистим RAM
    
    cd ~/Geoportal/Geoportal.Api
    rm -rf bin/ obj/
    
    dotnet build -c Debug # API лучше в дебаге для логов
    
    if [ $? -eq 0 ]; then
        screen -dmS $API_SESSION dotnet run --no-build --urls "http://0.0.0.0:$API_PORT"
        echo "✅ API запущено (Сессия: $API_SESSION, Порт: $API_PORT)"
    else
        echo "❌ Ошибка сборки API!"
    fi
    cd ..
}

# Функция для запуска WEB
run_web() {
    echo "🎨 Сборка и запуск WEB-интерфейса..."
    screen -S $WEB_SESSION -X quit 2>/dev/null
    
    sync && echo 3 > /proc/sys/vm/drop_caches
    
    cd ~/Geoportal/Geoportal.Web
    rm -rf bin/ obj/
    
    # Собираем в Release для скорости сайта
    dotnet build -c Release /p:MaxCpuCount=1 /p:UseSharedCompilation=false
    
    if [ $? -eq 0 ]; then
        screen -dmS $WEB_SESSION dotnet bin/Release/net10.0/Geoportal.Web.dll --urls "http://0.0.0.0:$WEB_PORT"
        echo "✅ WEB запущен (Сессия: $WEB_SESSION, Порт: $WEB_PORT)"
    else
        echo "❌ Ошибка сборки WEB!"
    fi
    cd ..
}

# Логика выбора
case $choice in
    1)
        run_api
        ;;
    2)
        run_web
        ;;
    3)
        run_api
        run_web
        ;;
    4)
        screen -S $API_SESSION -X quit 2>/dev/null
        screen -S $WEB_SESSION -X quit 2>/dev/null
        echo "🛑 Все процессы остановлены."
        ;;
    *)
        echo "❌ Неверный выбор."
        exit 1
        ;;
esac

echo "---------------------------------------"
echo "Списки активных сессий (screen -ls):"
screen -ls | grep geoportal