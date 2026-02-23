import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../services/auth_service.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool _isLogin = true; 
  
  // Сразу ставим +998 в контроллер
  final _phoneController = TextEditingController(text: "+998");
  final _passwordController = TextEditingController();

  @override
  void initState() {
    super.initState();
    // Слушатель, чтобы пользователь не мог стереть +998
    _phoneController.addListener(() {
      if (!_phoneController.text.startsWith("+998")) {
        _phoneController.text = "+998";
        _phoneController.selection = TextSelection.fromPosition(
          TextPosition(offset: _phoneController.text.length),
        );
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.symmetric(horizontal: 30),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                const Icon(Icons.location_on_rounded, size: 80, color: Colors.blueAccent),
                const SizedBox(height: 10),
                Text(
                  "GEOPORTAL",
                  style: GoogleFonts.ruslanDisplay(
                    fontSize: 32,
                    color: Colors.blueAccent,
                    letterSpacing: 2,
                  ),
                ),
                const SizedBox(height: 40),
                Text(
                  _isLogin ? "С возвращением!" : "Создать аккаунт",
                  style: GoogleFonts.montserrat(
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 30),

                // Поле телефона с фиксированным +998
                TextField(
                  controller: _phoneController,
                  keyboardType: TextInputType.phone,
                  decoration: InputDecoration(
                    prefixIcon: const Icon(Icons.phone_android),
                    hintText: "Номер телефона",
                    filled: true,
                    fillColor: Colors.grey[100],
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(15),
                      borderSide: BorderSide.none,
                    ),
                  ),
                ),
                const SizedBox(height: 15),

                TextField(
                  controller: _passwordController,
                  obscureText: true,
                  decoration: InputDecoration(
                    prefixIcon: const Icon(Icons.lock_outline),
                    hintText: "Пароль",
                    filled: true,
                    fillColor: Colors.grey[100],
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(15),
                      borderSide: BorderSide.none,
                    ),
                  ),
                ),
                const SizedBox(height: 25),

                SizedBox(
                  width: double.infinity,
                  height: 55,
                  child: ElevatedButton(
                    onPressed: _submitData,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.blueAccent,
                      foregroundColor: Colors.white,
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
                    ),
                    child: Text(_isLogin ? "Войти" : "Зарегистрироваться"),
                  ),
                ),

                TextButton(
                  onPressed: () => setState(() => _isLogin = !_isLogin),
                  child: Text(_isLogin ? "Нет аккаунта? Регистрация" : "Уже есть аккаунт? Войти"),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  void _submitData() async {
    final phone = _phoneController.text.trim();
    final pass = _passwordController.text.trim();

    // Проверка, что введено что-то кроме +998
    if (phone == "+998" || pass.isEmpty) {
      _showError("Введите номер и пароль");
      return;
    }

    _showLoading();

    try {
      final authService = AuthService();
      // Добавляем таймаут 10 секунд, чтобы не фризило вечно
      final result = await authService.register(phone, pass).timeout(
        const Duration(seconds: 10),
        onTimeout: () => {"success": false, "message": "Сервер не отвечает"},
      );

      if (!mounted) return;
      Navigator.pop(context); // Закрыть Loader

      if (result["success"]) {
        _showSuccess("Успешно!");
      } else {
        _showError(result["message"].toString());
      }
    } catch (e) {
      if (mounted) Navigator.pop(context);
      _showError("Ошибка сети. Проверьте соединение.");
    }
  }

  void _showLoading() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => const Center(child: CircularProgressIndicator()),
    );
  }

  void _showError(String msg) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(msg), backgroundColor: Colors.red),
    );
  }

  void _showSuccess(String msg) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(msg), backgroundColor: Colors.green),
    );
  }
}