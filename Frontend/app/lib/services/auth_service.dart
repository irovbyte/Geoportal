import 'package:dio/dio.dart';
import 'package:device_info_plus/device_info_plus.dart';
import 'dart:io';

class AuthService {
  // Твой URL бэкенда. Порт 5001, так как мы на .NET 9
  final String _baseUrl = "http://136.113.150.143:5001/api/auth";
  final Dio _dio = Dio();

  // Функция для получения уникального ID устройства
  Future<String> getUniqueId() async {
    DeviceInfoPlugin deviceInfo = DeviceInfoPlugin();
    if (Platform.isAndroid) {
      var androidInfo = await deviceInfo.androidInfo;
      return androidInfo.id; // Уникальный ID Android
    } else if (Platform.isWindows) {
      var windowsInfo = await deviceInfo.windowsInfo;
      return windowsInfo.deviceId; // ID железа Windows
    }
    return "web_user";
  }

  // Метод регистрации
  Future<Map<String, dynamic>> register(String phone, String password) async {
    try {
      String deviceId = await getUniqueId();
      
      var response = await _dio.post("$_baseUrl/register", data: {
        "phoneNumber": phone,
        "password": password,
        "deviceId": deviceId
      });

      return {"success": true, "data": response.data};
    } on DioException catch (e) {
      return {"success": false, "message": e.response?.data ?? "Ошибка сервера"};
    }
  }
}