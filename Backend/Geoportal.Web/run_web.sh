#!/bin/bash
SESSION="geoportal_web"
PORT="80"

echo "🎨 Сборка и запуск WEB-интерфейса..."
screen -S $SESSION -X quit 2>/dev/null

# Очистка кэша и принудительное освобождение RAM
sync && echo 3 > /proc/sys/vm/drop_caches

cd ~/Geoportal/Backend/Geoportal.Web
rm -rf bin/ obj/

# Сборка без использования сервера Roslyn (VBCSCompiler) и в один поток
# Это предотвращает "зависание" CoreCompile из-за нехватки памяти
dotnet build -c Release /p:MaxCpuCount=1 /p:UseSharedCompilation=false

if [ $? -eq 0 ]; then
    screen -dmS $SESSION dotnet bin/Release/net9.0/Geoportal.Web.dll --urls "http://0.0.0.0:$PORT"
    echo "✅ WEB запущен на порту $PORT (Сессия: $SESSION)"
else
    echo "❌ Ошибка сборки WEB! Проверь верстку."
fi