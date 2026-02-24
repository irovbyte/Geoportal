import 'package:flutter/material.dart';
import 'dart:ui';
import 'package:google_fonts/google_fonts.dart';
import '../services/auth_service.dart';
import '../services/lang_service.dart'; // Импортируем наш сервис перевода
import 'main_screen.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool _isLogin = true;
  bool _isServerDown = false;
  final _phoneController = TextEditingController(text: "+998");
  final _passwordController = TextEditingController();
  final _authService = AuthService();

  @override
  void initState() {
    super.initState();
    _checkServerStatus();
    _phoneController.addListener(_handlePhoneInput);
  }

  void _handlePhoneInput() {
    if (!_phoneController.text.startsWith("+998")) {
      _phoneController.text = "+998";
      _phoneController.selection = TextSelection.fromPosition(
        TextPosition(offset: _phoneController.text.length),
      );
    }
  }

  Future<void> _checkServerStatus() async {
    try {
      final result = await _authService.login("", "").timeout(const Duration(seconds: 3));
      if (result["message"] == "Ошибка сети") {
         setState(() => _isServerDown = true);
      }
    } catch (_) {
      setState(() => _isServerDown = true);
    }
  }

  // Виджет переключения языка
  Widget _buildLanguageSwitcher() {
    return SafeArea(
      child: Align(
        alignment: Alignment.topRight,
        child: Padding(
          padding: const EdgeInsets.all(10),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              _langButton("🇺🇿", "uz"),
              const SizedBox(width: 10),
              _langButton("🇷🇺", "ru"),
            ],
          ),
        ),
      ),
    );
  }

  Widget _langButton(String flag, String langCode) {
    bool isActive = LangService.currentLang == langCode;
    return GestureDetector(
      onTap: () => setState(() => LangService.currentLang = langCode),
      child: AnimatedContainer(
        duration: const Duration(milliseconds: 300),
        padding: const EdgeInsets.all(8),
        decoration: BoxDecoration(
          color: isActive ? Colors.white.withOpacity(0.3) : Colors.transparent,
          borderRadius: BorderRadius.circular(12),
          border: Border.all(color: isActive ? Colors.white : Colors.transparent),
        ),
        child: Text(flag, style: const TextStyle(fontSize: 24)),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          Container(
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
                colors: [Color(0xFF0A56C4), Color(0xFF3AB0C2), Color(0xFF3FE543)],
              ),
            ),
          ),
          
          _buildOrbit(200, Colors.white10, -50, -50),
          _buildOrbit(150, Colors.white12, null, null, bottom: 100, left: -30),

          // Добавляем переключатель языков поверх всего
          _buildLanguageSwitcher(),

          SafeArea(
            child: Center(
              child: SingleChildScrollView(
                padding: const EdgeInsets.symmetric(horizontal: 24),
                child: Column(
                  children: [
                    if (_isServerDown) _buildServerWarning(),
                    const SizedBox(height: 20),
                    
                    ClipRRect(
                      borderRadius: BorderRadius.circular(30),
                      child: BackdropFilter(
                        filter: ImageFilter.blur(sigmaX: 10, sigmaY: 10),
                        child: Container(
                          padding: const EdgeInsets.all(30),
                          decoration: BoxDecoration(
                            color: Colors.white.withOpacity(0.85),
                            borderRadius: BorderRadius.circular(30),
                            border: Border.all(color: Colors.white.withOpacity(0.2)),
                          ),
                          child: Column(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              _buildAppIcon(),
                              const SizedBox(height: 10),
                              Text(
                                LangService.t("app_name"), // Локализация заголовка
                                style: GoogleFonts.ruslanDisplay(
                                  fontSize: 28,
                                  color: const Color(0xFF0A56C4),
                                  letterSpacing: 3,
                                ),
                              ),
                              const SizedBox(height: 30),
                              
                              _buildToggleButtons(),
                              const SizedBox(height: 25),
                              
                              _buildInputFields(),
                              const SizedBox(height: 30),
                              
                              _buildSubmitButton(),
                              const SizedBox(height: 10),
                              
                              _buildSwitchModeButton(),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  // --- Вспомогательные UI методы ---

  Widget _buildOrbit(double size, Color color, double? top, double? right, {double? bottom, double? left}) {
    return Positioned(
      top: top, right: right, bottom: bottom, left: left,
      child: Container(width: size, height: size, decoration: BoxDecoration(shape: BoxShape.circle, color: color)),
    );
  }

  Widget _buildServerWarning() {
    return Container(
      padding: const EdgeInsets.all(10),
      decoration: BoxDecoration(color: Colors.redAccent.withOpacity(0.8), borderRadius: BorderRadius.circular(10)),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          const Icon(Icons.cloud_off, color: Colors.white, size: 20),
          const SizedBox(width: 10),
          Text(LangService.t("server_off"), style: const TextStyle(color: Colors.white, fontSize: 12)),
        ],
      ),
    );
  }

  Widget _buildToggleButtons() {
    return Row(
      children: [
        _toggleItem(LangService.t("login_btn"), _isLogin, () => setState(() => _isLogin = true)),
        _toggleItem(LangService.t("register_btn"), !_isLogin, () => setState(() => _isLogin = false)),
      ],
    );
  }

  Widget _toggleItem(String text, bool active, VoidCallback onTap) {
    return Expanded(
      child: InkWell(
        onTap: onTap,
        child: Column(
          children: [
            Text(text, style: TextStyle(fontSize: 12, fontWeight: FontWeight.bold, color: active ? const Color(0xFF0A56C4) : Colors.grey)),
            AnimatedContainer(
              duration: const Duration(milliseconds: 300),
              margin: const EdgeInsets.only(top: 5),
              height: 3, width: active ? 60 : 0,
              color: const Color(0xFF0A56C4),
            )
          ],
        ),
      ),
    );
  }

  Widget _buildInputFields() {
    return Column(
      children: [
        _customField(_phoneController, LangService.t("phone"), Icons.phone_android, type: TextInputType.phone),
        const SizedBox(height: 15),
        _customField(_passwordController, LangService.t("pass"), Icons.lock_outline, isPass: true),
      ],
    );
  }

  Widget _customField(TextEditingController ctrl, String hint, IconData icon, {bool isPass = false, TextInputType type = TextInputType.text}) {
    return TextField(
      controller: ctrl, obscureText: isPass, keyboardType: type,
      decoration: InputDecoration(
        prefixIcon: Icon(icon, color: const Color(0xFF0A56C4), size: 20),
        hintText: hint,
        filled: true, fillColor: Colors.grey[50],
        border: OutlineInputBorder(borderRadius: BorderRadius.circular(15), borderSide: BorderSide.none),
      ),
    );
  }

  Widget _buildSubmitButton() {
    return Container(
      width: double.infinity, height: 55,
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(15),
        gradient: LinearGradient(
          colors: _isServerDown ? [Colors.grey, Colors.grey] : [const Color(0xFF0A56C4), const Color(0xFF3AB0C2)],
        ),
      ),
      child: ElevatedButton(
        onPressed: _isServerDown ? null : _submitData,
        style: ElevatedButton.styleFrom(backgroundColor: Colors.transparent, shadowColor: Colors.transparent, shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15))),
        child: Text(_isLogin ? LangService.t("login_btn") : LangService.t("register_btn"), style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold)),
      ),
    );
  }

  Widget _buildSwitchModeButton() {
    return TextButton(
      onPressed: () => setState(() => _isLogin = !_isLogin),
      child: Text(
        _isLogin ? LangService.t("no_acc") : LangService.t("have_acc"),
        style: const TextStyle(color: Color(0xFF0A56C4), fontSize: 13),
      ),
    );
  }

  Widget _buildAppIcon() {
    return Image.asset('assets/logo.png', height: 80, errorBuilder: (_, __, ___) => const Icon(Icons.shield, size: 70, color: Color(0xFF0A56C4)));
  }

  void _submitData() async {
    final phone = _phoneController.text.trim();
    final pass = _passwordController.text.trim();
    if (!RegExp(r'^\+998\d{9}$').hasMatch(phone)) { _showMsg(LangService.t("error_phone"), Colors.red); return; }
    if (pass.length < 6) { _showMsg(LangService.t("error_pass"), Colors.red); return; }

    _showLoading();
    try {
      final result = _isLogin ? await _authService.login(phone, pass) : await _authService.register(phone, pass);
      if (!mounted) return;
      Navigator.pop(context);

      if (result["success"]) {
        Navigator.pushReplacement(context, MaterialPageRoute(builder: (_) => const MainScreen()));
      } else {
        _showMsg(result["message"].toString(), Colors.red);
      }
    } catch (e) {
      if (mounted) Navigator.pop(context);
      _showMsg("Ошибка соединения", Colors.red);
    }
  }

  void _showLoading() => showDialog(context: context, builder: (_) => const Center(child: CircularProgressIndicator(color: Colors.white)));
  void _showMsg(String m, Color c) => ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(m), backgroundColor: c, behavior: SnackBarBehavior.floating));
}